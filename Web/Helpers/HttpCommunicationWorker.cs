using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Web.Helpers
{
    public class HttpCommunicationWorker
    {
        private readonly Uri _baseUrl;
        private AuthenticationHeaderValue _authenticationHeader;
        
        public HttpCommunicationWorker(string baseUrl, string username, string password)
        {
            _baseUrl = new Uri(baseUrl);
            SetBasicAuthentication(username, password);
        }

        public void SetBasicAuthentication(string username, string password)
        {
            var userPassBytes = Encoding.UTF8.GetBytes($"{username}:{password}");
            var userPassBase64 = Convert.ToBase64String(userPassBytes);

            _authenticationHeader = new AuthenticationHeaderValue("Basic", userPassBase64);
        }

        private HttpClient CreateHttpClient()
        {
            var httpClient = new HttpClient {BaseAddress = _baseUrl};

            if (_authenticationHeader != null)
                httpClient.DefaultRequestHeaders.Authorization = _authenticationHeader;

            return httpClient;
        }

        public async Task<T> GetAsync<T>(string requestUrl)
        {
            using (var httpClient = CreateHttpClient())
            using (var httpResponse = await httpClient.GetAsync(requestUrl).ConfigureAwait(false))
            {
                var json = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                var response = JsonConvert.DeserializeObject<T>(json);

                return response;
            }
        }

        public async Task<T> PostAsync<T>(string requestUrl, T data, bool ignoreNullFields = false)
        {
            var strData = JsonConvert.SerializeObject(data, new JsonSerializerSettings
            {
                NullValueHandling = ignoreNullFields ? NullValueHandling.Ignore : NullValueHandling.Include
            });
            HttpContent contentToPost = new StringContent(strData, Encoding.UTF8, "application/json");

            using (var httpClient = CreateHttpClient())
            using (var httpResponse = await httpClient.PostAsync(requestUrl, contentToPost))
            {
                if (httpResponse.StatusCode != HttpStatusCode.Created && httpResponse.StatusCode != HttpStatusCode.OK && httpResponse.StatusCode != HttpStatusCode.NoContent)
                {
                    throw new Exception($"POST operation unsuccessful. Got HTTP status code '{httpResponse.StatusCode}'");
                }

                if (httpResponse.StatusCode == HttpStatusCode.NoContent) return default(T);

                var json = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                var response = JsonConvert.DeserializeObject<T>(json);

                return response;
            }
        }
    }
}