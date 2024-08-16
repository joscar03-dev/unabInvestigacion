using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Constants.Systems;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Interfaces;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.ViewModels.InvestigationConvocationViewModels;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.Pages.InvestigationConvocationPage
{
    [Authorize(Roles = GeneralConstants.ROLES.RESEARCHERS)]
    public class InscriptionModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataTablesService _dataTablesService;
        private readonly IAsyncRepository<ProgressFileConvocationPostulant> _progressFileConvocationPostulantRepository;
        private readonly IAsyncRepository<PostulantTechnicalFile> _postulantTechnicalFileRepository;
        private readonly IAsyncRepository<PostulantFinancialFile> _postulantFinancialFileRepository;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public InscriptionModel(
            IAsyncRepository<ProgressFileConvocationPostulant> progressFileConvocationPostulantRepository,
            IAsyncRepository<PostulantTechnicalFile> postulantTechnicalFileRepository,
            IAsyncRepository<PostulantFinancialFile> postulantFinancialFileRepository,
            AkdemicContext context,
            UserManager<ApplicationUser> userManager,
            IDataTablesService dataTablesService,
            IOptions<CloudStorageCredentials> storageCredentials
        )
        {
            _postulantFinancialFileRepository = postulantFinancialFileRepository;
            _postulantTechnicalFileRepository = postulantTechnicalFileRepository;
            _progressFileConvocationPostulantRepository = progressFileConvocationPostulantRepository;
            _context = context;
            _userManager = userManager;
            _dataTablesService = dataTablesService;
            _storageCredentials = storageCredentials;
        }

        [BindProperty]
        public InscriptionViewModel Input { get; set; }

        #region Formularios

        public async Task<IActionResult> OnPostGeneralInformationSaveAsync(GeneralInformationViewModel viewModel) 
        {
            var postulant = await _context.InvestigationConvocationPostulants
                .Where(x => x.Id == viewModel.InvestigationConvocationPostulantId)
                .FirstOrDefaultAsync();

            if (postulant == null)
                return new BadRequestObjectResult("Sucedio un error");

            var requirement = await _context.InvestigationConvocationRequirements
                .Where(x => x.InvestigationConvocation.Id == postulant.InvestigationConvocationId)
                .FirstOrDefaultAsync();

            if (requirement == null)
                return new BadRequestObjectResult("Sucedio un error");


            //1.1
            if (!requirement.InvestigationTypeHidden)
            {
                postulant.InvestigationTypeId = viewModel.InvestigationTypeId;
            }

            if (!requirement.ExternalEntityHidden)
            {
                postulant.ExternalEntityId = viewModel.ExternalEntityId;


            }

            if (!requirement.BudgetHidden)
            {
                postulant.Budget = viewModel.Budget;
            }

            if (!requirement.InvestigationPatternHidden)
            {
                postulant.InvestigationPatternId = viewModel.InvestigationPatternId;
            }

            if (!requirement.AreaHidden)
            {
                postulant.InvestigationAreaId = viewModel.InvestigationAreaId;
            }

            if (!requirement.FacultyHidden)
            {
                postulant.FacultyId = viewModel.FacultyId;
                postulant.FacultyText = viewModel.FacultyText;
            }

            if (!requirement.CareerHidden)
            {
                postulant.CareerId = viewModel.CareerId;
                postulant.CareerText = viewModel.CareerText;
            }

            if (!requirement.ResearchCenterHidden )
            {
                postulant.ResearchCenterId = viewModel.ResearchCenterId;
            }

            //1.2
            if (!requirement.FinancingHidden)
            {
                postulant.FinancingInvestigationId = viewModel.FinancingInvestigationId;

            }

            //1.3 Lugar de ejecución
            //Esto se agrega aparte en otro metodo

            //MainLocation



            if (!requirement.MainLocationHidden)
            {
                postulant.MainLocation = viewModel.MainLocation;
            }


            if (viewModel.ResearchLineCategoryRequirments != null && viewModel.ResearchLineCategoryRequirments.Count > 0)
            {
                //Las opciones marcadas actualmente
                var currentPostulantResearchLines = await _context.PostulantResearchLines
                    .Where(x => x.InvestigationConvocationPostulantId == postulant.Id)
                    .ToListAsync();

                if (currentPostulantResearchLines.Count > 0)
                    _context.PostulantResearchLines.RemoveRange(currentPostulantResearchLines);

                for (int i = 0; i < viewModel.ResearchLineCategoryRequirments.Count; i++)
                {
                    if (viewModel.ResearchLineCategoryRequirments[i].ResearchLineSelectedId != null)
                    {
                        var newPostulantResearchLine = new PostulantResearchLine
                        {
                            InvestigationConvocationPostulantId = postulant.Id,
                            ResearchLineId = viewModel.ResearchLineCategoryRequirments[i].ResearchLineSelectedId.Value,
                        };

                        await _context.PostulantResearchLines.AddAsync(newPostulantResearchLine);
                    }

                }
            }

            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnGetGeneralInformationLoadAsync(Guid investigationConvocationPostulantId)
        {
            //General Information
            var model = await _context.InvestigationConvocationPostulants
                .Where(x => x.Id == investigationConvocationPostulantId)
                .Select(x => new GeneralInformationViewModel
                {
                    InvestigationConvocationPostulantId = x.Id,
                    InvestigationConvocationId = x.InvestigationConvocationId,
                    InvestigationTypeId = x.InvestigationTypeId,
                    InvestigationTypeHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.InvestigationTypeHidden,
                    InvestigationTypeWeight = x.InvestigationTypeId == null ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.InvestigationTypeWeight,
                    ExternalEntityId = x.ExternalEntityId,
                    ExternalEntityHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.ExternalEntityHidden,
                    ExternalEntityWeight = x.ExternalEntityId == null ? 0: x.InvestigationConvocation.InvestigationConvocationRequirement.ExternalEntityWeight,
                    Budget = x.Budget.HasValue ? x.Budget.Value : 0,
                    BudgetHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.BudgetHidden,
                    BudgetWeight = x.Budget == null ? 0: x.InvestigationConvocation.InvestigationConvocationRequirement.BudgetWeight,
                    InvestigationPatternId = x.InvestigationPatternId,
                    InvestigationPatternHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.InvestigationPatternHidden,
                    InvestigationPatternWeight = x.InvestigationPatternId == null ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.InvestigationPatternWeight,
                    InvestigationAreaId = x.InvestigationAreaId,
                    InvestigationAreaHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.AreaHidden,
                    InvestigationAreaWeight = x.InvestigationAreaId == null ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.AreaWeight,
                    FacultyId = x.FacultyId,
                    FacultyHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.FacultyHidden,
                    FacultyWeight = x.FacultyId == null ? 0:x.InvestigationConvocation.InvestigationConvocationRequirement.FacultyWeight,
                    CareerId = x.CareerId,
                    CareerHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.CareerHidden,
                    CareerWeight = x.CareerId == null ? 0:x.InvestigationConvocation.InvestigationConvocationRequirement.CareerWeight,
                    ResearchCenterId = x.ResearchCenterId,
                    ResearchCenterHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.ResearchCenterHidden,
                    ResearchCenterWeight = x.ResearchCenterId == null ? 0: x.InvestigationConvocation.InvestigationConvocationRequirement.ResearchCenterWeight,
                    FinancingInvestigationId = x.FinancingInvestigationId,
                    FinancingHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.FinancingHidden,
                    FinancingWeight = x.FinancingInvestigationId == null ? 0: x.InvestigationConvocation.InvestigationConvocationRequirement.FinancingWeight,
                    ExecutionPlaceHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.ExecutionPlaceHidden,

                    MainLocationHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.MainLocationHidden,
                    MainLocationWeight = x.InvestigationConvocation.InvestigationConvocationRequirement.MainLocationWeight,
                    MainLocation = x.MainLocation,
                })
                .FirstOrDefaultAsync();

            var researchLineCategoryRequirmentViewModel = new List<ResearchLineCategoryRequirmentViewModel>();

            var researchLineCategoriesRequirement = await _context.ResearchLineCategoryRequirements
                .Where(x => x.InvestigationConvocationRequirement.InvestigationConvocation.Id == model.InvestigationConvocationId && !x.Hidden)
                .Select(x => new
                {
                    InvestigationConvocationId = x.InvestigationConvocationRequirement.InvestigationConvocation.Id,
                    InvestigationConvocationRequirementId = x.InvestigationConvocationRequirementId,
                    x.Id,
                    x.Hidden,
                    x.Weight,
                    x.ResearchLineCategoryId,
                    ResearchLineCategoryName = x.ResearchLineCategory.Name,
                    ResearchLines = x.ResearchLineCategory.ResearchLines
                        .Select(y => new ResearchLineViewModel
                        { 
                            Id = y.Id,
                            Name = y.Name
                        })
                        .ToList()
                })
                .ToListAsync();

            var postulantResearchLine = await _context.PostulantResearchLines
                .Where(x => x.InvestigationConvocationPostulantId == model.InvestigationConvocationPostulantId)
                .Select(x => new
                {
                    x.InvestigationConvocationPostulant.InvestigationConvocationId,
                    x.ResearchLine.ResearchLineCategoryId,
                    x.ResearchLineId,
                    x.Id
                })
                .ToListAsync();

            //En base a los requerimientos de categorias agregados
            for (int i = 0; i < researchLineCategoriesRequirement.Count; i++)
            {
                var postulantDataSelected = postulantResearchLine
                    .Where(x => x.InvestigationConvocationId == researchLineCategoriesRequirement[i].InvestigationConvocationId &&
                                x.ResearchLineCategoryId == researchLineCategoriesRequirement[i].ResearchLineCategoryId)
                    .FirstOrDefault();


                var researchLinePostulant = new ResearchLineCategoryRequirmentViewModel
                {
                    Id = researchLineCategoriesRequirement[i].Id,
                    InvestigationConvocationRequirementId = researchLineCategoriesRequirement[i].InvestigationConvocationRequirementId,
                    ResearchLineCategoryId = researchLineCategoriesRequirement[i].ResearchLineCategoryId,
                    ResearchLineCategoryName = researchLineCategoriesRequirement[i].ResearchLineCategoryName,
                    Hidden = researchLineCategoriesRequirement[i].Hidden,
                    Weight = 0,
                    ResearchLines = researchLineCategoriesRequirement[i].ResearchLines,
                    ResearchLineSelectedId = null
                };


                if (postulantDataSelected != null)
                {
                    researchLinePostulant.Weight = researchLineCategoriesRequirement[i].Weight;
                    researchLinePostulant.ResearchLineSelectedId = postulantDataSelected.ResearchLineId;
                }

                researchLineCategoryRequirmentViewModel.Add(researchLinePostulant);

            }

            model.ResearchLineCategoryRequirments = researchLineCategoryRequirmentViewModel;


            return Partial("Partials/_GeneralInformation", model);
        }

        public async Task<IActionResult> OnGetExecutionPlaceDatatableAsync(Guid investigationConvocationPostulantId)
        {
            //Lugar de ejecución
            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<PostulantExecutionPlace, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.DepartmentText;
                    break;
                case "1":
                    orderByPredicate = (x) => x.ProvinceText;
                    break;
                case "2":
                    orderByPredicate = (x) => x.DistrictText;
                    break;
            }

            var query = _context.PostulantExecutionPlaces
                .Where(x => x.InvestigationConvocationPostulantId == investigationConvocationPostulantId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.DepartmentText,
                    x.ProvinceText,
                    x.DistrictText
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

        public async Task<IActionResult> OnGetExecutionPlaceDeleteAsync(Guid id)
        {
            var postulantExecutionPlace = await _context.PostulantExecutionPlaces
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (postulantExecutionPlace == null)
                return new BadRequestObjectResult("Sucedio un error");

            _context.PostulantExecutionPlaces.Remove(postulantExecutionPlace);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnGetExecutionPlaceWeightAsync(Guid investigationConvocationPostulantId)
        {
            var postulantExecutionPlaces = await _context.PostulantExecutionPlaces
                .Where(x => x.InvestigationConvocationPostulantId == investigationConvocationPostulantId)
                .ToListAsync();

            if (postulantExecutionPlaces == null)
                return new OkObjectResult(0);

            if (postulantExecutionPlaces.Count == 0)
                return new OkObjectResult(0);

            var requirementWeight = await _context.InvestigationConvocationPostulants
                .Where(x => x.Id == investigationConvocationPostulantId)
                .Select(x => x.InvestigationConvocation.InvestigationConvocationRequirement.ExecutionPlaceWeight)
                .FirstOrDefaultAsync();

            return new OkObjectResult(requirementWeight);
        }

        public async Task<IActionResult> OnPostCreateExecutionPlaceAsync(ExecutionPlaceViewModel viewModel)
        {
            var investigationConvocationPostulant = await _context.InvestigationConvocationPostulants
                .Where(x => x.Id == viewModel.InvestigationConvocationPostulantId)
                .FirstOrDefaultAsync();

            if (investigationConvocationPostulant == null)
                return new BadRequestObjectResult("Sucedio un error");

            var postulantExecutionPlace = new PostulantExecutionPlace
            {
                InvestigationConvocationPostulantId = investigationConvocationPostulant.Id,
                DepartmentId = viewModel.DepartmentId,
                ProvinceId = viewModel.ProvinceId,
                DistrictId = viewModel.DistrictId,
            };

            if (viewModel.DistrictId != null)
                postulantExecutionPlace.DistrictText = viewModel.DistrictText;

            if (viewModel.DepartmentId != null)
                postulantExecutionPlace.DepartmentText = viewModel.DepartmentText;

            if (viewModel.ProvinceId != null)
                postulantExecutionPlace.ProvinceText = viewModel.ProvinceText;

            await _context.PostulantExecutionPlaces.AddAsync(postulantExecutionPlace);

            await _context.SaveChangesAsync();

            return new OkResult();
        }


        public async Task<IActionResult> OnPostProblemDescriptionSaveAsync(ProblemDescriptionViewModel viewModel)
        {
            var postulant = await _context.InvestigationConvocationPostulants
                .Where(x => x.Id == viewModel.InvestigationConvocationPostulantId)
                .FirstOrDefaultAsync();

            if (postulant == null)
                return new BadRequestObjectResult("Sucedio un error");

            var requirement = await _context.InvestigationConvocationRequirements
                .Where(x => x.InvestigationConvocation.Id == postulant.InvestigationConvocationId)
                .FirstOrDefaultAsync();

            if (requirement == null)
                return new BadRequestObjectResult("Sucedio un error");

            //2.0 Descripcion del Problema

            if (!requirement.ProjectTitleHidden)
            {
                postulant.ProjectTitle = viewModel.ProjectTitle;
            }
            if (!requirement.ProblemDescriptionHidden)
            {
                postulant.ProblemDescription = viewModel.ProblemDescription;
            }
            if (!requirement.GeneralGoalHidden)
            {
                postulant.GeneralGoal = viewModel.GeneralGoal;
            }
            if (!requirement.ProblemFormulationHidden)
            {
                postulant.ProblemFormulation = viewModel.ProblemFormulation;
            }
            if (!requirement.SpecificGoalHidden)
            {
                postulant.SpecificGoal = viewModel.SpecificGoal;
            }
            if (!requirement.JustificationHidden)
            {
                postulant.Justification = viewModel.Justification;
            }

            await _context.SaveChangesAsync();
            return new OkResult();
        }

        public async Task<IActionResult> OnGetProblemDescriptionLoadAsync(Guid investigationConvocationPostulantId)
        {
            //Descripci?n del problema
            var model = await _context.InvestigationConvocationPostulants
                .Where(x => x.Id == investigationConvocationPostulantId)
                .Select(x => new ProblemDescriptionViewModel
                {
                    InvestigationConvocationPostulantId = x.Id,
                    ProjectTitle = x.ProjectTitle,
                    ProjectTitleHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.ProjectTitleHidden,
                    ProjectTitleWeight= x.ProjectTitle == null ?0:x.InvestigationConvocation.InvestigationConvocationRequirement.ProjectTitleWeight,
                    ProblemDescription = x.ProblemDescription,
                    ProblemDescriptionHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.ProblemDescriptionHidden,
                    ProblemDescriptionWeight = x.ProblemDescription == null ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.ProblemDescriptionWeight,
                    GeneralGoal = x.GeneralGoal,
                    GeneralGoalHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.GeneralGoalHidden,
                    GeneralGoalWeight = x.GeneralGoal == null ?0:x.InvestigationConvocation.InvestigationConvocationRequirement.GeneralGoalWeight,
                    ProblemFormulation = x.ProblemFormulation,
                    ProblemFormulationHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.ProblemFormulationHidden,
                    ProblemFormulationWeight = x.ProblemFormulation == null ? 0: x.InvestigationConvocation.InvestigationConvocationRequirement.ProblemFormulationWeight,
                    SpecificGoal = x.SpecificGoal,
                    SpecificGoalHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.SpecificGoalHidden,
                    SpecificGoalWeight = x.ProblemFormulation == null ? 0:x.InvestigationConvocation.InvestigationConvocationRequirement.SpecificGoalWeight,
                    Justification = x.Justification,
                    JustificationHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.JustificationHidden,
                    JustificationWeight = x.Justification == null ? 0:x.InvestigationConvocation.InvestigationConvocationRequirement.JustificationWeight
                })
                .FirstOrDefaultAsync();

            return Partial("Partials/_ProblemDescription", model);
        }
        public async Task<IActionResult> OnPostMarkReferenceSaveAsync(MarkReferenceViewModel viewModel)
        {
            var postulant = await _context.InvestigationConvocationPostulants
                .Where(x => x.Id == viewModel.InvestigationConvocationPostulantId)
                .FirstOrDefaultAsync();

            if (postulant == null)
                return new BadRequestObjectResult("Sucedio un error");

            var requirement = await _context.InvestigationConvocationRequirements
                .Where(x => x.InvestigationConvocation.Id == postulant.InvestigationConvocationId)
                .FirstOrDefaultAsync();

            if (requirement == null)
                return new BadRequestObjectResult("Sucedio un error");

            //3.0 Marco de Referencia

            if (!requirement.TheoreticalFundamentHidden)
            {
                postulant.TheoreticalFundament = viewModel.TheoreticalFundament;
            }
            if (!requirement.ProblemRecordHidden)
            {
                postulant.ProblemRecord = viewModel.ProblemRecord;
            }
            if (!requirement.HypothesisHidden)
            {
                postulant.Hypothesis = viewModel.Hypothesis;
            }
            if (!requirement.VariableHidden)
            {
                postulant.Variable = viewModel.Variable;
            }

            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnGetMarkReferenceLoadAsync(Guid investigationConvocationPostulantId)
        {
            //Marco de Referencia
            var model = await _context.InvestigationConvocationPostulants
                .Where(x => x.Id == investigationConvocationPostulantId)
                .Select(x => new MarkReferenceViewModel
                {
                    InvestigationConvocationPostulantId = x.Id,
                    TheoreticalFundament = x.TheoreticalFundament,
                    TheoreticalFundamentHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.TheoreticalFundamentHidden,
                    TheoreticalFundamentWeight = x.TheoreticalFundament == null ? 0: x.InvestigationConvocation.InvestigationConvocationRequirement.TheoreticalFundamentWeight,
                    ProblemRecord = x.ProblemRecord,
                    ProblemRecordHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.ProblemRecordHidden,
                    ProblemRecordWeight = x.ProblemRecord== null ?0:x.InvestigationConvocation.InvestigationConvocationRequirement.ProblemRecordWeight,
                    Hypothesis = x.Hypothesis,
                    HypothesisHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.HypothesisHidden,
                    HypothesisWeight = x.Hypothesis==null ?0 : x.InvestigationConvocation.InvestigationConvocationRequirement.HypothesisWeight,
                    Variable = x.Variable,
                    VariableHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.VariableHidden,
                    VariableWeight = x.Variable == null ?0: x.InvestigationConvocation.InvestigationConvocationRequirement.VariableWeight
                })
                .FirstOrDefaultAsync();

            return Partial("Partials/_MarkReference", model);
        }

        public async Task<IActionResult> OnPostMethodologySaveAsync(MethodologyViewModel viewModel)
        {
            var postulant = await _context.InvestigationConvocationPostulants
                .Where(x => x.Id == viewModel.InvestigationConvocationPostulantId)
                .FirstOrDefaultAsync();

            if (postulant == null)
                return new BadRequestObjectResult("Sucedio un error");

            var requirement = await _context.InvestigationConvocationRequirements
                .Where(x => x.InvestigationConvocation.Id == postulant.InvestigationConvocationId)
                .FirstOrDefaultAsync();

            

            if (requirement == null)
                return new BadRequestObjectResult("Sucedio un error");

            if (!requirement.MethodologyTypeHidden)
            {
                postulant.MethodologyTypeId = viewModel.MethodologyTypeId;
            }
            if (!requirement.MethodologyDescriptionHidden)
            {
                postulant.MethodologyDescription = viewModel.MethodologyDescription;
            }
            if (!requirement.PopulationHidden)
            {
                postulant.Population = viewModel.Population;
            }
            if (!requirement.InformationCollectionTechniqueHidden)
            {
                postulant.InformationCollectionTechnique = viewModel.InformationCollectionTechnique;
            }
            if (!requirement.AnalysisTechniqueHidden)
            {
                postulant.AnalysisTechnique = viewModel.AnalysisTechnique;
            }
            if (!requirement.EthicalConsiderationsHidden)
            {
                postulant.EthicalConsiderations = viewModel.EthicalConsiderations;
            }
            if (viewModel.FieldWork != null)
            {
                if (!requirement.FieldWorkHidden)
                {
                    var storage = new CloudStorageService(_storageCredentials);

                    string fileUrl = await storage.UploadFile(viewModel.FieldWork.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.INVESTIGATIONCONVOCATION_DOCUMENTS,
                            Path.GetExtension(viewModel.FieldWork.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);

                    postulant.FieldWork = fileUrl;
                }
            }

            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnGetMethodologyLoadAsync(Guid investigationConvocationPostulantId)
        {
            //Metodologia
            var model = await _context.InvestigationConvocationPostulants
                .Where(x => x.Id == investigationConvocationPostulantId)
                .Select(x => new MethodologyViewModel
                {
                    InvestigationConvocationPostulantId = x.Id,
                    MethodologyTypeId = x.MethodologyTypeId,
                    MethodologyTypeHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.MethodologyTypeHidden,
                    MethodologyTypeWeight = x.MethodologyTypeId == null ?0: x.InvestigationConvocation.InvestigationConvocationRequirement.MethodologyTypeWeight,
                    MethodologyDescription = x.MethodologyDescription,
                    MethodologyDescriptionHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.MethodologyDescriptionHidden,
                    MethodologyDescriptionWeight = x.MethodologyDescription == null ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.MethodologyDescriptionWeight,
                    Population = x.Population,
                    PopulationHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.PopulationHidden,
                    PopulationWeight = x.Population == null ? 0:x.InvestigationConvocation.InvestigationConvocationRequirement.PopulationWeight,
                    InformationCollectionTechnique = x.InformationCollectionTechnique,
                    InformationCollectionTechniqueHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.InformationCollectionTechniqueHidden,
                    InformationCollectionTechniqueWeight = x.InformationCollectionTechnique== null ?0: x.InvestigationConvocation.InvestigationConvocationRequirement.InformationCollectionTechniqueWeight,
                    AnalysisTechnique = x.AnalysisTechnique,
                    AnalysisTechniqueHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.AnalysisTechniqueHidden,
                    AnalysisTechniqueWeight = x.AnalysisTechnique == null ?0:x.InvestigationConvocation.InvestigationConvocationRequirement.AnalysisTechniqueWeight,
                    //
                    EthicalConsiderations = x.EthicalConsiderations,
                    EthicalConsiderationsHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.EthicalConsiderationsHidden,
                    EthicalConsiderationsWeight = x.EthicalConsiderations == null ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.EthicalConsiderationsWeight,
                    //
                    FieldWorkUrl = x.FieldWork,
                    FieldWorkHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.FieldWorkHidden,
                    FieldWorkWeight = x.FieldWork == null ? 0: x.InvestigationConvocation.InvestigationConvocationRequirement.FieldWorkWeight,
                })
                .FirstOrDefaultAsync();

            return Partial("Partials/_Methodology", model);
        }

        public async Task<IActionResult> OnPostExpectedResultSaveAsync(ExpectedResultViewModel viewModel)
        {
            var postulant = await _context.InvestigationConvocationPostulants
                .Where(x => x.Id == viewModel.InvestigationConvocationPostulantId)
                .FirstOrDefaultAsync();

            if (postulant == null)
                return new BadRequestObjectResult("Sucedio un error");

            var requirement = await _context.InvestigationConvocationRequirements
                .Where(x => x.InvestigationConvocation.Id == postulant.InvestigationConvocationId)
                .FirstOrDefaultAsync();

            if (requirement == null)
                return new BadRequestObjectResult("Sucedio un error");

            if (!requirement.ExpectedResultsHidden)
            {
                postulant.ExpectedResults = viewModel.ExpectedResults;
            }
            if (!requirement.ThesisDevelopmentHidden)
            {
                postulant.ThesisDevelopment = viewModel.ThesisDevelopment;
            }
            if (!requirement.PublishedArticleHidden)
            {
                postulant.PublishedArticle = viewModel.PublishedArticle;
            }
            if (!requirement.BroadcastArticleHidden)
            {
                postulant.BroadcastArticle = viewModel.BroadcastArticle;
            }
            if (!requirement.ProcessDevelopmentHidden)
            {
                postulant.ProcessDevelopment = viewModel.ProcessDevelopment;
            }
            if (!requirement.BibliographicReferencesHidden)
            {
                postulant.BibliographicReferences = viewModel.BibliographicReferences;
            }
            await _context.SaveChangesAsync();
            return new OkResult();
        }

        public async Task<IActionResult> OnGetExpectedResultLoadAsync(Guid investigationConvocationPostulantId)
        {
            //Resultados Esperados
            var model = await _context.InvestigationConvocationPostulants
                .Where(x => x.Id == investigationConvocationPostulantId)
                .Select(x => new ExpectedResultViewModel
                {
                    InvestigationConvocationPostulantId = x.Id,
                    //
                    ExpectedResults = x.ExpectedResults,
                    ExpectedResultsHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.ExpectedResultsHidden,
                    ExpectedResultsWeight = x.ExpectedResults == null ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.ExpectedResultsWeight,

                    BibliographicReferences = x.BibliographicReferences,
                    BibliographicReferencesHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.BibliographicReferencesHidden,
                    BibliographicReferencesWeight = x.BibliographicReferences == null ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.BibliographicReferencesWeight,
                    //
                    ThesisDevelopment = x.ThesisDevelopment,
                    ThesisDevelopmentHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.ThesisDevelopmentHidden,
                    ThesisDevelopmentWeight = x.ThesisDevelopment == null ?0:x.InvestigationConvocation.InvestigationConvocationRequirement.ThesisDevelopmentWeight,
                    PublishedArticle = x.PublishedArticle,
                    PublishedArticleHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.PublishedArticleHidden,
                    PublishedArticleWeight = x.PublishedArticle == null ?0:x.InvestigationConvocation.InvestigationConvocationRequirement.PublishedArticleWeight,
                    BroadcastArticle = x.BroadcastArticle,
                    BroadcastArticleHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.BroadcastArticleHidden,
                    BroadcastArticleWeight = x.BroadcastArticle == null ?0: x.InvestigationConvocation.InvestigationConvocationRequirement.BroadcastArticleWeight,
                    ProcessDevelopment = x.ProcessDevelopment,
                    ProcessDevelopmentHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.ProcessDevelopmentHidden,
                    ProcessDevelopmentWeight = x.ProcessDevelopment == null?0: x.InvestigationConvocation.InvestigationConvocationRequirement.ProcessDevelopmentWeight

                })
                .FirstOrDefaultAsync();

            return Partial("Partials/_ExpectedResult", model);
        }


        public async Task<IActionResult> OnPostTeamMemberSaveAsync(TeamMemberSaveViewModel viewModel)
        {
            var researcherUser = await _context.Users.Where(x => x.Id == viewModel.ResearcherUserId)
                .FirstOrDefaultAsync();

            var researcherUserRole = await _context.TeamMemberRoles.Where(x => x.Id == viewModel.ResearcherUserRoleId)
                .FirstOrDefaultAsync();

            var investigationConvocationPostulant = await _context.InvestigationConvocationPostulants
                .Where(x => x.Id == viewModel.InvestigationConvocationPostulantId)
                .FirstOrDefaultAsync();

            if (researcherUser == null || researcherUser == null || investigationConvocationPostulant == null)
                return new BadRequestObjectResult("Sucedio un error");

            var existsAsTeamMember = await _context.PostulantTeamMemberUsers
                .AnyAsync(x => x.UserId == researcherUser.Id && x.InvestigationConvocationPostulantId == investigationConvocationPostulant.Id);

            if (existsAsTeamMember)
                return new BadRequestObjectResult("El usuario ya existe como miembro del equipo");

            var storage = new CloudStorageService(_storageCredentials);

            if (viewModel.CvFile != null)
            {
                string fileUrl = await storage.UploadFile(viewModel.CvFile.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.INVESTIGATIONCONVOCATION_DOCUMENTS,
                        Path.GetExtension(viewModel.CvFile.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);
            }
           
            
            var teamMember = new PostulantTeamMemberUser
            {
                InvestigationConvocationPostulantId = viewModel.InvestigationConvocationPostulantId,
                TeamMemberRoleId = researcherUserRole.Id,
                UserId = researcherUser.Id,
                Objectives = viewModel.Objectives,
            };

            await _context.PostulantTeamMemberUsers.AddAsync(teamMember);

            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnGetTeamMemberDeleteAsync(Guid id) 
        {
            var teamMember = await _context.PostulantTeamMemberUsers
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (teamMember == null)
                return new BadRequestObjectResult("Sucedio un error");

            _context.PostulantTeamMemberUsers.Remove(teamMember);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnGetTeamMemberWeightAsync(Guid investigationConvocationPostulantId)
        {
            var teamMemberUsers = await _context.PostulantTeamMemberUsers
                .Where(x => x.InvestigationConvocationPostulantId == investigationConvocationPostulantId)
                .ToListAsync();

            if (teamMemberUsers == null)
                return new OkObjectResult(0);

            if (teamMemberUsers.Count == 0)
                return new OkObjectResult(0);

            var requirementWeight = await _context.InvestigationConvocationPostulants
                .Where(x => x.Id == investigationConvocationPostulantId)
                .Select(x => x.InvestigationConvocation.InvestigationConvocationRequirement.TeamMemberUserWeight)
                .FirstOrDefaultAsync();

            return new OkObjectResult(requirementWeight);
        }

        public async Task<IActionResult> OnGetTeamMemberDatatableAsync(Guid investigationConvocationPostulantId)
        {
            //Equipo de Trabajo
            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<PostulantTeamMemberUser, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.TeamMemberRole.Name;
                    break;
            }

            var query = _context.PostulantTeamMemberUsers
                .Where(x => x.InvestigationConvocationPostulantId == investigationConvocationPostulantId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    fullName = x.User.FullName,
                    roleName = x.TeamMemberRole.Name,
                    cvUrlCTE = x.User.CteVitaeConcytecUrl,
                    x.CvFilePath
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

        public async Task<IActionResult> OnGetExternalMemberDatatableAsync(Guid investigationConvocationPostulantId)
        {
            //Colaborador externo
            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<PostulantExternalMember, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.PaternalSurname;
                    break;
                case "2":
                    orderByPredicate = (x) => x.MaternalSurname;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Dni;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Profession;
                    break;
            }

            var query = _context.PostulantExternalMembers
                .Where(x => x.InvestigationConvocationPostulantId == investigationConvocationPostulantId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.PaternalSurname,
                    x.MaternalSurname,
                    x.Dni,
                    cvFilePath = x.CvFilePath,
                    x.Profession
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

        public async Task<IActionResult> OnGetExternalMemberWeightAsync(Guid investigationConvocationPostulantId)
        {
            var postulantExternalMembers = await _context.PostulantExternalMembers
                .Where(x => x.InvestigationConvocationPostulantId == investigationConvocationPostulantId)
                .ToListAsync();

            if (postulantExternalMembers == null)
                return new OkObjectResult(0);

            if (postulantExternalMembers.Count == 0)
                return new OkObjectResult(0);

            var requirementWeight = await _context.InvestigationConvocationPostulants
                .Where(x => x.Id == investigationConvocationPostulantId)
                .Select(x => x.InvestigationConvocation.InvestigationConvocationRequirement.ExternalMemberWeight)
                .FirstOrDefaultAsync();

            return new OkObjectResult(requirementWeight);
        }

        public async Task<IActionResult> OnGetExternalMemberDeleteAsync(Guid id)
        {
            var postulantExternalMember = await _context.PostulantExternalMembers
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (postulantExternalMember == null)
                return new BadRequestObjectResult("Sucedio un error");

            _context.PostulantExternalMembers.Remove(postulantExternalMember);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnGetExternalMemberAsync(Guid investigationConvocationPostulantId)
        {
            var postulantExternalMembers = await _context.PostulantExternalMembers
                .Where(x => x.InvestigationConvocationPostulantId == investigationConvocationPostulantId)
                .ToListAsync();

            if (postulantExternalMembers == null)
                return new OkObjectResult(0);

            if (postulantExternalMembers.Count == 0)
                return new OkObjectResult(0);

            var requirementWeight = await _context.InvestigationConvocationPostulants
                .Where(x => x.Id == investigationConvocationPostulantId)
                .Select(x => x.InvestigationConvocation.InvestigationConvocationRequirement.ExternalMemberWeight)
                .FirstOrDefaultAsync();

            return new OkObjectResult(requirementWeight);
        }

        public async Task<IActionResult> OnPostCreateExternalMemberAsync(ExternalMemberViewModel viewModel)
        {
            var investigationConvocationPostulant = await _context.InvestigationConvocationPostulants
                .Where(x => x.Id == viewModel.InvestigationConvocationPostulantId)
                .FirstOrDefaultAsync();

            if (investigationConvocationPostulant == null)
                return new BadRequestObjectResult("Sucedio un error");

            var existsAsExternalMember = await _context.PostulantExternalMembers
                .AnyAsync(x => x.Dni == viewModel.Dni && x.InvestigationConvocationPostulantId == investigationConvocationPostulant.Id);

            if (existsAsExternalMember)
                return new BadRequestObjectResult("Ya se ha registrado un colaborador externo con ese DNI");

            var storage = new CloudStorageService(_storageCredentials);

            var postulantExternalMember = new PostulantExternalMember
            {
                InvestigationConvocationPostulantId = viewModel.InvestigationConvocationPostulantId,
                Name = viewModel.Name,
                PaternalSurname = viewModel.PaternalSurname,
                MaternalSurname = viewModel.MaternalSurname,
                Dni = viewModel.Dni,
                Description = viewModel.Description,
                InstitutionOrigin = viewModel.InstitutionOrigin,
                Objectives = viewModel.Objectives,
                Profession = viewModel.Profession,
                Address = viewModel.Address,
                PhoneNumber = viewModel.PhoneNumber,
            };

            if (viewModel.CvFile != null)
            {
                postulantExternalMember.CvFilePath = await storage.UploadFile(viewModel.CvFile.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.INVESTIGATIONCONVOCATION_DOCUMENTS,
                        Path.GetExtension(viewModel.CvFile.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);
            }

            await _context.PostulantExternalMembers.AddAsync(postulantExternalMember);

            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnGetAnnexedFileDatatable(Guid investigationConvocationPostulantId)
        {
            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<PostulantAnnexFile, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Name;
                    break;
            }

            var query = _context.PostulantAnnexFiles
                .Where(x => x.InvestigationConvocationPostulantId == investigationConvocationPostulantId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.DocumentPath,
                    CreatedAt = x.CreatedAt.HasValue ? x.CreatedAt.ToLocalDateFormat() : "",
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

        public async Task<IActionResult> OnGetAnnexedFileLoadAsync(Guid investigationConvocationPostulantId) 
        {
            var postulant = await _context.InvestigationConvocationPostulants
                .Where(x => x.Id == investigationConvocationPostulantId)
                .Select(x => new 
                {
                    x.ProjectDuration,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ProjectDurationWeight,
                    PostulationAttachmentFilesWeight = x.PostulantAnnexFiles.Count() == 0 ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.PostulationAttachmentFilesWeight
                })
                .FirstOrDefaultAsync();

            var result = new
            {
                postulant.ProjectDuration,
                ProjectDurationWeight = postulant.ProjectDuration == null ? 0 : postulant.ProjectDurationWeight,
                PostulationAttachmentFilesWeight = postulant.PostulationAttachmentFilesWeight,
                TotalWeight = (postulant.ProjectDuration == null ? 0 : postulant.ProjectDurationWeight) + postulant.PostulationAttachmentFilesWeight
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnPostAnnexedFileSaveAsync(AnnexedFileViewModel viewModel) 
        {
            var user = await _userManager.GetUserAsync(User);

            var postulant = await _context.InvestigationConvocationPostulants                
                .Where(x => x.Id == viewModel.InvestigationConvocationPostulantId)
                .FirstOrDefaultAsync();

            if (postulant == null)
                return new BadRequestObjectResult("Sucedio un error");

            if (postulant.UserId != user.Id)
                return new BadRequestObjectResult("Sucedio un error");

            var requirement = await _context.InvestigationConvocationRequirements
                .Where(x => x.InvestigationConvocation.Id == postulant.InvestigationConvocationId)
                .FirstOrDefaultAsync();

            if (!requirement.ProjectDurationHidden)
            {
                postulant.ProjectDuration = viewModel.ProjectDuration;
                await _context.SaveChangesAsync();
            }

            return new OkResult();
        }

        public async Task<IActionResult> OnPostCreateAnnexedFileAsync(AnnexedFileModalViewModel viewModel) 
        {
            if (viewModel.AnnexedFileDocument == null)
                return new BadRequestObjectResult("Debe agregar un archivo");

            var investigationConvocationPostulant = await _context.InvestigationConvocationPostulants
                .Where(x => x.Id == viewModel.InvestigationConvocationPostulantId)
                .FirstOrDefaultAsync();

            if (investigationConvocationPostulant == null)
                return new BadRequestObjectResult("Sucedio un error");

            var storage = new CloudStorageService(_storageCredentials);

            string fileUrl = await storage.UploadFile(viewModel.AnnexedFileDocument.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.INVESTIGATIONCONVOCATION_DOCUMENTS,
                    Path.GetExtension(viewModel.AnnexedFileDocument.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);

            var postulantAnnexFile = new PostulantAnnexFile
            {
                InvestigationConvocationPostulantId = investigationConvocationPostulant.Id,
                Name = viewModel.AnnexedFileName,
                DocumentPath = fileUrl
            };
            await _context.PostulantAnnexFiles.AddAsync(postulantAnnexFile);

            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnGetAnnexedFileDeleteAsync(Guid id) 
        {
            var postulantAnnexFile = await _context.PostulantAnnexFiles
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (postulantAnnexFile == null)
                return new BadRequestObjectResult("Sucedio un error");

            _context.PostulantAnnexFiles.Remove(postulantAnnexFile);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostAdditionalQuestionSaveAsync(AdditionalQuestionViewModel viewModel)
        {
            if (viewModel.QuestionsHidden)
                return new OkResult();

            var user = await _userManager.GetUserAsync(User);

            if (viewModel.UserId != user.Id)
                return new BadRequestObjectResult("Sucedio un error");

            var requirement = await _context.InvestigationConvocationRequirements
                .Where(x => x.InvestigationConvocation.Id == viewModel.InvestigationConvocationId)
                .FirstOrDefaultAsync();

            //Borrar respuestas previas
            var answersByUser = await _context.InvestigationAnswerByUsers
                .Where(x => x.UserId == user.Id && x.InvestigationQuestion.InvestigationConvocationRequirementId == requirement.Id)
                .ToListAsync();

            _context.InvestigationAnswerByUsers.RemoveRange(answersByUser);

            if (viewModel.InvestigationQuestions != null)
            {
                for (int i = 0; i < viewModel.InvestigationQuestions.Count; i++)
                {
                    var currentQuestionId = viewModel.InvestigationQuestions[i].Id;

                    if (viewModel.InvestigationQuestions[i].Type == TeacherInvestigationConstants.InscriptionForm.QuestionType.TEXT_QUESTION)
                    {
                        var answerByUser = new InvestigationAnswerByUser
                        {
                            InvestigationQuestionId = currentQuestionId,
                            UserId = user.Id,
                            AnswerDescription = viewModel.InvestigationQuestions[i].AnswerDescription
                        };
                        await _context.InvestigationAnswerByUsers.AddAsync(answerByUser);
                    }
                    else if (viewModel.InvestigationQuestions[i].Type == TeacherInvestigationConstants.InscriptionForm.QuestionType.MULTIPLE_SELECTION_QUESTION)
                    {
                        for (int j = 0; j < viewModel.InvestigationQuestions[i].InvestigationAnswers.Count; j++)
                        {
                            if (viewModel.InvestigationQuestions[i].InvestigationAnswers[j].Selected)
                            {
                                var answerByUser = new InvestigationAnswerByUser
                                {
                                    InvestigationQuestionId = currentQuestionId,
                                    UserId = user.Id,
                                    InvestigationAnswerId = viewModel.InvestigationQuestions[i].InvestigationAnswers[j].Id
                                };
                                await _context.InvestigationAnswerByUsers.AddAsync(answerByUser);
                            }

                        }
                    }
                    else if (viewModel.InvestigationQuestions[i].Type == TeacherInvestigationConstants.InscriptionForm.QuestionType.UNIQUE_SELECTION_QUESTION)
                    {
                        if (viewModel.InvestigationQuestions[i].UniqueAnswer.HasValue)
                        {
                            var answerByUser = new InvestigationAnswerByUser
                            {
                                InvestigationQuestionId = currentQuestionId,
                                UserId = user.Id,
                                InvestigationAnswerId = viewModel.InvestigationQuestions[i].UniqueAnswer
                            };
                            await _context.InvestigationAnswerByUsers.AddAsync(answerByUser);
                        }

                    }
                }
            }

            await _context.SaveChangesAsync();

            return new OkResult();
        }





        #endregion

        public async Task<IActionResult> OnPostUploadDocumentFileAsync(PostulantDocumentsViewModel viewModel)
        {
            var investigationConvocationPostulant = await _context.InvestigationConvocationPostulants
                .Where(x => x.Id == viewModel.InvestigationConvocationPostulantId)
                .FirstOrDefaultAsync();

            if (investigationConvocationPostulant == null)
            {
                return new BadRequestObjectResult("Sucedio un error");
            }

            if (viewModel.FinancialReport == null || viewModel.TechnicalReport == null)
            {
                return new BadRequestObjectResult("Debe seleccionar los archivos");
            }

            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostSolveObservationAsync(Guid postulantObservationId)
        {
            var observation = await _context.PostulantObservations
                .Where(x => x.Id == postulantObservationId)
                .FirstOrDefaultAsync();

            if (observation == null)
                return new BadRequestObjectResult("Sucedio un error");

            observation.State = TeacherInvestigationConstants.PostulantObservation.State.PENDINGREVIEW;

            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnGetObservationsDatatableAsync(Guid investigationConvocationPostulantId) 
        {
            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<PostulantObservation, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Description;
                    break;
                case "3":
                    orderByPredicate = (x) => x.State;
                    break;
            }

            var query = _context.PostulantObservations
                .Where(x => x.InvestigationConvocationPostulantId == investigationConvocationPostulantId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    CreatedAt = x.CreatedAt.HasValue ? x.CreatedAt.ToLocalDateFormat() : "",
                    x.Description,
                    x.State,
                    StateText = TeacherInvestigationConstants.PostulantObservation.State.VALUES.ContainsKey(x.State) ?
                        TeacherInvestigationConstants.PostulantObservation.State.VALUES[x.State]  : ""
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


        public async Task<IActionResult> OnGetProgressPercentageAsync(Guid investigationConvocationPostulantId)
        {
            var user = await _userManager.GetUserAsync(User);

            var postulant = await _context.InvestigationConvocationPostulants
                .Where(x => x.Id == investigationConvocationPostulantId && x.UserId == user.Id)
                .Select(x => new 
                {
                    //1 Datos generales
                    x.InvestigationTypeId,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.InvestigationTypeHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.InvestigationTypeWeight,
                    x.ExternalEntityId,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ExternalEntityHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ExternalEntityWeight,
                    x.Budget,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.BudgetHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.BudgetWeight,
                    x.InvestigationPatternId,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.InvestigationPatternHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.InvestigationPatternWeight,
                    x.InvestigationAreaId,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.AreaHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.AreaWeight,
                    x.FacultyId,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.FacultyHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.FacultyWeight,
                    x.CareerId,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.CareerHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.CareerWeight,
                    x.ResearchCenterId,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ResearchCenterHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ResearchCenterWeight,
                    x.FinancingInvestigationId,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.FinancingHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.FinancingWeight,
                    //Lugar de ejecucion
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ExecutionPlaceHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ExecutionPlaceWeight,
                    ExecutionPlaces = x.PostulantExecutionPlaces
                        .Select(y => new
                        {
                            y.Id,
                            y.DepartmentId,
                            y.DepartmentText,
                            y.ProvinceId,
                            y.ProvinceText,
                            y.DistrictId,
                            y.DistrictText
                        })
                        .ToList(),
                    ResearchLineCategoryRequirements = x.InvestigationConvocation.InvestigationConvocationRequirement.ResearchLineCategoryRequirements
                            .Select(y => new 
                            {
                                y.Id,
                                y.ResearchLineCategoryId,
                                ResearchLineCategoryName = y.ResearchLineCategory.Name,
                                y.Hidden,
                                y.Weight,
                                Selected = x.PostulantResearchLines
                                    .Where(z => z.ResearchLine.ResearchLineCategoryId == y.ResearchLineCategoryId)
                                    .Any()
                            })
                            .ToList(),
                    //2 Descripci?n del Problema
                    x.ProjectTitle,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ProjectTitleHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ProjectTitleWeight,
                    x.ProblemDescription,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ProblemDescriptionHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ProblemDescriptionWeight,
                    x.GeneralGoal,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.GeneralGoalHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.GeneralGoalWeight,
                    x.ProblemFormulation,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ProblemFormulationHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ProblemFormulationWeight,
                    x.SpecificGoal,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.SpecificGoalHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.SpecificGoalWeight,
                    x.Justification,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.JustificationHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.JustificationWeight,
                    //3 Marco de Referencia
                    x.TheoreticalFundament,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.TheoreticalFundamentHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.TheoreticalFundamentWeight,
                    x.ProblemRecord,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ProblemRecordHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ProblemRecordWeight,
                    x.Hypothesis,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.HypothesisHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.HypothesisWeight,
                    x.Variable,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.VariableHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.VariableWeight,
                    //4 Metodologia
                    x.MethodologyTypeId,
                    MethodologyType = x.MethodologyTypeId == null ? "" : x.MethodologyType.Name,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.MethodologyTypeHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.MethodologyTypeWeight,
                    x.MethodologyDescription,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.MethodologyDescriptionHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.MethodologyDescriptionWeight,
                    x.Population,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.PopulationHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.PopulationWeight,
                    x.InformationCollectionTechnique,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.InformationCollectionTechniqueHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.InformationCollectionTechniqueWeight,
                    x.AnalysisTechnique,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.AnalysisTechniqueHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.AnalysisTechniqueWeight,
                    //
                    x.EthicalConsiderations,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.EthicalConsiderationsHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.EthicalConsiderationsWeight,
                    //
                    x.FieldWork,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.FieldWorkHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.FieldWorkWeight,
                    //5 Resultados esperados
                    //
                    x.ExpectedResults,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ExpectedResultsHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ExpectedResultsWeight,

                    x.BibliographicReferences,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.BibliographicReferencesHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.BibliographicReferencesWeight,
                    //
                    x.ThesisDevelopment,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ThesisDevelopmentHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ThesisDevelopmentWeight,
                    x.PublishedArticle,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.PublishedArticleHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.PublishedArticleWeight,
                    x.BroadcastArticle,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.BroadcastArticleHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.BroadcastArticleWeight,
                    x.ProcessDevelopment,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ProcessDevelopmentHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ProcessDevelopmentWeight,

                    //6.Equipo de trabajo,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.TeamMemberUserHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.TeamMemberUserWeight,
                    PostulantTeamMemberUsers = x.PostulantTeamMemberUsers
                        .Select(y => new
                        {
                            y.Id,
                            y.User.FullName,
                            RoleName = y.TeamMemberRole.Name
                        })
                        .ToList(),
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ExternalMemberHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ExternalMemberWeight,
                    ExternalMembers = x.PostulantExternalMembers
                        .Select(y => new
                        {
                            y.Id,
                            FullName = $"{y.PaternalSurname} {y.MaternalSurname} {y.Name}",
                            y.Dni,
                            y.Profession
                        })
                        .ToList(),
                    //7.Anexos Adjuntos
                    x.ProjectDuration,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ProjectDurationHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ProjectDurationWeight,
                    PostulationAttachmentFiles = x.PostulantAnnexFiles
                        .Select(y => new 
                        { 
                            y.Id,
                            y.Name
                        })
                        .ToList(),
                    x.InvestigationConvocation.InvestigationConvocationRequirement.PostulationAttachmentFilesHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.PostulationAttachmentFilesWeight,
                    //8.Preguntas Adicionales
                    AdditionalQuestions = _context.InvestigationAnswerByUsers
                        .Where(y => y.InvestigationQuestion.InvestigationConvocationRequirement.InvestigationConvocation.Id == x.InvestigationConvocationId && y.UserId == x.UserId)
                        .Select(y => new 
                        {
                            y.Id,
                            y.UserId,
                            y.InvestigationAnswerId,
                            y.AnswerDescription,
                            y.InvestigationQuestionId,
                            y.InvestigationQuestion.InvestigationConvocationRequirementId
                        })
                        .ToList(),
                    x.InvestigationConvocation.InvestigationConvocationRequirement.QuestionsHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.QuestionsWeight,
                })
                .FirstOrDefaultAsync();

            var result = new InscriptionProgressDataViewModel
            {
                //1
                GeneralInformationPercentage = (postulant.InvestigationTypeHidden ? 0 : (postulant.InvestigationTypeId == null ? 0 : postulant.InvestigationTypeWeight))
                    + (postulant.ExternalEntityHidden ? 0 : (postulant.ExternalEntityId == null ? 0 : postulant.ExternalEntityWeight))
                    + (postulant.BudgetHidden ? 0 : (postulant.Budget == null ? 0 : postulant.BudgetWeight))
                    + (postulant.InvestigationPatternHidden ? 0 : (postulant.InvestigationPatternId == null ? 0 : postulant.InvestigationPatternWeight))
                    + (postulant.AreaHidden ? 0 : (postulant.InvestigationAreaId == null ? 0 : postulant.AreaWeight))
                    + (postulant.FacultyHidden ? 0 : (postulant.FacultyId == null ? 0 : postulant.FacultyWeight))
                    + (postulant.CareerHidden ? 0 : (postulant.CareerId == null ? 0 : postulant.CareerWeight))
                    + (postulant.ResearchCenterHidden ? 0 : (postulant.ResearchCenterId == null ? 0 : postulant.ResearchCenterWeight))
                    + (postulant.FinancingHidden ? 0 : (postulant.FinancingInvestigationId == null ? 0 : postulant.FinancingWeight))
                    + (postulant.ExecutionPlaceHidden ? 0 : (postulant.ExecutionPlaces.Count == 0 ? 0 : postulant.ExecutionPlaceWeight))
                    + (postulant.ResearchLineCategoryRequirements.Where(x => !x.Hidden && x.Selected).Sum(x => x.Weight)),
                //2
                ProblemDescriptionPercentage = (postulant.ProjectTitleHidden ? 0 : (postulant.ProjectTitle == null ? 0 : postulant.ProjectTitleWeight))
                    + (postulant.ProblemDescriptionHidden ? 0 : (postulant.ProblemDescription == null ? 0 : postulant.ProblemDescriptionWeight))
                    + (postulant.GeneralGoalHidden ? 0 :(postulant.GeneralGoal == null ? 0: postulant.GeneralGoalWeight))
                    + (postulant.ProblemFormulationHidden ? 0 :(postulant.ProblemFormulation == null ? 0: postulant.ProblemFormulationWeight))
                    + (postulant.SpecificGoalHidden ? 0 :(postulant.SpecificGoal == null ? 0: postulant.SpecificGoalWeight))
                    + (postulant.JustificationHidden ? 0 :(postulant.Justification == null ? 0 : postulant.JustificationWeight)),
                //3
                MarkReferencePercentage = (postulant.TheoreticalFundamentHidden ? 0:(postulant.TheoreticalFundament == null ? 0 : postulant.TheoreticalFundamentWeight))
                    + (postulant.ProblemRecordHidden ? 0 :(postulant.ProblemRecord == null ? 0: postulant.ProblemRecordWeight ))
                    + (postulant.HypothesisHidden ?0:(postulant.Hypothesis == null ?0: postulant.HypothesisWeight))
                    + (postulant.VariableHidden ?0:(postulant.Variable == null ?0: postulant.VariableWeight)),
                //4
                MethodologyPercentage = (postulant.MethodologyTypeHidden ? 0: (postulant.MethodologyTypeId == null ?0: postulant.MethodologyTypeWeight))
                    + (postulant.MethodologyDescriptionHidden ? 0 : (postulant.MethodologyDescription == null ? 0 : postulant.MethodologyDescriptionWeight))
                    + (postulant.PopulationHidden ? 0: (postulant.Population == null ? 0: postulant.PopulationWeight))
                    + (postulant.InformationCollectionTechniqueHidden ? 0:(postulant.InformationCollectionTechnique == null ? 0 : postulant.InformationCollectionTechniqueWeight))
                    + (postulant.AnalysisTechniqueHidden ? 0 : (postulant.AnalysisTechnique == null ? 0: postulant.AnalysisTechniqueWeight))
                    + (postulant.EthicalConsiderationsHidden ? 0 : (postulant.EthicalConsiderations == null ? 0 : postulant.EthicalConsiderationsWeight))
                    + (postulant.FieldWorkHidden ? 0 : (postulant.FieldWork == null ? 0: postulant.FieldWorkWeight)),
                //5
                ExpectedResultPercentage = (postulant.ExpectedResultsHidden ? 0 : (postulant.ExpectedResults == null ? 0 : postulant.ExpectedResultsWeight))
                    + (postulant.ThesisDevelopmentHidden ? 0:(postulant.ThesisDevelopment == null ?0 : postulant.ThesisDevelopmentWeight))
                    + (postulant.PublishedArticleHidden ? 0:(postulant.PublishedArticle == null ? 0 : postulant.PublishedArticleWeight))
                    + (postulant.BroadcastArticleHidden ? 0:(postulant.BroadcastArticle == null ? 0 : postulant.BroadcastArticleWeight))
                    + (postulant.ProcessDevelopmentHidden ? 0:(postulant.ProcessDevelopment == null ?0 : postulant.ProcessDevelopmentWeight))
                    + (postulant.BibliographicReferencesHidden ? 0 : (postulant.BibliographicReferences == null ? 0 : postulant.BibliographicReferencesWeight)),
                //6
                TeamMemberPercentage = postulant.TeamMemberUserHidden ? 0 : (postulant.PostulantTeamMemberUsers.Count == 0 ? 0 : postulant.TeamMemberUserWeight)
                    + (postulant.ExternalMemberHidden ? 0 : (postulant.ExternalMembers.Count == 0 ? 0 : postulant.ExternalMemberWeight)),
                //7
                AnnexFilesPercentage = (postulant.ProjectDurationHidden ? 0 : (postulant.ProjectDuration == null ? 0 : postulant.ProjectDurationWeight)) 
                    + ( postulant.PostulationAttachmentFilesHidden ? 0 : (postulant.PostulationAttachmentFiles.Count == 0 ? 0 : postulant.PostulationAttachmentFilesWeight)),
                //8
                AdditionalQuestionsPercentage = postulant.QuestionsHidden ? 0 : ( postulant.AdditionalQuestions.Count == 0 ? 0: postulant.QuestionsWeight)
            };

            result.ProgressBarPercentage = result.GeneralInformationPercentage +
                result.ProblemDescriptionPercentage +
                result.MarkReferencePercentage +
                result.MethodologyPercentage +
                result.ExpectedResultPercentage +
                result.TeamMemberPercentage +
                result.AnnexFilesPercentage +
                result.AdditionalQuestionsPercentage;

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnGet(Guid investigationConvocationPostulantId)
        {
            var user = await _userManager.GetUserAsync(User);

            var postulant = await _context.InvestigationConvocationPostulants
                .Where(x => x.Id == investigationConvocationPostulantId && x.UserId == user.Id)
                .Select(x => new InscriptionViewModel
                {
                    InvestigationConvocationPostulantId = x.Id,
                    UserId = x.UserId,
                    InvestigationConvocationId = x.InvestigationConvocationId,
                    InvestigationConvocationRequirementId = x.InvestigationConvocation.InvestigationConvocationRequirement.Id,
                    PostulationAttachmentFilesHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.PostulationAttachmentFilesHidden,
                    PostulationAttachmentFilesWeight = x.PostulantAnnexFiles == null ?0:x.InvestigationConvocation.InvestigationConvocationRequirement.PostulationAttachmentFilesWeight, 
                    TeamMemberUserHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.TeamMemberUserHidden,
                    TeamMemberUserWeight = x.PostulantTeamMemberUsers == null ? 0:x.InvestigationConvocation.InvestigationConvocationRequirement.TeamMemberUserWeight,
                    ExternalMemberHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.ExternalMemberHidden,
                    ExternalMemberWeight = x.PostulantExternalMembers == null ? 0: x.InvestigationConvocation.InvestigationConvocationRequirement.ExternalMemberWeight,
                    ProjectDurationHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.ProjectDurationHidden,
                    ProjectDurationWeight = x.ProjectDuration == null ?0:x.InvestigationConvocation.InvestigationConvocationRequirement.ProjectDurationWeight,
                    QuestionsHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.QuestionsHidden,
                    QuestionsWeight = x.InvestigationConvocation.InvestigationConvocationRequirement.QuestionsWeight,

                    MainLocationHidden = x.InvestigationConvocation.InvestigationConvocationRequirement.MainLocationHidden,
                    MainLocationWeight = x.InvestigationConvocation.InvestigationConvocationRequirement.MainLocationWeight,

                    ProjectState = x.ProjectState,
                    ProjectStateText = TeacherInvestigationConstants.ConvocationPostulant.ProjectState.VALUES.ContainsKey(x.ProjectState) ?
                        TeacherInvestigationConstants.ConvocationPostulant.ProjectState.VALUES[x.ProjectState] : "",
                    ProgressState = x.ProgressState,
                    ProgressStateText = TeacherInvestigationConstants.ConvocationPostulant.ProgressState.VALUES.ContainsKey(x.ProgressState) ?
                        TeacherInvestigationConstants.ConvocationPostulant.ProgressState.VALUES[x.ProgressState] : "",
                    ReviewState = x.ReviewState,
                    ReviewStateText = TeacherInvestigationConstants.ConvocationPostulant.ReviewState.VALUES.ContainsKey(x.ReviewState) ?
                        TeacherInvestigationConstants.ConvocationPostulant.ReviewState.VALUES[x.ReviewState] : ""
                })
                .FirstOrDefaultAsync();

            if (postulant == null)
                return RedirectToPage("/Index");

            if (postulant.UserId != user.Id)
                return RedirectToPage("/Index");

            //Query para las respuestas de los usuarios
            var answerByUsers = _context.InvestigationAnswerByUsers
                .Where(x => x.UserId == user.Id && x.InvestigationQuestion.InvestigationConvocationRequirementId == postulant.InvestigationConvocationRequirementId);

            var questions = await _context.InvestigationQuestions
                .Where(x => x.InvestigationConvocationRequirementId == postulant.InvestigationConvocationRequirementId)
                .Select(x => new InvestigationQuestionViewModel
                {
                    Id = x.Id,
                    QuestionDescription = x.Description,
                    IsRequired = x.IsRequired,
                    Type = x.Type,
                    AnswerDescription = answerByUsers.Where(au => au.InvestigationQuestionId == x.Id).Select(au => au.AnswerDescription).FirstOrDefault(),
                    UniqueAnswer = answerByUsers.Where(au => au.InvestigationQuestionId == x.Id).Select(au => au.InvestigationAnswerId).FirstOrDefault(),
                    InvestigationAnswers = x.InvestigationAnswers
                        .Select(y => new InvestigationAnswerViewModel
                        {
                            Id = y.Id,
                            Description = y.Description,
                            Selected = answerByUsers
                                .Any(au => au.InvestigationQuestionId == y.InvestigationQuestionId && au.InvestigationAnswerId == y.Id)
                        })
                        .ToList()
                })
                .ToListAsync();

            Input = postulant;

            Input.InvestigationQuestions = questions;

            return Page();
        }

        public async Task<IActionResult> OnGetAdvanceDatatable(Guid investigationConvocationPostulantId)
        {
            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<ProgressFileConvocationPostulant, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Name;
                    break;
            }

            var query = _context.ProgressFileConvocationPostulants
                .Where(x => x.InvestigationConvocationPostulantId == investigationConvocationPostulantId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.FilePath,
                    CreatedAt = x.CreatedAt.HasValue ? x.CreatedAt.ToLocalDateFormat() : "",
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
        public async Task<IActionResult> OnPostCreateAdvanceAsync(ProgressFileConvocationCreateViewModel viewModel, Guid investigationConvocationPostulantId)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise los campos del formulario");

            if (viewModel.File == null)
                return new BadRequestObjectResult("Debe subir un archivo");

            var storage = new CloudStorageService(_storageCredentials);

            string fileUrl = await storage.UploadFile(viewModel.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.INVESTIGATIONCONVOCATION_DOCUMENTS,
                    Path.GetExtension(viewModel.File.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);

            var progressFileConvocationPostulant = new ProgressFileConvocationPostulant
            {
                InvestigationConvocationPostulantId = investigationConvocationPostulantId,
                FilePath = fileUrl,
                Name = viewModel.Name,
            };

            await _progressFileConvocationPostulantRepository.InsertAsync(progressFileConvocationPostulant);

            return new OkResult();
        }
        public async Task<IActionResult> OnPostEditAdvanceAsync(ProgressFileConvocationEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise los campos del formulario");

            var progressFileConvocationPostulant = await _progressFileConvocationPostulantRepository.GetByIdAsync(viewModel.Id);

            if (progressFileConvocationPostulant == null)
                return new BadRequestObjectResult("Sucedio un error");


            progressFileConvocationPostulant.Name = viewModel.Name;
            var storage = new CloudStorageService(_storageCredentials);

            if (viewModel.File != null)
            {
                string fileUrl = await storage.UploadFile(viewModel.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.INVESTIGATIONCONVOCATION_DOCUMENTS,
                                    Path.GetExtension(viewModel.File.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);

                progressFileConvocationPostulant.FilePath = fileUrl;
            }


            await _progressFileConvocationPostulantRepository.UpdateAsync(progressFileConvocationPostulant);

            return new OkResult();
        }
        public async Task<IActionResult> OnGetDetailAdvanceAsync(Guid id)
        {
            var progressFileConvocationPostulant = await _progressFileConvocationPostulantRepository.GetByIdAsync(id);

            if (progressFileConvocationPostulant == null)
                return new BadRequestObjectResult("Sucedio un Error");
            var result = new
            {
                progressFileConvocationPostulant.FilePath,
                progressFileConvocationPostulant.Name,
                progressFileConvocationPostulant.Id,
                progressFileConvocationPostulant.InvestigationConvocationPostulantId,
            };

            return new OkObjectResult(result);
        }
        public async Task<IActionResult> OnGetAdvanceDeleteAsync(Guid id)
        {
            var progressFileConvocationPostulant = await _progressFileConvocationPostulantRepository.GetByIdAsync(id);
            await _progressFileConvocationPostulantRepository.DeleteAsync(progressFileConvocationPostulant);

            return new OkResult();
        }
        public async Task<IActionResult> OnGetUploadTechDocumentFileDatatable(Guid investigationConvocationPostulantId)
        {
            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<PostulantTechnicalFile, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Name;
                    break;
            }

            var query = _context.PostulantTechnicalFiles
                .Where(x => x.InvestigationConvocationPostulantId == investigationConvocationPostulantId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.FilePath,
                    CreatedAt = x.CreatedAt.HasValue ? x.CreatedAt.ToLocalDateFormat() : "",
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
        public async Task<IActionResult> OnPostCreateUploadTechDocumentFileAsync(PostulantTechnicalFileCreateViewModel viewModel, Guid investigationConvocationPostulantId)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise los campos del formulario");

            if (viewModel.File == null)
                return new BadRequestObjectResult("Debe subir un archivo");

            var storage = new CloudStorageService(_storageCredentials);

            string fileUrl = await storage.UploadFile(viewModel.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.INVESTIGATIONCONVOCATION_DOCUMENTS,
                    Path.GetExtension(viewModel.File.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);

            var postulantTechnicalFile = new PostulantTechnicalFile
            {
                InvestigationConvocationPostulantId = investigationConvocationPostulantId,
                FilePath = fileUrl,
                Name = viewModel.Name,
            };

            await _postulantTechnicalFileRepository.InsertAsync(postulantTechnicalFile);

            return new OkResult();
        }
        public async Task<IActionResult> OnPostEditUploadTechDocumentFileAsync(PostulantTechnicalFileEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise los campos del formulario");

            var postulantTechnicalFile = await _postulantTechnicalFileRepository.GetByIdAsync(viewModel.Id);

            if (postulantTechnicalFile == null)
                return new BadRequestObjectResult("Sucedio un error");


            postulantTechnicalFile.Name = viewModel.Name;
            var storage = new CloudStorageService(_storageCredentials);

            if (viewModel.File != null)
            {
                string fileUrl = await storage.UploadFile(viewModel.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.INVESTIGATIONCONVOCATION_DOCUMENTS,
                                    Path.GetExtension(viewModel.File.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);

                postulantTechnicalFile.FilePath = fileUrl;
            }


            await _postulantTechnicalFileRepository.UpdateAsync(postulantTechnicalFile);

            return new OkResult();
        }
        public async Task<IActionResult> OnGetDetailUploadTechDocumentFileAsync(Guid id)
        {
            var postulantTechnicalFile = await _postulantTechnicalFileRepository.GetByIdAsync(id);

            if (postulantTechnicalFile == null)
                return new BadRequestObjectResult("Sucedio un Error");
            var result = new
            {
                postulantTechnicalFile.FilePath,
                postulantTechnicalFile.Name,
                postulantTechnicalFile.Id,
                postulantTechnicalFile.InvestigationConvocationPostulantId,
            };

            return new OkObjectResult(result);
        }
        public async Task<IActionResult> OnGetUploadTechDocumentFileDeleteAsync(Guid id)
        {
            var postulantTechnicalFile = await _postulantTechnicalFileRepository.GetByIdAsync(id);
            await _postulantTechnicalFileRepository.DeleteAsync(postulantTechnicalFile);

            return new OkResult();
        }

        public async Task<IActionResult> OnGetUploadFinancialDocumentFileDatatable(Guid investigationConvocationPostulantId)
        {
            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<PostulantFinancialFile, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Name;
                    break;
            }

            var query = _context.PostulantFinancialFiles
                .Where(x => x.InvestigationConvocationPostulantId == investigationConvocationPostulantId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.FilePath,
                    CreatedAt = x.CreatedAt.HasValue ? x.CreatedAt.ToLocalDateFormat() : "",
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
        public async Task<IActionResult> OnPostCreateUploadFinancialDocumentFileAsync(PostulantFinancialFileCreateViewModel viewModel, Guid investigationConvocationPostulantId)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise los campos del formulario");

            if (viewModel.File == null)
                return new BadRequestObjectResult("Debe subir un archivo");

            var storage = new CloudStorageService(_storageCredentials);

            string fileUrl = await storage.UploadFile(viewModel.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.INVESTIGATIONCONVOCATION_DOCUMENTS,
                    Path.GetExtension(viewModel.File.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);

            var postulantFinancialFile = new PostulantFinancialFile
            {
                InvestigationConvocationPostulantId = investigationConvocationPostulantId,
                FilePath = fileUrl,
                Name = viewModel.Name,
            };

            await _postulantFinancialFileRepository.InsertAsync(postulantFinancialFile);

            return new OkResult();
        }
        public async Task<IActionResult> OnPostEditUploadFinancialDocumentFileAsync(PostulantFinancialFileEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise los campos del formulario");

            var postulantFinancialFile = await _postulantFinancialFileRepository.GetByIdAsync(viewModel.Id);

            if (postulantFinancialFile == null)
                return new BadRequestObjectResult("Sucedio un error");


            postulantFinancialFile.Name = viewModel.Name;
            var storage = new CloudStorageService(_storageCredentials);

            if (viewModel.File != null)
            {
                string fileUrl = await storage.UploadFile(viewModel.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.INVESTIGATIONCONVOCATION_DOCUMENTS,
                                    Path.GetExtension(viewModel.File.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);

                postulantFinancialFile.FilePath = fileUrl;
            }


            await _postulantFinancialFileRepository.UpdateAsync(postulantFinancialFile);

            return new OkResult();
        }
        public async Task<IActionResult> OnGetDetailUploadFinancialDocumentFileAsync(Guid id)
        {
            var postulantFinancialFile = await _postulantFinancialFileRepository.GetByIdAsync(id);

            if (postulantFinancialFile == null)
                return new BadRequestObjectResult("Sucedio un Error");
            var result = new
            {
                postulantFinancialFile.FilePath,
                postulantFinancialFile.Name,
                postulantFinancialFile.Id,
                postulantFinancialFile.InvestigationConvocationPostulantId,
            };

            return new OkObjectResult(result);
        }
        public async Task<IActionResult> OnGetUploadFinancialDocumentFileDeleteAsync(Guid id)
        {
            var postulantFinancialFile = await _postulantFinancialFileRepository.GetByIdAsync(id);
            await _postulantFinancialFileRepository.DeleteAsync(postulantFinancialFile);

            return new OkResult();
        }

    }
}
