﻿using System.Web.Mvc;
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
            if (!string.IsNullOrEmpty(model.Login) && !string.IsNullOrEmpty(model.Password) && ModelState.IsValid)
            {
                if (UserService.CheckUserLoginExists(model.Login))
                {
                    LoggedUserViewModel login = UserService.Login(model);

                    if (SettingsService.IsLoggingDelayedFor(model.Login))
                    {
                        return ErrorRedirect($"Konto zostało czasowo zablokowane do {SettingsService._userSettingsViewModel.AccountLockedTo.AddHours(2)}! Spróbuj ponownie wkrótce!");
                    }

                    Session["login"] = login;

                    return RedirectToAction("Index");
                }
            }

            return View();
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