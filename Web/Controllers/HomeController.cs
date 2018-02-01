using System.Web.Mvc;
using Web.Helpers;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiClient _apiClient = new ApiClient();

        public ActionResult Index()
        {
            var response = _apiClient.GetProjects<ResponseWrapper<Project>>(); //get all projects 

            foreach (var value in response.Values)
                value.Avatar = value.GetProjectAvatar(); //photo's get project

            return View(response);
        }}
}