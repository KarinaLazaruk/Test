using System.Linq;
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
            var response = _apiClient.GetProjects<ResponseWrapper<Project>>();

            return View(response);
        }}
}