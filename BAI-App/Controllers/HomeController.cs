using System;
using System.Web.Mvc;
using Bai_APP.Entity.ViewModels;
using Bai_APP.Helpers;
using Bai_APP.Services;
using BAI_App.Services;

namespace Bai_APP.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login(UserLoginViewModel model)
        {
            if (string.IsNullOrEmpty(model.Login) || string.IsNullOrEmpty(model.Password) || !ModelState.IsValid)
            {
                return View();
            }

            if (!UserService.CheckUserLoginExists(model.Login))
            {
                AnonymousService.SaveAnonymousLoginAttempt(model.Login);
                DateTime anonAccountLockedToTime = AnonymousService.GetAccountLockedToTime(model.Login);

                if (anonAccountLockedToTime >= DateTime.UtcNow)
                {
                    return ErrorRedirect($"Konto zostało czasowo zablokowane do {anonAccountLockedToTime}! Spróbuj ponownie wkrótce!");
                }

                return ErrorRedirect($"Nieprawidłowy login lub hasło.");
            }

            LoggedUserViewModel login = UserService.Login(model);

            if (SettingsService.IsAccountLocked(model.Login))
            {
                return ErrorRedirect($"Konto zostało całkowicie zablokowane! Skontaktuj się z administratorem w celu odblokowania.");
            }
            else if (SettingsService.IsLoggingDelayedFor(model.Login))
            {
                return ErrorRedirect($"Konto zostało czasowo zablokowane do {SettingsService._userSettingsViewModel.AccountLockedTo}! Spróbuj ponownie wkrótce!");
            }
            else if (login == null)
            {
                return ErrorRedirect($"Nieprawidłowy login lub hasło.");
            }

            Session["login"] = login;

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Register(RegisterUserViewModel model)
        {
            if (ModelState.IsValid && !UserService.CheckUserLoginExists(model.UserLogin))
            {
                int userID = UserService.RegisterUser(model);
                MessageService.SetDefaultPermissionsForUser(userID);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("UserLogin", Error.UserNameExists);
                return View();
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session["login"] = null;

            return RedirectToAction("Index");
        }

        private ActionResult ErrorRedirect(string Message)
        {
            TempData["blad"] = Message;

            return RedirectToAction("Error", "Shared");
        }
    }
}