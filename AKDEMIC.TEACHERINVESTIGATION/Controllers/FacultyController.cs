using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.ViewModels.Api.AuthViewModels;
using AKDEMIC.TEACHERINVESTIGATION.ViewModels.Api.FacultyViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Controllers
{
    [Route("api/facultades")]
    [ApiController]
    public class FacultyController : ControllerBase
    {
        protected readonly AkdemicContext _context;
        private readonly IHttpClientFactory _clientFactory;

        public FacultyController(
            AkdemicContext context,
            IHttpClientFactory clientFactory
        )
        {
            _context = context;
            _clientFactory = clientFactory;
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetFacultys()
        {
            var client = _clientFactory.CreateClient("akdemic");

            var request = new HttpRequestMessage(HttpMethod.Get,
                "api/Faculty/get");

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
            var faculties = await JsonSerializer.DeserializeAsync
                    <List<FacultyViewModel>>(responseStream);

            var result = faculties
                .Select(x => new
                {
                    id = x.id,
                    text = x.name
                })
                .ToList();

            return Ok(result);
        }
    }
}
