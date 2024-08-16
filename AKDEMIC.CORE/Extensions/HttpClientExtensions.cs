using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Extensions
{
    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> PostAsync(this HttpClient httpClient, HttpContent httpContent, string apiRoute)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, apiRoute);
            request.Content = httpContent;

            return httpClient.SendAsync(request, CancellationToken.None);
        }

        public static Task<HttpResponseMessage> PostAsync(this HttpClient httpClient, HttpContent httpContent,string token, string apiRoute)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, apiRoute);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = httpContent;

            return httpClient.SendAsync(request, CancellationToken.None);
        }
    }
}
