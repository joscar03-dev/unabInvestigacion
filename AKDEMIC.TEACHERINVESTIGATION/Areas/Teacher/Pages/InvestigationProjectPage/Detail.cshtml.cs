using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Interfaces;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.ViewModels.InvestigationProjectViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.Pages.InvestigationProjectPage
{
    [Authorize(Roles = GeneralConstants.ROLES.RESEARCHERS)]
    public class DetailModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataTablesService _dataTablesService;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public DetailModel(
            AkdemicContext context,
            IDataTablesService dataTablesService,
            UserManager<ApplicationUser> userManager,
            IOptions<CloudStorageCredentials> storageCredentials
        )
        {
            _context = context;
            _dataTablesService = dataTablesService;
            _userManager = userManager;
            _storageCredentials = storageCredentials;
        }

        [BindProperty]
        public InvestigationProjectViewModel Input { get; set; }



        public async Task<IActionResult> OnGetAsync(Guid investigationProjectId)
        {
            var user = await _userManager.GetUserAsync(User);

            var investigationProject = await _context.InvestigationProjects
                .Where(x => x.Id == investigationProjectId && x.InvestigationConvocationPostulant.UserId == user.Id)
                .Select(x => new
                {
                    x.Id,
                    x.GeneralGoal,
                    x.InvestigationConvocationPostulant.ProjectTitle,
                    x.SpecificGoal,
                    x.GanttDiagramUrl,
                    x.ExecutionAddress,
                    x.InvestigationProjectTypeId,
                    x.FinalReportUrl,
                    InvestigationProjectType = x.InvestigationProjectType.Name ?? "",
                    x.InvestigationConvocationPostulant.User.FullName,
                    x.InvestigationConvocationPostulantId,
                    FinancingInvestigation = x.InvestigationConvocationPostulant.FinancingInvestigation.Name ?? ""
                })
                .FirstOrDefaultAsync();

            if (investigationProject == null)
                return RedirectToPage("Index");

            var values = await _context.Configurations.ToDictionaryAsync(x => x.Key, x => x.Value);

            Input = new InvestigationProjectViewModel
            {
                InvestigationProjectId = investigationProject.Id,
                GeneralGoal = investigationProject.GeneralGoal,
                SpecificGoal = investigationProject.SpecificGoal,
                GanttDiagramUrl = investigationProject.GanttDiagramUrl,
                FinalReportUrl = investigationProject.FinalReportUrl,
                ExecutionAddress = investigationProject.ExecutionAddress,
                InvestigationProjectTypeId = investigationProject.InvestigationProjectTypeId,
                ProjectTitle = investigationProject.ProjectTitle,
                FinancingInvestigation = investigationProject.FinancingInvestigation,
                ArticleUrl = GetConfigurationValue(values, ConfigurationConstants.TEACHERINVESTIGATION.RULES_ARTICLESCIENTIFIC)
            };
            
            return Page();
        }

        public async Task<IActionResult> OnGetTaskDatatableAsync(Guid investigationProjectId)
        {
            var user = await _userManager.GetUserAsync(User);

            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<DOMAIN.Entities.TeacherInvestigation.InvestigationProjectTask, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Description;
                    break;
                case "1":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
            }

            var query = _context.InvestigationProjectTasks
                .Where(x => x.InvestigationProjectId == investigationProjectId && x.InvestigationProject.InvestigationConvocationPostulant.UserId == user.Id)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    CreatedAt = x.CreatedAt.HasValue ? x.CreatedAt.ToLocalDateFormat() : "",
                    TaskName = x.Description,
                    x.User.FullName
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            int recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
            return new OkObjectResult(result);
        }
        public async Task<IActionResult> OnPostFinalDocumentAsync(Guid investigationProjectId)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            if (Input.FinalReportFile == null)
                return new BadRequestObjectResult("Debe subir un archivo");


            var investigationProjects = await _context.InvestigationProjects.Where(x => x.Id == investigationProjectId).FirstOrDefaultAsync();
            if (investigationProjects == null)
                return new BadRequestObjectResult("Sucedio un error");

            var storage = new CloudStorageService(_storageCredentials);

            string fileUrl = await storage.UploadFile(Input.FinalReportFile.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.INVESTIGATIONCONVOCATION_DOCUMENTS,
                    Path.GetExtension(Input.FinalReportFile.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);


            investigationProjects.FinalReportUrl = fileUrl;
            

            await _context.SaveChangesAsync();

            return new OkResult();


        }
        public async Task<IActionResult> OnGetExpenseDatatableAsync(Guid investigationProjectId)
        {
            var user = await _userManager.GetUserAsync(User);

            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<InvestigationProjectExpense, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Description;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Amount;
                    break;
                case "2":
                    orderByPredicate = (x) => x.InvestigationProjectTask.InvestigationProject.InvestigationConvocationPostulant.FinancingInvestigation.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.InvestigationProjectTask.Description;
                    break;
                case "4":
                    orderByPredicate = (x) => x.ExpenseCode;
                    break;
                case "5":
                    orderByPredicate = (x) => x.ProductType;
                    break;
            }

            var query = _context.InvestigationProjectExpenses
                .Where(x => x.InvestigationProjectTask.InvestigationProjectId == investigationProjectId && x.InvestigationProjectTask.InvestigationProject.InvestigationConvocationPostulant.UserId == user.Id)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    CreatedAt = x.CreatedAt.HasValue ? x.CreatedAt.ToLocalDateFormat() : "",
                    x.Description,
                    x.Amount,
                    x.ExpenseCode,
                    x.ProductType,
                    TaskDescription = x.InvestigationProjectTask.Description,
                    FinancingInvestigation = x.InvestigationProjectTask.InvestigationProject.InvestigationConvocationPostulant.FinancingInvestigation.Name ?? ""
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            int recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnGetReportDatatableAsync(Guid investigationProjectId)
        {
            var user = await _userManager.GetUserAsync(User);

            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<DOMAIN.Entities.TeacherInvestigation.InvestigationProjectReport, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.ExpirationDate;
                    break;
            }

            var query = _context.InvestigationProjectReports
                .Where(x => x.InvestigationProjectId == investigationProjectId && x.InvestigationProject.InvestigationConvocationPostulant.UserId == user.Id)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    CreatedAt = x.CreatedAt.HasValue ? x.CreatedAt.ToLocalDateFormat() : "",
                    x.Name,
                    ExpirationDate = x.ExpirationDate.ToLocalDateFormat(),
                    x.ReportUrl
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            int recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnGetProjectTeamMemberDatatableAsync(Guid investigationProjectId)
        {
            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<DOMAIN.Entities.TeacherInvestigation.InvestigationProjectTeamMember, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.CvFilePath;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Objectives;
                    break;

            }

            var query = _context.InvestigationProjectTeamMembers
                .Where(x => x.InvestigationProjectId == investigationProjectId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.User.FullName,
                    x.CvFilePath,
                    RoleName=x.TeamMemberRole.Name,
                    x.Objectives

                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            int recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnGetProjectScientificArticleDatatableAsync(Guid investigationProjectId)
        {
            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<DOMAIN.Entities.TeacherInvestigation.ScientificArticle, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Title;
                    break;
                case "1":
                    orderByPredicate = (x) => x.FilePath;
                    break;

            }

            var query = _context.ScientificArticles
                .Where(x => x.InvestigationProjectId == investigationProjectId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.FilePath,
                    x.Title,
                    x.InvestigationProjectId
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            int recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnPostSaveProjectAsync() 
        {
            var investigationProject = await _context.InvestigationProjects.Where(x => x.Id == Input.InvestigationProjectId).FirstOrDefaultAsync();

            if (investigationProject == null)
                return new BadRequestObjectResult("Sucedio un error");

            investigationProject.GeneralGoal = Input.GeneralGoal;
            investigationProject.SpecificGoal = Input.SpecificGoal;
            investigationProject.ExecutionAddress = Input.ExecutionAddress;
            investigationProject.InvestigationProjectTypeId = Input.InvestigationProjectTypeId;
            
            //TODO OTROS CAMPOS

            var storage = new CloudStorageService(_storageCredentials);

            if (Input.GanttDiagramFile != null)
            {
                string fileUrl = await storage.UploadFile(Input.GanttDiagramFile.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.INVESTIGATIONCONVOCATION_DOCUMENTS,
                                    Path.GetExtension(Input.GanttDiagramFile.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);

                investigationProject.GanttDiagramUrl = fileUrl;
            }

            await _context.SaveChangesAsync();

            var result = new
            {
                investigationProject.GanttDiagramUrl
            };
            return new OkObjectResult(result);
        }

        #region InvestigationProjectTeamMembers
        public async Task<IActionResult> OnPostCreateProjectTeamMembersAsync(InvestigationProjectCreateTeamMembersViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");


            if (viewModel.File == null)
                return new BadRequestObjectResult("Debe seleccionar un archivo");

            if (!await _context.Users.AnyAsync(x => x.Id == viewModel.UserId))
                return new BadRequestObjectResult("Sucedio un error");

            if (!await _context.TeamMemberRoles.AnyAsync(x => x.Id == viewModel.TeamMemberRoleId))
                return new BadRequestObjectResult("Sucedio un error");

            if (!await _context.InvestigationProjects.AnyAsync(x => x.Id == viewModel.InvestigationProjectId))
                return new BadRequestObjectResult("Sucedio un error");

            var storage = new CloudStorageService(_storageCredentials);

            string fileUrl = await storage.UploadFile(viewModel.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.INVESTIGATIONCONVOCATION_DOCUMENTS,
                    Path.GetExtension(viewModel.File.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);

            var investigationProjectTeamMember = new InvestigationProjectTeamMember
            {
                InvestigationProjectId = viewModel.InvestigationProjectId,
                UserId= viewModel.UserId,
                TeamMemberRoleId = viewModel.TeamMemberRoleId,
                Objectives = viewModel.Objectives,
                CvFilePath = fileUrl
            };


            await _context.InvestigationProjectTeamMembers.AddAsync(investigationProjectTeamMember);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditProjectTeamMembersAsync(InvestigationProjectEditTeamMembersViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var investigationProjectTeamMember = await _context.InvestigationProjectTeamMembers.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (investigationProjectTeamMember == null) return new BadRequestObjectResult("Sucedio un error");

            investigationProjectTeamMember.TeamMemberRoleId = viewModel.TeamMemberRoleId;
            investigationProjectTeamMember.Objectives = viewModel.Objectives;

            var storage = new CloudStorageService(_storageCredentials);

            if (viewModel.File != null)
            {
                string fileUrl = await storage.UploadFile(viewModel.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.INVESTIGATIONCONVOCATION_DOCUMENTS,
                                    Path.GetExtension(viewModel.File.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);

                investigationProjectTeamMember.CvFilePath = fileUrl;
            }

            await _context.SaveChangesAsync();

            return new OkResult();
        }


        public async Task<IActionResult> OnPostDeleteProjectTeamMembersAsync(Guid id)
        {
            var investigationProjectTeamMember = await _context.InvestigationProjectTeamMembers.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (investigationProjectTeamMember == null) return new BadRequestObjectResult("Sucedio un error");

            _context.InvestigationProjectTeamMembers.Remove(investigationProjectTeamMember);
            await _context.SaveChangesAsync();
            return new OkResult();
        }


        public async Task<IActionResult> OnGetDetailProjectTeamMembersAsync(Guid id)
        {
            var investigationProjectTeamMember = await _context.InvestigationProjectTeamMembers.Where(x => x.Id == id)
                .Select(x=> new
                {
                    x.Id,
                    x.InvestigationProjectId,
                    x.User.FullName,
                    x.TeamMemberRoleId,
                    x.CvFilePath,
                    x.Objectives
                })
                .FirstOrDefaultAsync();

            if (investigationProjectTeamMember == null) return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                investigationProjectTeamMember.Id,
                investigationProjectTeamMember.InvestigationProjectId,
                investigationProjectTeamMember.FullName,
                investigationProjectTeamMember.TeamMemberRoleId,
                investigationProjectTeamMember.CvFilePath,
                investigationProjectTeamMember.Objectives
            };

            return new OkObjectResult(result);
        }


        #endregion

        #region InvestigationProjectArticleScientific
        public async Task<IActionResult> OnPostCreateProjectScientificArticle(ScientificArticleViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");


            if (viewModel.File == null)
                return new BadRequestObjectResult("Debe seleccionar un archivo");

            var storage = new CloudStorageService(_storageCredentials);

            string fileUrl = await storage.UploadFile(viewModel.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.INVESTIGATIONCONVOCATION_DOCUMENTS,
                    Path.GetExtension(viewModel.File.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);

            var scientificArticle = new ScientificArticle
            {
                InvestigationProjectId = viewModel.InvestigationProjectId,
                Title = viewModel.Title,
                FilePath = fileUrl
            };


            await _context.ScientificArticles.AddAsync(scientificArticle);
            await _context.SaveChangesAsync();

            return new OkResult();
        }
        public async Task<IActionResult> OnPostEditProjectScientificArticleAsync(ScientificArticleViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var scientificArticles = await _context.ScientificArticles.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (scientificArticles == null) return new BadRequestObjectResult("Sucedio un error");

            scientificArticles.Title = viewModel.Title;
            scientificArticles.FilePath = viewModel.FilePath;

            var storage = new CloudStorageService(_storageCredentials);

            if (viewModel.File != null)
            {
                string fileUrl = await storage.UploadFile(viewModel.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.INVESTIGATIONCONVOCATION_DOCUMENTS,
                                    Path.GetExtension(viewModel.File.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);

                scientificArticles.FilePath = fileUrl;
            }

            await _context.SaveChangesAsync();

            return new OkResult();
        }


        public async Task<IActionResult> OnPostDeleteProjectScientificArticleAsync(Guid id)
        {
            var scientificArticles = await _context.ScientificArticles.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (scientificArticles == null) return new BadRequestObjectResult("Sucedio un error");

            _context.ScientificArticles.Remove(scientificArticles);
            await _context.SaveChangesAsync();
            return new OkResult();
        }


        public async Task<IActionResult> OnGetDetailProjectScientificArticleAsync(Guid id)
        {
            var scientificArticles = await _context.ScientificArticles.Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.InvestigationProjectId,
                    x.Title,
                    x.FilePath,
                })
                .FirstOrDefaultAsync();

            if (scientificArticles == null) return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                scientificArticles.Id,
                scientificArticles.InvestigationProjectId,
                scientificArticles.Title,
                scientificArticles.FilePath,
            };

            return new OkObjectResult(result);
        }

        #endregion

        #region InvestigationProjectReport-Entregables

        public async Task<IActionResult> OnGetDetailUploadprojectreportDocumentFile(Guid id)
        {
            var investigationProjectReport = await _context.InvestigationProjectReports.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (investigationProjectReport == null)
                return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                investigationProjectReport.Name,
                ExpirationDate = investigationProjectReport.ExpirationDate.ToLocalDateFormat(),
                investigationProjectReport.ReportUrl,
                investigationProjectReport.InvestigationProjectId,
                investigationProjectReport.Id,

            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnPostCreateUploadProjectReportDocumentFile(InvestigationProjectReportEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise los campos del formulario");

            var investigationProjectReport = await _context.InvestigationProjectReports.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (investigationProjectReport == null)
                return new BadRequestObjectResult("Sucedio un error");

            if (investigationProjectReport.ExpirationDate < DateTime.UtcNow)
                return new BadRequestObjectResult("El entregable esta fuera de fecha");

            var storage = new CloudStorageService(_storageCredentials);

            if(viewModel.File != null)
            {
                string fileUrl = await storage.UploadFile(viewModel.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.INVESTIGATIONCONVOCATION_DOCUMENTS,
                                    Path.GetExtension(viewModel.File.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);

                investigationProjectReport.ReportUrl = fileUrl;
                await _context.SaveChangesAsync();
            }

            return new OkResult();
        }

        #endregion

        #region InvestigationProjectTask-Tareas

        public async Task<IActionResult> OnPostCreateTaskAsync(InvestigationProjectTaskViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult("Revise el formulario");
            }

            var user = await _userManager.GetUserAsync(User);

            var investigationProject = await _context.InvestigationProjects
                .Where(x => x.Id == viewModel.InvestigationProjectId)
                .FirstOrDefaultAsync();

            //validacion de ser el usuario del proyecto o miembro del equipo?

            if (investigationProject == null)
                return new BadRequestObjectResult("Sucedio un error");

            var investigationProjectTask = new InvestigationProjectTask
            {
                Description = viewModel.Description,
                InvestigationProjectId = investigationProject.Id,
                UserId = user.Id
            };

            await _context.InvestigationProjectTasks.AddAsync(investigationProjectTask);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditTaskAsync(InvestigationProjectTaskEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult("Revise el formulario");
            }

            var user = await _userManager.GetUserAsync(User);

            var investigationProjectTask = await _context.InvestigationProjectTasks
                .Where(x => x.Id == viewModel.Id && x.InvestigationProjectId == viewModel.InvestigationProjectId)
                .FirstOrDefaultAsync();

            //validacion de ser el usuario del proyecto o miembro del equipo?

            if (investigationProjectTask == null)
                return new BadRequestObjectResult("Sucedio un error");

            investigationProjectTask.Description = viewModel.Description;

            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnGetDetailTaskAsync(Guid id)
        {
            var investigationProjectTask = await _context.InvestigationProjectTasks
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (investigationProjectTask == null)
                return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                investigationProjectTask.Description,
                investigationProjectTask.Id,
                investigationProjectTask.InvestigationProjectId
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnPostDeleteTaskAsync(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);

            var investigationProjectTask = await _context.InvestigationProjectTasks
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (investigationProjectTask == null)
                return new BadRequestObjectResult("Sucedio un error");

            _context.InvestigationProjectTasks.Remove(investigationProjectTask);
            await _context.SaveChangesAsync();

            return new OkResult();
        }
        #endregion

        #region InvestigationProjectExpense-Gastos


        public async Task<IActionResult> OnPostCreateExpenseAsync(InvestigationProjectExpenseViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult("Revise el formulario");
            }

            var user = await _userManager.GetUserAsync(User);

            var investigationProject = await _context.InvestigationProjects
                .Where(x => x.Id == viewModel.InvestigationProjectId)
                .FirstOrDefaultAsync();

            //validacion de ser el usuario del proyecto o miembro del equipo?

            if (investigationProject == null)
                return new BadRequestObjectResult("Sucedio un error");

            var investigationProjectExpense = new InvestigationProjectExpense
            {
                Description = viewModel.Description,
                //InvestigationProjectId = investigationProject.Id, Ahora deberia ser tarea
                Amount = viewModel.Amount,
                InvestigationProjectTaskId  = viewModel.InvestigationProjectTaskId,
                ExpenseCode = viewModel.ExpenseCode,
                ProductType = viewModel.ProductType,

            };

            await _context.InvestigationProjectExpenses.AddAsync(investigationProjectExpense);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditExpenseAsync(InvestigationProjectExpenseEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult("Revise el formulario");
            }

            var user = await _userManager.GetUserAsync(User);

            var investigationProjectExpense = await _context.InvestigationProjectExpenses
                .Where(x => x.Id == viewModel.Id && x.InvestigationProjectTask.InvestigationProjectId == viewModel.InvestigationProjectId)
                .FirstOrDefaultAsync();

            //validacion de ser el usuario del proyecto o miembro del equipo?

            if (investigationProjectExpense == null)
                return new BadRequestObjectResult("Sucedio un error");

            investigationProjectExpense.Description = viewModel.Description;
            investigationProjectExpense.Amount = viewModel.Amount;
            investigationProjectExpense.InvestigationProjectTaskId = viewModel.InvestigationProjectTaskId;
            investigationProjectExpense.ExpenseCode = viewModel.ExpenseCode;
            investigationProjectExpense.ProductType = viewModel.ProductType;
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnGetDetailExpenseAsync(Guid id)
        {
            var investigationProjectExpense = await _context.InvestigationProjectExpenses
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (investigationProjectExpense == null)
                return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                investigationProjectExpense.Description,
                investigationProjectExpense.Id,
                investigationProjectExpense.Amount,
                investigationProjectExpense.ExpenseCode,
                investigationProjectExpense.ProductType,
                investigationProjectExpense.InvestigationProjectTaskId,
                //investigationProjectExpense.InvestigationProjectId
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnPostDeleteExpenseAsync(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);

            var investigationProjectExpense = await _context.InvestigationProjectExpenses
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (investigationProjectExpense == null)
                return new BadRequestObjectResult("Sucedio un error");

            _context.InvestigationProjectExpenses.Remove(investigationProjectExpense);
            await _context.SaveChangesAsync();

            return new OkResult();
        }
        #endregion


        private string GetConfigurationValue(Dictionary<string, string> list, string key)
        {
            return list.ContainsKey(key) ? list[key] :

                ConfigurationConstants.TEACHERINVESTIGATION.DEFAULT_VALUES.ContainsKey(key) ?
                ConfigurationConstants.TEACHERINVESTIGATION.DEFAULT_VALUES[key] : null;
        }


    }
}
