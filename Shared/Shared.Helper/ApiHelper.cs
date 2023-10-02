
using Shared.Model;
using System.Text.Json;
using System.Net.Http.Headers;

namespace Shared.Helper
{
    public class ApiHelper
    {
        public ApiToken AccessToken { get; set; }
        public string BaseUrl { get; set; }
        public Dictionary<string, string> RequestHeaders { get; set; }

        public async Task<T> GetObjectResponse<T>(string endpoint)
        {
            var response = await GetApiResponse(endpoint);

            if (response.ResponseBody != null)
            {
                return JsonSerializer.Deserialize<T>(response.ResponseBody);
            }

            return default(T);
        }

        public async Task<T> GetObjectResponse<T>(string endpoint, int limit)
        {
            var response = await GetApiResponse(endpoint, limit);

            if (response.ResponseBody != null)
            {
                return JsonSerializer.Deserialize<T>(response.ResponseBody);
            }

            return default(T);
        }

        public async Task<ApiResponse> GetApiResponse(string endpoint)
        {
            var response = default(ApiResponse);

            var ApiClient = new HttpClient();
            ApiClient.BaseAddress = new Uri(BaseUrl);

            if (!string.IsNullOrWhiteSpace(AccessToken.Token))
            {
                ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AccessToken.Type, AccessToken.Token);
            }

            if (RequestHeaders.Keys.Count > 0)
            {
                foreach (var Header in RequestHeaders)
                {
                    ApiClient.DefaultRequestHeaders.Add(Header.Key, Header.Value);
                }
            }

            try
            {
                var req = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}{endpoint}");

                var apiRes = await ApiClient.SendAsync(req);

                string body = await apiRes.Content.ReadAsStringAsync();

                response = new ApiResponse()
                {
                    ResponseBody = body,
                    Status = apiRes.StatusCode
                };

            }
            catch (Exception ex)
            {
                response = new ApiResponse()
                {
                    Status = System.Net.HttpStatusCode.InternalServerError
                };
            }

            return response;
        }

        public async Task<ApiResponse> GetApiResponse(string endpoint, int limit)
        {
            var response = default(ApiResponse);

            var ApiClient = new HttpClient();
            ApiClient.BaseAddress = new Uri(BaseUrl);

            if (!string.IsNullOrWhiteSpace(AccessToken.Token))
            {
                ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AccessToken.Type, AccessToken.Token);
            }

            if (RequestHeaders.Keys.Count > 0)
            {
                foreach (var Header in RequestHeaders)
                {
                    ApiClient.DefaultRequestHeaders.Add(Header.Key, Header.Value);
                }
            }

            try
            {
                var req = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/{endpoint}?limit={limit}");

                var apiRes = await ApiClient.SendAsync(req);

                apiRes.EnsureSuccessStatusCode();

                string body = await apiRes.Content.ReadAsStringAsync();

                response = new ApiResponse()
                {
                    ResponseBody = body 
                };

            }
            catch (Exception ex)
            {
            }

            return response;
        }
    }
}

