using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Constants.Systems;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.ViewModels.InvestigationConvocationPostulantViewModels;
using AKDEMIC.TEACHERINVESTIGATION.Helpers;
using DinkToPdf.Contracts;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.WindowsAzure.Storage.Auth;
using static System.Net.WebRequestMethods;
using static AKDEMIC.CORE.Constants.FileStorageConstants;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using Microsoft.VisualBasic;
using Microsoft.CodeAnalysis.FlowAnalysis;
using System.Net.Http;
using System.Text.Json;
using AKDEMIC.TEACHERINVESTIGATION.ViewModels.Api.AuthViewModels;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.Pages.InvestigationConvocationPage
{
    [Authorize(Roles = GeneralConstants.ROLES.RESEARCHERS)]
    public class IndexModel : PageModel
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private IConverter _dinkConverter;
        private readonly IViewRenderService _viewRenderService;
        private readonly IDataTablesService _dataTablesService;
        private readonly UserManager<ApplicationUser> _userManager;
        protected readonly AkdemicContext _context;
        private readonly ITextSharpService _textSharpService;
        private readonly IHttpClientFactory _clientFactory;
        public IndexModel(
            IConverter dinkConverter,
            IDataTablesService dataTablesService,
            AkdemicContext context,
            IWebHostEnvironment environment,
            IViewRenderService viewRenderService,
            UserManager<ApplicationUser> userManager,
            ITextSharpService textSharpService,
            IHttpClientFactory clientFactory
        )
        {
            _dinkConverter = dinkConverter;
            _viewRenderService = viewRenderService;
            _dataTablesService = dataTablesService;
            _hostingEnvironment = environment;
            _userManager = userManager;
            _context = context;
            _textSharpService = textSharpService;
            _clientFactory = clientFactory;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue)
        {
            var user = await _userManager.GetUserAsync(User);
            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<InvestigationConvocationPostulant, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {

                case "1":
                    orderByPredicate = ((x) => x.InvestigationConvocation.Code);
                    break;

            }

            var query = _context.InvestigationConvocationPostulants
                .Where(x => x.UserId == user.Id)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.InvestigationConvocation.Code.ToUpper().Contains(searchValue.ToUpper())
                                      || x.InvestigationConvocation.Name.ToUpper().Contains(searchValue.ToUpper())
                                      || x.ProjectTitle.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.InvestigationConvocation.Code,
                    x.InvestigationConvocation.Name,
                    x.ProjectTitle,
                    x.FacultyText,
                    CreatedAt = x.CreatedAt.HasValue ? x.CreatedAt.ToLocalDateFormat() : "",
                    StartDate = x.InvestigationConvocation.StartDate.ToLocalDateFormat(),
                    EndDate = x.InvestigationConvocation.EndDate.ToLocalDateFormat(),
                    reviewState = TeacherInvestigationConstants.ConvocationPostulant.ReviewState.VALUES.ContainsKey(x.ReviewState) ?
                        TeacherInvestigationConstants.ConvocationPostulant.ReviewState.VALUES[x.ReviewState] : "",
                    progressState = TeacherInvestigationConstants.ConvocationPostulant.ProgressState.VALUES.ContainsKey(x.ProgressState) ?
                        TeacherInvestigationConstants.ConvocationPostulant.ProgressState.VALUES[x.ProgressState] : "",
                    ProgressPercentage = (x.InvestigationTypeId == null ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.InvestigationTypeWeight)
                             + (x.ExternalEntityId == null ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.ExternalEntityWeight)
                             + (x.Budget == null ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.BudgetWeight)
                             + (x.InvestigationPatternId == null ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.InvestigationPatternWeight)
                             + (x.InvestigationAreaId == null ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.AreaWeight)
                             + (x.FacultyId == null ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.FacultyWeight)
                             + (x.CareerId == null ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.CareerWeight)
                             + (x.ResearchCenterId == null ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.ResearchCenterWeight)
                             + (x.FinancingInvestigationId == null ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.FinancingWeight)
                             + (x.PostulantExecutionPlaces.Count() == 0 ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.ExecutionPlaceWeight)
                             + (x.InvestigationConvocation.InvestigationConvocationRequirement.ResearchLineCategoryRequirements
                                    .Where(y => x.PostulantResearchLines.Any(z => z.ResearchLine.ResearchLineCategoryId == y.ResearchLineCategoryId) && !y.Hidden)
                                    .Sum(y => y.Weight))
                             + (string.IsNullOrEmpty(x.ProjectTitle) ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.ProjectTitleWeight)
                             + (string.IsNullOrEmpty(x.ProblemDescription) ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.ProblemDescriptionWeight)
                             + (string.IsNullOrEmpty(x.GeneralGoal) ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.GeneralGoalWeight)
                             + (string.IsNullOrEmpty(x.ProblemFormulation) ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.ProblemFormulationWeight)
                             + (string.IsNullOrEmpty(x.SpecificGoal) ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.SpecificGoalWeight)
                             + (string.IsNullOrEmpty(x.Justification) ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.JustificationWeight)
                             + (string.IsNullOrEmpty(x.TheoreticalFundament) ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.TheoreticalFundamentWeight)
                             + (string.IsNullOrEmpty(x.ProblemRecord) ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.ProblemRecordWeight)
                             + (string.IsNullOrEmpty(x.Hypothesis) ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.HypothesisWeight)
                             + (string.IsNullOrEmpty(x.Variable) ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.VariableWeight)
                             + (x.MethodologyTypeId == null ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.MethodologyTypeWeight)
                             + (string.IsNullOrEmpty(x.MethodologyDescription) ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.MethodologyDescriptionWeight)
                             + (string.IsNullOrEmpty(x.Population) ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.PopulationWeight)
                             + (string.IsNullOrEmpty(x.InformationCollectionTechnique) ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.InformationCollectionTechniqueWeight)
                             + (string.IsNullOrEmpty(x.AnalysisTechnique) ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.AnalysisTechniqueWeight)
                             + (string.IsNullOrEmpty(x.FieldWork) ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.FieldWorkWeight)
                             + (string.IsNullOrEmpty(x.ThesisDevelopment) ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.ThesisDevelopmentWeight)
                             + (string.IsNullOrEmpty(x.PublishedArticle) ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.PublishedArticleWeight)
                             + (string.IsNullOrEmpty(x.BroadcastArticle) ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.BroadcastArticleWeight)
                             + (string.IsNullOrEmpty(x.ProcessDevelopment) ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.ProcessDevelopmentWeight)
                             + (x.ProjectDuration == null ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.ProjectDurationWeight)
                             + (x.PostulantAnnexFiles.Count() > 0 ? x.InvestigationConvocation.InvestigationConvocationRequirement.PostulationAttachmentFilesWeight : 0)
                             + (x.PostulantTeamMemberUsers.Count() > 0 ? x.InvestigationConvocation.InvestigationConvocationRequirement.TeamMemberUserWeight : 0)
                             + (x.PostulantExternalMembers.Count() == 0 ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.ExternalMemberWeight)
                             + (_context.InvestigationAnswerByUsers
                                .Count(y => y.UserId == x.UserId && y.InvestigationQuestion.InvestigationConvocationRequirement.InvestigationConvocation.Id == x.InvestigationConvocationId) == 0 ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.QuestionsWeight)
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

        public async Task<IActionResult> OnGetPdfAsync(Guid id)
        {
            var builder = new HtmlContentBuilder();

            var user = await _userManager.GetUserAsync(User);

            var query = await _context.InvestigationConvocationPostulants
                .Where(x => x.UserId == user.Id && x.Id == id)
                .Select(x => new
                {
                    x.ProjectTitle,
                    x.UserId,
                    UserFullName = x.User.FullName,
                    UserName = x.User.UserName,
                    Dni = x.User.Dni,
                    x.InvestigationTypeId,
                    InvestigationTypeName = x.InvestigationType.Name,
                    x.Budget,
                    x.MainLocation,
                    x.ProblemDescription,
                    x.ProblemRecord,
                    x.ProblemFormulation,
                    x.Justification,
                    x.Hypothesis,
                    x.Variable,
                    x.SpecificGoal,
                    x.MethodologyDescription,
                    x.EthicalConsiderations,
                    x.ExpectedResults,
                    x.BibliographicReferences,
                    StartDate = x.InvestigationConvocation.StartDate,
                    EndDate = x.InvestigationConvocation.EndDate,
                })
                .FirstOrDefaultAsync();

            var teamMembers = await _context.PostulantTeamMemberUsers
                .Where(x => x.InvestigationConvocationPostulantId == id)
                .Select(x => new 
                {
                    FullName = x.User.FullName,
                    UserName = x.User.UserName,
                    Dni = x.User.Dni,
                    Role = x.TeamMemberRole.Name
                })
                .ToListAsync();


            var queryResearchLine = await _context.PostulantResearchLines
                    .Where(x => x.InvestigationConvocationPostulantId == id)
                    .Select(x => new
                    {
                        ResearchLineNames = x.ResearchLine.Name,

                    }).ToListAsync();

            var model = new InvestigationConvocationPostulantPdfViewModel
            {
                ShieldPath = Path.Combine(_hostingEnvironment.WebRootPath, $@"images\themes\{GeneralConstants.GetTheme()}\escudo.png"),
                ImageLogoPath = Path.Combine(_hostingEnvironment.WebRootPath, $@"images\themes\{GeneralConstants.GetTheme()}\logo-report.png"),
                InstitutionName = GeneralConstants.GetInstitutionName().ToUpperInvariant(),
                InstitutionLocation = GeneralConstants.GetInstitutionLocation(),
                ProjectTitle = string.IsNullOrEmpty(query.ProjectTitle)? "No se encuentra información del título" : query.ProjectTitle, //1.1
                UserFullName = query.UserFullName, //1.2
                UserDni = query.Dni, //1.2
                UserName = query.UserName,
                InvestigationType = query.InvestigationTypeId != null ? query.InvestigationTypeName : "No se encuentra información del tipo de investigación", //1.3
                ResearchLines = new List<string>(), //1.4
                MainLocation = string.IsNullOrEmpty(query.MainLocation)? "No se encuentra información de la localización" : query.MainLocation,//1.6 
                Budget = query.Budget != null ? query.Budget.Value : 0, //1.7
                UserPdfViewModel = new List<UserPdfViewModel>(),
                ProblemDescription = string.IsNullOrEmpty(query.ProblemDescription)? "No se encuentra información sobre el resumen del proyecto" : query.ProblemDescription , //III
                ProblemRecord = string.IsNullOrEmpty(query.ProblemRecord)? "No se encuentra información sobre antecedentes" : query.ProblemRecord, //IV
                ProblemFormulation = string.IsNullOrEmpty(query.ProblemFormulation)? "No se encuentra información sobre planteamiento del problema" : query.ProblemFormulation,//V
                Justification = string.IsNullOrEmpty(query.Justification)? "No se encuentra información sobre justificación" : query.Justification, //VI
                Hypothesis = string.IsNullOrEmpty(query.Hypothesis)? "No se encuentra información sobre hipótesis" : query.Hypothesis, //VII
                Variable = string.IsNullOrEmpty(query.Variable)? "No se encuentra información sobre variable" : query.Variable,  //VII
                SpecificGoal = string.IsNullOrEmpty(query.SpecificGoal)? "No se encuentra información sobre objetivos" : query.SpecificGoal, //VIII
                MethodologyDescription = string.IsNullOrEmpty(query.MethodologyDescription)? "No se encuentra información sobre metodología" : query.MethodologyDescription, //IX
                EthicalConsiderations = string.IsNullOrEmpty(query.EthicalConsiderations)? "No se encuentra información sobre consideraciones éticas" : query.EthicalConsiderations, //X
                ExpectedResults = string.IsNullOrEmpty(query.ExpectedResults)? "No se encuentra información sobre resultados esperados" : query.ExpectedResults, //XI
                BibliographicReferences = string.IsNullOrEmpty(query.BibliographicReferences)? "No se encuentra información sobre referencias bibliográficas" : query.BibliographicReferences,//XII
                TotalDays = (query.EndDate-query.StartDate).TotalDays,
        };
            model.TotalYears = Math.Truncate(model.TotalDays / 365);
            model.TotalMonths = Math.Truncate((model.TotalDays % 365) / 30);
            model.RemainingDays = Math.Truncate((model.TotalDays % 365) % 30);


            model.UserPdfViewModel.Add(new UserPdfViewModel
            {
                UserFullName = query.UserFullName,
                Dni = query.Dni,
                UserName = query.UserName,
                MainFunction = "Responsable de Investigación"
            });

            List<string> userNamesLst = new List<string>();
            userNamesLst.Add(model.UserName);
            if (teamMembers.Count != 0)
            {
                foreach (var member in teamMembers)
                {
                    model.UserPdfViewModel.Add(new UserPdfViewModel
                    {
                        UserFullName = member.FullName,
                        Dni = member.Dni,
                        UserName = member.UserName,
                        MainFunction = "Miembro",
                        Speciality = "",
                        AcademicDegree = "",
                    });
                    userNamesLst.Add(member.UserName);
                }



                //Request a la otra base de datos, por webservice 
                var client = _clientFactory.CreateClient("akdemic");

                var request = new HttpRequestMessage(HttpMethod.Get,
                "api/ApplicationUser/informacion-educativa");
                //Obtener el token de la aplicacion
                var authRequest = new HttpRequestMessage(HttpMethod.Post, "api/Auth/request-token");
                authRequest.Content = new StringContent(JsonSerializer.Serialize(new { clientId = "akdemic", clientSecret = "Educacion2020" }), System.Text.Encoding.UTF8, "application/json");
                var bearerResponse = await client.SendAsync(authRequest);

                using var tokenStream = await bearerResponse.Content.ReadAsStreamAsync();
                var tokenModel = await JsonSerializer.DeserializeAsync
                    <TokenViewModel>(tokenStream);

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenModel.token);



                request.Content = new StringContent(JsonSerializer.Serialize(userNamesLst), System.Text.Encoding.UTF8, "application/json");
                var response = await client.SendAsync(request);

                using var responseStream = await response.Content.ReadAsStreamAsync();

                var result = await JsonSerializer.DeserializeAsync<List<UserApiViewModel>>(responseStream);

                foreach (var item in model.UserPdfViewModel)
                {
                    var resultQuery = result.Where(x => x.userName == item.UserName).First();
                    if (resultQuery != null && resultQuery.maxStudy != null)
                    {
                        item.Speciality = resultQuery.maxStudy.specialty;
                        item.AcademicDegree = resultQuery.maxStudy.academicDegree;
                    }
                    else
                    {
                        item.Speciality = "No se encuentra registro";
                        item.AcademicDegree = "No se encuentra registro";
                    }

                }

            }
            foreach (var lines in queryResearchLine)
            {
                model.ResearchLines.Add(lines.ResearchLineNames);
            }

            if(queryResearchLine.Count != 0)
            {
                model.ResearchLineConcat = string.Join(",", model.ResearchLines);
            }
            else
            {
                model.ResearchLineConcat = "No se encuentra información relacionada sobre líneas de investigación";
            }
            var headerGlobalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new DinkToPdf.MarginSettings { Top = 10, Bottom = 10, Left = 10, Right = 10 },
                DocumentTitle = $"RECIBOS DE INGRESOS",
                DPI = 290
            };

            var bodyGlobalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new DinkToPdf.MarginSettings { Top = 50, Bottom = 30, Left = 10, Right = 10 },
                DocumentTitle = $"RECIBOS DE INGRESOS",
                DPI = 290
            };

            using var resultPdf = new PdfDocument();

            var pages = new List<DinkToPdf.ObjectSettings>();
            var pdfDocuments = new List<PdfDocument>();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            var bodyToString = await _viewRenderService.RenderToStringAsync("/Areas/Teacher/Pages/InvestigationConvocationPage/Partials/_Pdf.cshtml", model);
            var bodyPdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = bodyGlobalSettings,
                Objects =
                {
                    new DinkToPdf.ObjectSettings
                    {
                        HtmlContent = bodyToString,
                        WebSettings =
                        {
                            DefaultEncoding = "utf-8"
                        }
                    }
                }
            };

            var bodyByte = _dinkConverter.Convert(bodyPdf);

            var headerToString = await _viewRenderService.RenderToStringAsync("/Areas/Teacher/Pages/InvestigationConvocationPage/Partials/_PdfHeader.cshtml", model);
            var headerPdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = headerGlobalSettings,
                Objects =
                {
                    new DinkToPdf.ObjectSettings
                    {
                        HtmlContent = headerToString,
                        WebSettings =
                        {
                            DefaultEncoding = "utf-8"
                        }
                    }
                }
            };

            var headerByte = _dinkConverter.Convert(headerPdf);

            var mixedPdf = _textSharpService.AddHeaderToAllPages(bodyByte, headerByte);
            using var document = PdfReader.Open(new MemoryStream(mixedPdf), PdfDocumentOpenMode.Import);

            foreach (var page in document.Pages)
                resultPdf.AddPage(page);



            var stream = new MemoryStream();
            resultPdf.Save(stream, false);
            var bytes = stream.ToArray();

            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";

            return File(bytes, "application/pdf", "reporte_unab.pdf");
        }
    }
}
