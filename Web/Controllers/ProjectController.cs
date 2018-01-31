using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web.Helpers;
using Web.Models;
using Web.ViewModels;

namespace Web.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ApiClient _apiClient = new ApiClient();

        public ActionResult Index(Project project)
        {
            var response = _apiClient.GetPullRequests<ResponseWrapper<PullRequest>>(project.Key, project.Name);
            
            var pullRequests = new List<PullRequests>();

            if (response?.Values != null) pullRequests.AddRange(response.Values.Select(value => new PullRequests(value)));

            var model = new PullRequestViewModel(project.Name, pullRequests);

            return View(model);
        }
    }
}