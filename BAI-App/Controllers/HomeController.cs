using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bai_APP.Entity.ViewModels;
using Bai_APP.Services;

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

        public ActionResult Register(RegisterUserViewModel model)
        {
            if(ModelState.IsValid)
            {
                UserService.RegisterUser(model);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        private ActionResult ErrorRedirect(string Message)
        {
            TempData["blad"] = Message;
            return RedirectToAction("Error", "Shared");
        }
    }
}