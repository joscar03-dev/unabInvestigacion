using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using AKDEMIC.CORE.Extensions;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.TEACHERINVESTIGATION.Helpers;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.UserRequestPage
{
    [Authorize(Roles = GeneralConstants.ROLES.SUPERADMIN + "," +
        GeneralConstants.ROLES.TEACHERINVESTIGATION_ADMIN + "," +
        GeneralConstants.ROLES.INNOVATION_TECHNOLOGY_TRANSFER_UNIT)]
    public class IndexModel : PageModel
    {
        private readonly IDataTablesService _dataTablesService;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IRandomPasswordService _randomPasswordService;
        private readonly UserManager<ApplicationUser> _userManager;
        protected readonly AkdemicContext _context;

        public IndexModel(
            IDataTablesService dataTablesService,
            IEmailSenderService emailSenderService,
            IRandomPasswordService randomPasswordService,
            UserManager<ApplicationUser> userManager,
            AkdemicContext context
            )
        {
            _dataTablesService = dataTablesService;
            _emailSenderService = emailSenderService;
            _randomPasswordService = randomPasswordService;
            _userManager = userManager;
            _context = context;
        }


        public void OnGet()
        {
        }

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<UserRequest, dynamic>> orderByPredicate = null;
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
                    orderByPredicate = (x) => x.Dni;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Email;
                    break;
                case "5":
                    orderByPredicate = (x) => x.State;
                    break;
                case "6":
                    orderByPredicate = (x) => x.Type;
                    break;
            }

            var query = _context.UserRequests
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.PaternalSurname.ToLower().Trim().Contains(searchValue.ToLower().Trim()) ||
                                    x.MaternalSurname.ToLower().Trim().Contains(searchValue.ToLower().Trim()) ||
                                    x.Name.ToLower().Trim().Contains(searchValue.ToLower().Trim()) ||
                                    x.Dni.ToLower().Trim().Contains(searchValue.ToLower().Trim()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.PaternalSurname,
                    x.MaternalSurname,
                    x.Email,
                    x.Dni,
                    x.Type,
                    HasLogged = x.User.FirstLoginDate != null,
                    TypeText = GeneralConstants.USERREQUEST_TYPE.VALUES.ContainsKey(x.Type) ?
                        GeneralConstants.USERREQUEST_TYPE.VALUES[x.Type] : "-",
                    x.State,
                    StateText = GeneralConstants.USERREQUEST_STATES.VALUES.ContainsKey(x.State) ?
                        GeneralConstants.USERREQUEST_STATES.VALUES[x.State] : "-",
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

        public async Task<IActionResult> OnPostUserRequestDeleteAsync(Guid id)
        {
            var userRequest = await _context.UserRequests.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (userRequest == null)
                return BadRequest("Sucedio un Error");

            if (userRequest.UserId != null)
                return BadRequest("Esta solicitud tiene información relacionada");

            if (!(userRequest.State == GeneralConstants.USERREQUEST_STATES.PENDING ||
                userRequest.State == GeneralConstants.USERREQUEST_STATES.REJECTED))
                return BadRequest("Solo pueden ser eliminadas las solicitudes pendientes o rechazadas");

            _context.UserRequests.Remove(userRequest);
            await _context.SaveChangesAsync();

            return new OkResult();
        }
        public async Task<IActionResult> OnPostUserRequestRejectAsync(Guid id)
        {
            var userRequest = await _context.UserRequests
                .Where(x => x.Id == id).FirstOrDefaultAsync();

            if (userRequest == null)
                return BadRequest("Sucedio un Error");

            if (userRequest.State != GeneralConstants.USERREQUEST_STATES.PENDING)
                return BadRequest("Solo se pueden rechazar solicitudes con estado pendiente");

            userRequest.State = GeneralConstants.USERREQUEST_STATES.REJECTED;

            await _context.SaveChangesAsync();

            return new OkResult();
        }
        public async Task<IActionResult> OnPostUserRequestAcceptAsync(Guid id)
        {
            var userRequest = await _context.UserRequests
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            string roleId = null;

            if (userRequest == null)
                return BadRequest("Sucedió un error");

            //Validar que el usuario que le vamos a asignar no exista
            string userName = "";

            if (!GeneralConstants.USERREQUEST_TYPE.VALUES.ContainsKey(userRequest.Type))
                return BadRequest("El tipo de solicitud no es valido");

            if (userRequest.Type == GeneralConstants.USERREQUEST_TYPE.CONTEST_EVALUATOR)
            {
                userName = $"EE{userRequest.Dni}";
                roleId = await _context.Roles.Where(x => x.Name == GeneralConstants.ROLES.EXTERNAL_EVALUATOR).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (userRequest.Type == GeneralConstants.USERREQUEST_TYPE.TECHNICAL_COLLABORATOR)
            {
                userName = $"CT{userRequest.Dni}";
                roleId = await _context.Roles.Where(x => x.Name == GeneralConstants.ROLES.TECHNICAL_COLLABORATOR).Select(x => x.Id).FirstOrDefaultAsync();
            }
            else
            {
                return BadRequest("El tipo de solicitud no se encuentrá registrado");
            }

            if (string.IsNullOrEmpty(roleId))
            {
                return BadRequest($"No existe el rol para el tipo - {GeneralConstants.USERREQUEST_TYPE.VALUES[userRequest.Type]}");
            }

            //Validacion dentro de nuestra BD
            var userExist = await _context.Users
                .IgnoreQueryFilters()
                .AnyAsync(x => x.UserName == userName);

            if (userExist)
                return BadRequest("El usuario que se desea asignar, ya existe");

            //TODO Validacion en AKDEMIC...

            var newPassword = _randomPasswordService.GeneratePassword();

            var user = new ApplicationUser
            {
                UserName = userName,
                NormalizedUserName = userName.ToUpper(),
                Email = userRequest.Email,
                NormalizedEmail = userRequest.Email.ToUpper(),
                Address = userRequest.Address,
                Dni = userRequest.Dni,
                PaternalSurname = userRequest.PaternalSurname,
                MaternalSurname = userRequest.MaternalSurname,
                Name = userRequest.Name,
                FullName = $"{userRequest.PaternalSurname} {userRequest.MaternalSurname} {userRequest.Name}",
                Type = GeneralConstants.USER_TYPES.NOT_ASIGNED,
                PhoneNumber = userRequest.PhoneNumber
            };

            var hashedPassword = _userManager.PasswordHasher.HashPassword(user, newPassword);
            user.PasswordHash = hashedPassword;


            var userRole = new ApplicationUserRole
            {
                UserId = user.Id,
                RoleId = roleId
            };

            await _context.Users.AddAsync(user);
            await _context.UserRoles.AddAsync(userRole);

            //Vincular al Request con el usuario y aceptarlo
            userRequest.UserId = user.Id;
            userRequest.State = GeneralConstants.USERREQUEST_STATES.ACCEPTED;

            //Enviar el correo
            await _context.SaveChangesAsync();

            try
            {
                //UNAB – Confirmación de usuario Nuevo
                var subject = $"{GeneralConstants.GetInstitutionAbbreviation()} - Bienvenido(a) al {ConstantHelpers.PROJECT.NAME}";
                var systemUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
                var message = EmailHelper.GetNewUserRegisterFormat(systemUrl, ConstantHelpers.PROJECT.NAME, user.UserName, newPassword);
                await _emailSenderService.SendEmailAsync(user.Email, subject, message, ConstantHelpers.PROJECT.NAME);
            }
            catch (Exception e)
            {
                return new OkObjectResult("No se ha podido enviar el correo de Bienvenida");
            }

            return new OkResult();
        }
        public async Task<IActionResult> OnPostSendInvitationEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("Debe especificar un correo electrónico");

            try
            {
                //UNAB – Invitación para Colaborador técnico
                var subject = $"{GeneralConstants.GetInstitutionAbbreviation()} - Invitación al registro dentro del {ConstantHelpers.PROJECT.NAME}";
                var systemUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
                var message = EmailHelper.GetUserRequestFormat(systemUrl);
                await _emailSenderService.SendEmailAsync(email, subject, message, ConstantHelpers.PROJECT.NAME);
            }
            catch (Exception e)
            {
                return BadRequest("No se ha podido enviar el correo de invitación");
            }

            return new OkResult();
        }

        public async Task<IActionResult> OnPostUserRequestReSendEmailAsync(Guid id)
        {
            var userRequest = await _context.UserRequests
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (userRequest == null)
                return BadRequest("Sucedió un error");


            var user = await _context.Users
                .IgnoreQueryFilters()
                .Where(x => x.Id == userRequest.UserId)
                .FirstOrDefaultAsync();

            if (userRequest.State != GeneralConstants.USERREQUEST_STATES.ACCEPTED)
                return BadRequest("Para poder enviarle el correo, el estado debe ser aceptado");

            if (user == null)
                return BadRequest("Sucedió un error");

            if (user.FirstLoginDate != null)
                return BadRequest("El usuario ya logeo por primera vez, no se podrá enviar un correo de Bienvenida");

            var newPassword = _randomPasswordService.GeneratePassword();

            var hashedPassword = _userManager.PasswordHasher.HashPassword(user, newPassword);
            user.PasswordHash = hashedPassword;

            await _context.SaveChangesAsync();
            //Enviar el correo
            try
            {
                //UNAB – Confirmación de usuario Nuevo
                var subject = $"{GeneralConstants.GetInstitutionAbbreviation()} - Bienvenido(a) al {ConstantHelpers.PROJECT.NAME}";
                var systemUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
                var message = EmailHelper.GetNewUserRegisterFormat(systemUrl, ConstantHelpers.PROJECT.NAME, user.UserName, newPassword);
                await _emailSenderService.SendEmailAsync(user.Email, subject, message, ConstantHelpers.PROJECT.NAME);
            }
            catch (Exception e)
            {
                return new OkObjectResult("No se ha podido reenviar el correo de Bienvenida");
            }

            return new OkResult();

        }
    }
}
