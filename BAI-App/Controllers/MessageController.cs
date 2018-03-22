using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bai_APP.Entity;
using Bai_APP.Entity.ViewModels;
using Bai_APP.Helpers;
using BAI_App.Services;

namespace BAI_App.Controllers
{
    public class MessageController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Messages
        public ActionResult Index()
        {
            return View(db.Messages.ToList());
        }

        [HttpGet]
        public ActionResult Create(MessageViewModel message)
        {
            if (Session["login"] != null)
            {
                if (ModelState.IsValid)
                {
                    LoggedUserModel l = (LoggedUserModel)Session["login"];
                    MessageService.InsertMessage(l.UserID, message.MessageText);
                    return RedirectToAction("List", "Message");
                }
                return View(message);
            }
            else
            {
                return ErrorRedirect("Nie masz uprawnień do tworzenia wpisów do bazy danych");
            }            
        }

        [HttpGet]
        public ActionResult List()
        {
            LoggedUserModel l = (LoggedUserModel)Session["login"];
            if (l != null)
            {
                List<MessageViewModel> messages = l.Messages;
                return View(messages);
            }
            else
            {
                return RedirectToAction("Error", "Shared");
            }
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            MessageViewModel message = MessageService.GetMessage(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        [HttpGet]
        public ActionResult Edit(MessageViewModel message)
        {
            if (ModelState.IsValid)
            {
                MessageService.UpdateMessage(message);
                return RedirectToAction("List","Message");
            }
            return View(message.MessageID);
        }

        // GET: Messages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id.HasValue)
            {
                LoggedUserModel l = (LoggedUserModel)Session["login"];
                if(MessageService.CheckMessagePermission(l.UserID,id.Value))
                {
                    return View(id);
                }
                return ErrorRedirect(Error.InsufficientResourcePermission);
            }
            else
            {
                return ErrorRedirect(Error.InvalidMessageID);
            }
        }

        [HttpGet]
        public ActionResult Delete(MessageViewModel model)
        {
            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private ActionResult ErrorRedirect(string Message)
        {
            TempData["blad"] = Message;
            return RedirectToAction("Error", "Shared");
        }
    }
}
