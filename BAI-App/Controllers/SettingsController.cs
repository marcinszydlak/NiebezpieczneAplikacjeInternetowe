using System.Web.Mvc;
using Bai_APP.Models.ViewModels;
using Bai_APP.Services;

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
            UserSettingsViewModel userSettingsViewModel = SettingsService.GetUserSettings(Session["login"].ToString());

            return View(userSettingsViewModel);
        }

        public ActionResult UpdateAttemptsToLockAccount(int AttemptsToLockAccount)
        {
            SettingsService.UpdateAttemptsToLockAccount(Session["login"].ToString(), AttemptsToLockAccount);

            return RedirectToAction("Change");
        }
    }
}