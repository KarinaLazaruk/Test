using System.Threading.Tasks;
using Web.Constants;
using Web.Helpers;
using Web.Models;

namespace Web.Managers
{
    public class PullRequestsManager
    {
        public enum Order
        {
            Oldest,
            Newest
        }

        public enum Direction
        {
            Incoming,
            Outgoing
        }

        private readonly HttpCommunicationWorker _httpWorker;

        public PullRequestsManager(HttpCommunicationWorker httpWorker)
        {
            _httpWorker = httpWorker;
        }

        public async Task<ResponseWrapper<PullRequest>> Get(string projectKey, string repositorySlug, Order order = Order.Newest, RequestOptions options = null, Direction direction = Direction.Incoming,
            PullRequestState state = PullRequestState.All, bool withAttributes = true, bool withProperties = true)
        {
            string requestUrl = UrlBuilder
                .ToRestApiUrl(string.Format(ApiConstans.PullRequest, projectKey, repositorySlug))
                .WithOptions(options)
                //.WithQueryParam("direction", direction.ToString())
                .WithQueryParam("state", state.ToString());
                //.WithQueryParam("withAttributes", withAttributes.ToString())
                //.WithQueryParam("withProperties", withProperties.ToString())
                //.WithQueryParam("order", order.ToString());

            var pullRequests = await _httpWorker.GetAsync<ResponseWrapper<PullRequest>>(requestUrl).ConfigureAwait(false);

            return pullRequests;

        }
    }
}