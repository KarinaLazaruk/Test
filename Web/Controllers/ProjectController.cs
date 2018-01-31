using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web.Helpers;
using Web.Managers;
using Web.Models;

namespace Web.Controllers
{
    public class ProjectInfo
    {
        public string Name { get; set; }
        public List<PullRequestInfo> PullRequests { get; set; }
    }

    public class PullRequestInfo
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public AuthorWrapper Author { get; set; }
        public string Link { get; set; }

        public PullRequestInfo(PullRequest pullRequest)
        {
            Id = pullRequest.Id;
            Link = pullRequest.Links.Self.ToString();
            Date = pullRequest.CreatedDate;
            Author = pullRequest.Author;
            Description = pullRequest.Description;
        }
    }

    public class ProjectController : Controller
    {
        private readonly ApiClient _apiClient = new ApiClient();
        
        public ActionResult Index(Project project)
        {
            var response = new ResponseWrapper<PullRequest>();
            
            Task.Run(async () => response = await _apiClient.PullRequests.Get(project.Key, project.Name)).Wait();

            var pullRequests = new List<PullRequestInfo>();
            
            if (response.Values != null)
                pullRequests.AddRange(response.Values.Select(pullRequest => new PullRequestInfo(pullRequest)));

            var model = new ProjectInfo
            {
                Name = project.Name,
                PullRequests = pullRequests
            };

            return View(model);
        }
    }
}