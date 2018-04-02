using System.Web.Mvc;

namespace Bai_APP.Controllers
{
    public class SettingsController : Controller
    {
        // GET: Settings
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Change()
        {
            return View();
        }
    }
}