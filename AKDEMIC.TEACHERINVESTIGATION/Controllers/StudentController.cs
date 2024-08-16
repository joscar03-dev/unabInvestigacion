using AKDEMIC.CORE.Services.General.Interfaces;
using AKDEMIC.CORE.Services;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using AKDEMIC.TEACHERINVESTIGATION.ViewModels.Api.AuthViewModels;
using AKDEMIC.TEACHERINVESTIGATION.ViewModels.Api.DepartmentViewModels;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using AKDEMIC.TEACHERINVESTIGATION.ViewModels.Api.StudentViewModels;

namespace AKDEMIC.TEACHERINVESTIGATION.Controllers
{
    [Route("api/estudiantes")]
    [ApiController]
    public class StudentController : Controller
    {
        protected readonly AkdemicContext _context;
        private readonly IHttpClientFactory _clientFactory;

        public StudentController(
            AkdemicContext context,
            IHttpClientFactory clientFactory
        )
        {
            _context = context;
            _clientFactory = clientFactory;
        }

        [HttpGet("select-search")]
        public async Task<IActionResult> SelectSearchStudents(string term = null)
        {
            var client = _clientFactory.CreateClient("akdemic");

            var request = new HttpRequestMessage(HttpMethod.Get,
                $"api/Student/select-search?search={term}");

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
            var students = await JsonSerializer.DeserializeAsync
                    <List<StudentViewModel>>(responseStream);

            var result = students
                .Select(x => new
                {
                    id = x.userId,
                    text = $"{x.userName} - {x.fullName}",
                    userId = x.userId,
                    paternalSurname = x.paternalSurname,
                    maternalSurname = x.maternalSurname,
                    name = x.name,
                    userName = x.userName,
                    fullName = x.fullName,
                    email = x.email,
                    sex = x.sex,
                    career = x.career,
                })
                .ToList();

            return Ok(result);
        }
    }
}
