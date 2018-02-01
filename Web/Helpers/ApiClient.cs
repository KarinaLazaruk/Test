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
            _client = new WebClient {Encoding = Encoding.UTF8}; //init
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(ApiConstans.Login + ":" + ApiConstans.Password));
            _client.Headers[HttpRequestHeader.Authorization] = $"Basic {credentials}"; //authorization
        }

        public T GetProjects<T>() //get projects
        {
            var result = _client.DownloadString(ApiConstans.BaseUrl + ApiConstans.ManyProjects); //get json
            return JsonConvert.DeserializeObject<T>(result); // parsing json
        }

        public T GetPullRequests<T>(string projectKey, string repositorySlug) //get pullRequests
        {
            string requestUrl = UrlBuilder
                .ToRestApiUrl(string.Format(ApiConstans.PullRequest, projectKey, repositorySlug))
                .WithQueryParam("state", PullRequestState.All.ToString()); //get url with parameters
            try
            {
                var result = _client.DownloadString($"{ApiConstans.BaseUrl}{requestUrl}"); //get json
                var response = JsonConvert.DeserializeObject<T>(result); // parsing json
                return response;
            }
            catch (Exception)
            {
                return default(T); // error
            }
        }
    }
}