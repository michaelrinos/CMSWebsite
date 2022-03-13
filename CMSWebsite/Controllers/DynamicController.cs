using CMSWebsite.Models;
using CMSWebsite.Services;
using Microsoft.AspNetCore.Mvc;

namespace CMSWebsite.Controllers
{
    public class DynamicController : Controller
    {
        #region Constants

        private const string cshtml = ".cshtml";
        private const string editor = "Editor ";

        #endregion // Constants

        #region Fields

        private IViewRepository repository;
        private readonly IConfiguration _Config;
        private readonly ICMSService cmsService;

        #endregion // Fields

        #region Constructor

        public DynamicController(IViewRepository repo, IConfiguration config, ICMSService CMSService)
        {
            repository = repo ?? throw new ArgumentNullException(nameof(repo));
            _Config = config;
            this.cmsService = CMSService;
        }

        #endregion // Constructor

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DisplayView(string view)
        {
            return View(view, new RazerView());
        }

        #region CRUD
        public async Task<IActionResult> Read()
        {

            //var result = await this.cmsService.ViewsGetAll();


            return View();
        }


        #endregion // CRUD

        public async Task<IActionResult> Editor(string mode = "Editor", string location = "")
        {

            if (location == string.Empty && mode == string.Empty)
            { // We are creating a new model and we don't care about the view mode
                return View(new RazerView());
            }
            else
            {
                if (location == string.Empty)
                {   // We want a specific mode
                    return View(mode);
                }
                else
                {                          // Specific editor and existing model
                    try
                    {
                        var view = await (this.cmsService.GetRazerView(location) ?? cmsService.GetRazerViewLikeLocation(location));
                        return View(mode, view ?? new RazerView());
                    }
                    catch (Exception)
                    {
                        return View(mode, new RazerView());
                    }
                }
            }
        }

        public IActionResult Search(string searchItems)
        {
            return View();
        }

        [HttpPost]
        public async Task<ViewResult> SaveView(RazerView view)
        {
            if (ModelState.IsValid)
            {
                var fromDb = await (cmsService.GetRazerView(view.RazerViewId.ToString()) ?? cmsService.GetRazerView(view.Location) ?? cmsService.GetRazerViewLikeLocation(view.Location)) ;             
                if (fromDb != default(RazerView))
                {
                    fromDb.HTMLContent = view.HTMLContent;
                    fromDb.JSContent = view.JSContent;
                    fromDb.CSSContent = view.CSSContent;
                    fromDb.Model = view.Model;
                    cmsService.UpdateRazerView(fromDb);
                }
                else
                {
                    await cmsService.CreateRazerView(view);
                }
                return View("Thanks", view.Location);
            }
            return View();
        }

    }
}
