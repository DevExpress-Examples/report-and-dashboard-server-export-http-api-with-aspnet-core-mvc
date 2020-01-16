using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ExportApiDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ExportApiDemo.Services {
    public interface ITokenService {
        Task<string> GetToken();
    }

    public class TokenService : ITokenService {
        const string TokenCookie = "ReportServerToken";

        readonly IHttpContextAccessor httpContextAccessor;
        readonly IHttpClientFactory httpClientFactory;
        readonly IConfiguration configuration;

        HttpRequest Request => httpContextAccessor.HttpContext.Request;
        HttpResponse Response => httpContextAccessor.HttpContext.Response;

        public TokenService(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory, IConfiguration configuration) {
            this.httpContextAccessor = httpContextAccessor;
            this.httpClientFactory = httpClientFactory;
            this.configuration = configuration;
        }

        public async Task<string> GetToken() {
            string username = configuration["ReportServer:UserName"];
            string password = configuration["ReportServer:UserPassword"];

            var requestContent = new Dictionary<string, string> {
                { "grant_type", "password"},
                { "username", username },
                { "password", password }
            };

            HttpClient client = httpClientFactory.CreateClient("reportServer");
            HttpResponseMessage response = await client.PostAsync("oauth/token", new FormUrlEncodedContent(requestContent));
            response.EnsureSuccessStatusCode();
            Token token = JsonConvert.DeserializeObject<Token>(await response.Content.ReadAsStringAsync());
            return token.AuthToken;
        }
    }
}