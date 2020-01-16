using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ExportApiDemo.Services {
    public interface IHttpRequestService {
        Task<HttpResponseMessage> GetAsync(string path);
        Task<HttpResponseMessage> PostAsync(string path, StringContent content);
    }

    public class HttpRequestService : IHttpRequestService {
        readonly ITokenService tokenService;
        readonly IHttpClientFactory httpClientFactory;

        public HttpRequestService(ITokenService tokenService, IHttpClientFactory httpClientFactory) {
            this.tokenService = tokenService;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<HttpResponseMessage> GetAsync(string path) {
            return await GetResponse(path, HttpMethod.Get, null);
        }

        public async Task<HttpResponseMessage> PostAsync(string path, StringContent content) {
            return await GetResponse(path, HttpMethod.Post, content);
        }

        async Task<HttpResponseMessage> GetResponse(string path, HttpMethod httpMethod, StringContent content) {
            string token = await tokenService.GetToken();

            using (var request = new HttpRequestMessage(httpMethod, path)) {
                if(content != null) {
                    request.Content = content;
                }
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpClient client = httpClientFactory.CreateClient("reportServer");
                HttpResponseMessage response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                return response;
            }
        }
    }
}