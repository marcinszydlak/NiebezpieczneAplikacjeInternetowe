using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
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

            if (!string.IsNullOrEmpty(model.Login) && !string.IsNullOrEmpty(model.Password))
            {
                if (ModelState.IsValid)
                {
                    LoggedUserViewModel login = UserService.Login(model);
                    Session["login"] = login;
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
        
        [HttpGet]
        public ActionResult Register(RegisterUserViewModel model)
        {
            if(ModelState.IsValid && !UserService.CheckUserLoginExists(model.UserLogin))
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