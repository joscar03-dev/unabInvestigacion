using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.ViewModels.Api.AuthViewModels;
using AKDEMIC.TEACHERINVESTIGATION.ViewModels.Api.StudentViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using AKDEMIC.TEACHERINVESTIGATION.ViewModels.Api.TeacherViewModels;
using System.Linq;

namespace AKDEMIC.TEACHERINVESTIGATION.Controllers
{
    [Route("api/docentes")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        protected readonly AkdemicContext _context;
        private readonly IHttpClientFactory _clientFactory;

        public TeacherController(
            AkdemicContext context,
            IHttpClientFactory clientFactory
        )
        {
            _context = context;
            _clientFactory = clientFactory;
        }

        [HttpGet("{userId}/get-info")]
        public async Task<IActionResult> GetTeacherInformation(string userId)
        {
            var client = _clientFactory.CreateClient("akdemic");

            var request = new HttpRequestMessage(HttpMethod.Get,
                $"api/Teacher/{userId}/get-info");

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
            var teacher = await JsonSerializer.DeserializeAsync
                    <TeacherViewModel>(responseStream);

            return Ok(teacher);
        }

        [HttpGet("select-search")]
        public async Task<IActionResult> SelectSearchTeachers(string term = null)
        {
            var client = _clientFactory.CreateClient("akdemic");

            var request = new HttpRequestMessage(HttpMethod.Get,
                $"api/Teacher/select-search?search={term}");

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
            var teachers = await JsonSerializer.DeserializeAsync
                    <List<TeacherViewModel>>(responseStream);

            var result = teachers
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
                    academicDepartment = x.academicDepartment,
                    teacherDedication = x.teacherDedication,
                    category = x.category,
                    dni = x.dni
                })
                .ToList();

            return Ok(result);
        }
    }
}
