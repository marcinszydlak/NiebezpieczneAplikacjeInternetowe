
using Bai_APP.Entity;
using Bai_APP.Entity.ViewModels;
using Bai_APP.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bai_APP.Services;

namespace BAI_App.Services
{
    public static class MessageService
    {
        public static List<MessageViewModel> GetUserMessages(int userID)
        {
            List<AllowedMessage> allowedMessages;
            using (DataContext db = new DataContext())
            {
                allowedMessages = db.AllowedMessages.Where(x => x.UserID == userID && x.PermissionLevel >= 1).ToList();
            }
            List<MessageViewModel> messages = new List<MessageViewModel>();
            foreach (int MessageID in allowedMessages.Select(x => x.MessageID))
            {
                messages.Add(GetMessage(MessageID));
            }
            return messages;
        }

        public static bool CheckMessagePermission(int userID,int messageID)
        {
            MessageOwnerViewModel model = GetMessageOwner(messageID);
            return model.OwnerID == userID;
        }

        public static MessageOwnerViewModel GetMessageOwner(int messageID)
        {
            using (DataContext db = new DataContext())
            {
                return db.Messages.Where(x => x.MessageID == messageID).Select(x => new MessageOwnerViewModel()
                {
                    MessageID = x.MessageID,
                    OwnerID = x.OwnerID
                }).FirstOrDefault();
            }
        }

        public static MessageViewModel GetMessage(int? MessageID)
        {
            if (MessageID.HasValue)
            {
                using (DataContext db = new DataContext())
                {
                    return db.Messages.Where(x => x.MessageID == MessageID).Select(x => new MessageViewModel()
                    {
                        MessageID = x.MessageID,
                        MessageText = x.Text
                    }).FirstOrDefault();
                }
            }
            else
            {
                return null;
            }
        }

        public static void InsertMessage(int userID, string Content)
        {
            Message m;
            using (DataContext db = new DataContext())
            {
                m = db.Messages.Add(new Message()
                {
                    Text = Content,
                    OwnerID = userID
                });
                db.SaveChanges();
            }
            InsertAllowedMessage(userID, m.MessageID);
            List<int> usersID = UserService.GetUserIDs();
            foreach (int ID in usersID)
            {
                InsertAllowedMessage(ID, m.MessageID, Permission.Unavailable);
            }
        }

        public static void InsertAllowedMessage(int userID, int messageID, Permission permission = Permission.FullAccess)
        {
            using (DataContext db = new DataContext())
            {
                db.AllowedMessages.Add(new AllowedMessage()
                {
                    MessageID = messageID,
                    UserID = userID,
                    PermissionLevel = (int)permission
                });
                db.SaveChanges();
            }
        }

        public static void DeleteAllowedMessages(Message message)
        {
            using (DataContext db = new DataContext())
            {
                List<AllowedMessage> messages = db.AllowedMessages.Where(x => x.MessageID == message.MessageID).ToList();
                db.AllowedMessages.RemoveRange(messages.AsEnumerable());
                db.SaveChanges();
            }
        }

        public static void DeleteMessage(int MessageID)
        {
            using (DataContext db = new DataContext())
            {
                Message message = db.Messages.Where(x => x.MessageID == MessageID).FirstOrDefault();
                if (message != null)
                {
                    DeleteAllowedMessages(message);
                    db.Messages.Remove(message);
                }
            }
        }
        public static void UpdateMessage(MessageViewModel message)
        {
            using (DataContext db = new DataContext())
            {
                Message m = db.Messages.Where(x => x.MessageID == message.MessageID).FirstOrDefault();
                if (m != null)
                {
                    m.Text = message.MessageText;
                    db.SaveChanges();
                }
            }
        }
    }
}