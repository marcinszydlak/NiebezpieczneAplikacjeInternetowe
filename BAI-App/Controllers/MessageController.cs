using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Bai_APP.Entity;
using Bai_APP.Entity.ViewModels;
using Bai_APP.Helpers;
using Bai_APP.Models.Enums;
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
                    LoggedUserViewModel l = (LoggedUserViewModel)Session["login"];
                    MessageService.InsertMessage(l.UserID, message.MessageText);
                    l.Messages.Add(message);
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
            LoggedUserViewModel l = (LoggedUserViewModel)Session["login"];
            if (l != null)
            {
                List<MessageViewModel> messages = MessageService.GetUserVisibleMessages(l.UserID);
                return View(messages);
            }
            else
            {
                return RedirectToAction("Error", "Shared");
            }
        }



        //public ActionResult Edit(int? id)
        //{
        //    MessageViewModel model = MessageService.GetMessage(id);
        //    return View(model);
        //}
        [HttpGet]
        public ActionResult Edit(MessageViewModel item)
        {
            LoggedUserViewModel l = (LoggedUserViewModel)Session["login"];
            if (!MessageService.CheckMessagePermission(l.UserID, item.MessageID))
            {
                return ErrorRedirect(Error.InsufficientOperationPermission);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    MessageService.UpdateMessage(item);
                    return RedirectToAction("List");
                }
                else
                {
                    if (item.MessageText == null)
                    {
                        item = MessageService.GetMessage(item.MessageID);
                    }
                }
            }
            return View(item);
        }


        [HttpGet]
        public ActionResult Delete(MessageViewModel model)
        { 
            LoggedUserViewModel l = (LoggedUserViewModel)Session["login"];
            if (MessageService.CheckMessagePermission(l.UserID, model.MessageID, Permission.Owner))
            {
                if (ModelState.IsValid && MessageService.CheckMessageExists(model))
                {
                    ViewBag.Message = $"wpis {model.MessageID} został usunięty";
                    l.Messages.RemoveAll(x => x.MessageID == model.MessageID);
                    MessageService.DeleteMessage(model.MessageID);
                    return RedirectToAction("List", "Message");
                }
                else
                {
                    return ErrorRedirect(Error.InvalidMessage);
                }
            }

            else
            {
                return ErrorRedirect(Error.InsufficientResourcePermission);
            }
        }

        [HttpGet]
        public ActionResult ManageMessagePermissions()
        {
            LoggedUserViewModel l = (LoggedUserViewModel)Session["login"];
            List<MessageViewModel> messages = MessageService.GetUserMessages(l.UserID);
            return View(messages);
        }

        [HttpGet]
        public ActionResult ManageUserPermissions(MessageViewModel model)
        {
            LoggedUserViewModel l = (LoggedUserViewModel)Session["login"];
            if (MessageService.CheckMessageExists(model))
            {
                if (MessageService.CheckMessagePermission(l.UserID, model.MessageID, Permission.Owner))
                {
                    MessageService.FillPermissions(model.MessageID);
                    List<MessagePermissionViewModel> permissionModel = MessageService.GetMessagePermissions(model.MessageID).Where(x => x.UserID != l.UserID).ToList();
                    return View(permissionModel);
                }
                else
                {
                    return ErrorRedirect(Error.InsufficientOperationPermission);
                }
            }
            else
            {
                return ErrorRedirect(Error.InvalidMessage);
            }
        }
        [HttpGet]
        public ActionResult UserMessagePermission(MessagePermissionViewModel model)
        {
            LoggedUserViewModel l = (LoggedUserViewModel)Session["login"];
            if (l != null)
            {
                MessagePermissionViewModel model2 = null;
                if (MessageService.CheckMessagePermission(l.UserID, model.MessageID, Permission.Owner))
                {
                    if (!ModelState.IsValid)
                    {
                        model2 = MessageService.GetUserPermission(model.UserID, model.MessageID);
                    }
                    else
                    {
                        model2 = model;
                        MessageService.SetPermissionForUser(model.UserID, model.MessageID, (Permission)model.PermissionLevel);
                        MessageViewModel message = MessageService.GetMessage(model.MessageID);
                        return RedirectToAction("ManageUserPermissions", "Message", message);
                    }
                }
                return View(model2);
            }
            else
            {
                return ErrorRedirect(Error.InsufficientResourcePermission);
            }
            
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
