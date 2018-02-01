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

        public ActionResult Index(string key)
        {
            var project = _apiClient.GetProjectKey<Project>(key);
            var response = _apiClient.GetPullRequests<ResponseWrapper<PullRequest>>(key, project.Name); //get pullRequests 

            var pullRequests = new List<PullRequests>();

            if (response?.Values != null) pullRequests.AddRange(response.Values.Select(value => new PullRequests(value)));

            var model = new PullRequestViewModel("", pullRequests); // get model 

            return View(model);
        }
        //
    }
}