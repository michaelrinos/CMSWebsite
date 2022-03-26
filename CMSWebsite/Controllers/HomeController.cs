using Microsoft.AspNetCore.Mvc;

namespace CMSWebsite.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()//IStoreRepository repo)
        {
            //this.repository = repo;
        }
        public IActionResult Index() => View();
    }
}
