using Newtonsoft.Json;
using System;
using System.Net;
using System.Text;
using Web.Data;
using Web.Models;

namespace Web.Helpers
{
    public class ApiClient
    {
        private readonly WebClient _client;

        public ApiClient()
        {
            _client = new WebClient {Encoding = Encoding.UTF8};
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(ApiConstans.Login + ":" + ApiConstans.Password));
            _client.Headers[HttpRequestHeader.Authorization] = $"Basic {credentials}";
        }

        public T GetProjects<T>()
        {
            var result = _client.DownloadString(ApiConstans.BaseUrl + ApiConstans.ManyProjects);
            return JsonConvert.DeserializeObject<T>(result);
        }

        public T GetPullRequests<T>(string projectKey, string repositorySlug)
        {
            string requestUrl = UrlBuilder
                .ToRestApiUrl(string.Format(ApiConstans.PullRequest, projectKey, repositorySlug))
                .WithQueryParam("state", PullRequestState.All.ToString());
            try
            {
                var result = _client.DownloadString($"{ApiConstans.BaseUrl}{requestUrl}");
                var response = JsonConvert.DeserializeObject<T>(result);
                return response;
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}