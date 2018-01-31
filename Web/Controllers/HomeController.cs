using System.Threading.Tasks;
using System.Web.Mvc;
using Web.Helpers;
using Web.Managers;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiClient _apiClient = new ApiClient();

        public ActionResult Index()
        {
            var response = new ResponseWrapper<Project>();
            Task.Run(async () => response = await _apiClient.Projects.Get()).Wait();

            return View(response);
        }

        public ActionResult Action(Project project)
        {
            return View("ProjectView");
        }
    }
}