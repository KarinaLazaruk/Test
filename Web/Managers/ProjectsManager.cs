using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Constants;
using Web.Helpers;
using Web.Models;

namespace Web.Managers
{
    public class ResponseWrapper<T>
    {
        public int Size { get; set; }
        public int Limit { get; set; }
        public bool IsLastPage { get; set; }
        public IEnumerable<T> Values { get; set; }
        public int Start { get; set; }
        public int? NextPageStart { get; set; }
    }

    public class RequestOptions
    {
        public int? Limit { get; set; }
        public int? Start { get; set; }
        public string At { get; set; }
    }

    public class ProjectsManager
    {
        private readonly HttpCommunicationWorker _httpWorker;

        public ProjectsManager(HttpCommunicationWorker httpWorker)
        {
            _httpWorker = httpWorker;
        }

        public async Task<ResponseWrapper<Project>> Get(RequestOptions requestOptions = null)
        {
            var requestUrl = UrlBuilder.FormatRestApiUrl(ApiConstans.ManyProjects, requestOptions);

            var response = await _httpWorker.GetAsync<ResponseWrapper<Project>>(requestUrl).ConfigureAwait(false);

            return response;
        }
    }
}