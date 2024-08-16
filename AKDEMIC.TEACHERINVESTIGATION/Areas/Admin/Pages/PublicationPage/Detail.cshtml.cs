using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Constants.Systems;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.PublicationViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.PublicationPage
{
    [Authorize(Roles = GeneralConstants.ROLES.SUPERADMIN + "," + GeneralConstants.ROLES.TEACHERINVESTIGATION_ADMIN + "," + GeneralConstants.ROLES.PUBLICATION_UNIT)]
    public class DetailModel : PageModel
    {

        protected readonly AkdemicContext _context;
        private readonly IDataTablesService _dataTablesService;
        private readonly UserManager<ApplicationUser> _userManager;

        public DetailModel(
            AkdemicContext context,
            IDataTablesService dataTablesService,
            UserManager<ApplicationUser> userManager
        )
        {
            _context = context;
            _userManager = userManager;
            _dataTablesService = dataTablesService;
        }

        [BindProperty]
        public PublicationDetailViewModel Input { get; set; }

        public async Task<IActionResult> OnGet(Guid publicationId)
        {
            var publication = await _context.Publications
                .Where(x => x.Id == publicationId)
                .Select(x => new
                {
                    Id = x.Id,
                    UserFullName = x.User.FullName,
                    UserName = x.User.UserName,
                    AuthorShipOrderName = x.AuthorshipOrder.Name,
                    OpusTypeName = x.OpusType.Name,
                    PublicationFunctionName = x.PublicationFunction.Name,
                    IndexPlaceName = x.IndexPlace.Name,
                    IdentificationTypeName = x.IdentificationType.Name,
                    WorkCategory = x.WorkCategory,
                    Title = x.Title,
                    SubTitle = x.SubTitle,
                    Journal = x.Journal,
                    Description = x.Description,
                    Volume = x.Volume,
                    Fascicle = x.Fascicle,
                    MainAuthor = x.MainAuthor,
                    PublishingHouse = x.PublishingHouse,
                    DOI = x.DOI,
                    PublishDate = x.PublishDate.ToLocalDateFormat()
                }).FirstOrDefaultAsync();

            if (publication == null)
                return RedirectToPage("/Index");

            Input = new PublicationDetailViewModel
            {
                Id = publication.Id,
                UserFullName = publication.UserFullName,
                UserName = publication.UserName,
                AuthorShipOrderName = publication.AuthorShipOrderName,
                OpusTypeName = publication.OpusTypeName,
                PublicationFunctionName = publication.PublicationFunctionName,
                IndexPlaceName = publication.IndexPlaceName,
                IdentificationTypeName = publication.IdentificationTypeName,
                WorkCategoryName = TeacherInvestigationConstants.Publication.WorkCategory.VALUES.ContainsKey(publication.WorkCategory) ? TeacherInvestigationConstants.Publication.WorkCategory.VALUES[publication.WorkCategory] : "",
                Title = publication.Title,
                SubTitle = publication.SubTitle,
                Journal = publication.Journal,
                Description = publication.Description,
                Volume = publication.Volume,
                Fascicle = publication.Fascicle,
                MainAuthor = publication.MainAuthor,
                PublishingHouse = publication.PublishingHouse,
                DOI = publication.DOI,
                PublishDate = publication.PublishDate
            };

            return Page();
        }
        //Datatable de Autor
        public async Task<IActionResult> OnGetAuthorDatatableAsync(Guid publicationId)
        {
            var user = await _userManager.GetUserAsync(User);

            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<PublicationAuthor, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.PaternalSurname;
                    break;
                case "1":
                    orderByPredicate = (x) => x.MaternalSurname;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Email;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Dni;
                    break;
            }

            var query = _context.PublicationAuthors
                .Where(x => x.PublicationId == publicationId && x.Publication.UserId == user.Id)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    CreatedAt = x.CreatedAt.HasValue ? x.CreatedAt.ToLocalDateFormat() : "",
                    x.PaternalSurname,
                    x.MaternalSurname,
                    x.Name,
                    x.Email,
                    x.Dni
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
        //Datatable de Archivo
        public async Task<IActionResult> OnGetFileDatatableAsync(Guid publicationId)
        {
            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<PublicationFile, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.FilePath;
                    break;

            }

            var query = _context.PublicationFiles
                .Where(x => x.PublicationId == publicationId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.FilePath,
                    x.Name,
                    x.PublicationId
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


    }
}
