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
        private List<PullRequests> _pullRequests;
        private string _projectName;
        
        public ActionResult Index(string key, int? page)
        {
            _projectName = _apiClient.GetProjectKey<Project>(key).Name;
            var response = _apiClient.GetPullRequests<ResponseWrapper<PullRequest>>(key, _projectName); //get pullRequests 

            _pullRequests = new List<PullRequests>();

            if (response?.Values != null) _pullRequests.AddRange(response.Values.Select(value => new PullRequests(value)));
            
            if (Request.IsAjaxRequest()) return PartialView("Items", GetPullRequests(page));

            return View(GetPullRequests());
        }
        
        private PullRequestViewModel GetPullRequests(int? page = 0)
        {
            var itemsToSkip = page * 20;
            var items = _pullRequests.OrderByDescending(t => t.Id).Skip((int)itemsToSkip).Take(20).ToList();

            return items.Count > 0 ?new PullRequestViewModel(_projectName, items) : null; // get model 
        }
    }
}