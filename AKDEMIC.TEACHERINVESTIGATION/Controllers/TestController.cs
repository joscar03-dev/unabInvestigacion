using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Helpers;
using AKDEMIC.TEACHERINVESTIGATION.ViewModels.Api.AuthViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using AKDEMIC.CORE.Constants;
using AKDEMIC.DOMAIN.Entities.General;

namespace AKDEMIC.TEACHERINVESTIGATION.Controllers
{
    //[Route("api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        protected readonly AkdemicContext _context;
        private readonly IHttpClientFactory _clientFactory;

        public TestController(
            AkdemicContext context,
            IHttpClientFactory clientFactory,
            SignInManager<ApplicationUser> signInManager
        )
        {
            _context = context;
            _clientFactory = clientFactory;
            _signInManager = signInManager;
        }

        //[HttpGet("cargarprueba")]
        //public async Task<IActionResult> Test()
        //{

        //    var convocation = await _context.InvestigationConvocations
        //        .Include(x => x.InvestigationConvocationRequirement)
        //        .FirstOrDefaultAsync();

        //    var investigationConvocationRequirement = new InvestigationConvocationRequirement();

        //    await _context.InvestigationConvocationRequirements.AddAsync(investigationConvocationRequirement);
        //    await _context.SaveChangesAsync();

        //    return Ok(investigationConvocationRequirement);
        //}

        [HttpGet("obtener-prueba")]
        public async Task<IActionResult> Test()
        {
            var client = _clientFactory.CreateClient("akdemic");

            var request = new HttpRequestMessage(HttpMethod.Get,
            "api/ApplicationUser/informacion-educativa");

            var authRequest = new HttpRequestMessage(HttpMethod.Post, "api/Auth/request-token");
            authRequest.Content = new StringContent(JsonSerializer.Serialize(new { clientId = "akdemic", clientSecret = "Educacion2020" }), System.Text.Encoding.UTF8, "application/json");
            var bearerResponse = await client.SendAsync(authRequest);

            if (!bearerResponse.IsSuccessStatusCode)
            {
                return BadRequest("Fallo el servicio");
            }

            using var tokenStream = await bearerResponse.Content.ReadAsStreamAsync();
            var tokenModel = await JsonSerializer.DeserializeAsync
                <TokenViewModel>(tokenStream);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenModel.token);
            var list = new List<string>
            {
                "docente", "31618099"
            };
            request.Content = new StringContent(JsonSerializer.Serialize(list), System.Text.Encoding.UTF8, "application/json");
            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Fallo el servicio");
            }

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync
                    <object>(responseStream);

            return Ok(result);
        }

        [HttpGet("carreras")]
        public async Task<IActionResult> Test2() 
        {
            var client = _clientFactory.CreateClient("akdemic");

            var request = new HttpRequestMessage(HttpMethod.Get,
            "api/Career/get");

            var authRequest = new HttpRequestMessage(HttpMethod.Post, "api/Auth/request-token");
            authRequest.Content = new StringContent(JsonSerializer.Serialize(new { clientId = "akdemic", clientSecret = "Educacion2020" }), System.Text.Encoding.UTF8, "application/json");
            var bearerResponse = await client.SendAsync(authRequest);

            if (!bearerResponse.IsSuccessStatusCode)
            {
                return BadRequest("Fallo el servicio");
            }

            using var tokenStream = await bearerResponse.Content.ReadAsStreamAsync();
            var tokenModel = await JsonSerializer.DeserializeAsync
                <TokenViewModel>(tokenStream);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenModel.token);

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Fallo el servicio");
            }

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync
                    <object>(responseStream);

            return Ok(result);
        }


        [HttpGet("/logout")]
        public async Task<IActionResult> Logout()
        {
            if (GeneralConstants.Authentication.SSO_ENABLED)
                return SignOut(CookieAuthenticationDefaults.AuthenticationScheme, /*OpenIdConnectDefaults.AuthenticationScheme*/"oidc");


            await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync();

            return LocalRedirect("/");
        }

        [AllowAnonymous]
        [HttpGet("/fc-logout")]
        public async Task<IActionResult> FrontChannelLogout()
        {
            if (_signInManager.IsSignedIn(User) || User?.Identity.IsAuthenticated == true)
            {
                await _signInManager.SignOutAsync();
                await HttpContext.SignOutAsync();
            }
            return Ok();
        }
    }
}
