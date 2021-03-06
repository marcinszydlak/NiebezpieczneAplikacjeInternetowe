﻿
using Bai_APP.Entity;
using Bai_APP.Entity.ViewModels;
using Bai_APP.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bai_APP.Services;
using System.Transactions;

namespace BAI_App.Services
{
    public static class MessageService
    {
        public static List<MessageViewModel> GetUserMessages(int userID)
        {
            List<MessageViewModel> messages = new List<MessageViewModel>();
            using (DataContext db = new DataContext())
            {
                messages = db.AllowedMessages.Where(x => x.UserID == userID && x.PermissionLevel == (int)Permission.Owner).Select(x => new MessageViewModel()
                {
                    MessageID = x.MessageID,
                    MessageText = x.Message.Text
                }).ToList();
            }
            return messages;
        }

        public static List<MessageViewModel> GetUserVisibleMessages(int userID)
        {
            List<MessageViewModel> messages = new List<MessageViewModel>();
            using (DataContext db = new DataContext())
            {
                messages = db.AllowedMessages.Where(x => x.UserID == userID && x.PermissionLevel >= (int)Permission.ReadOnly).Select(x => new MessageViewModel()
                {
                    MessageID = x.MessageID,
                    MessageText = x.Message.Text
                }).ToList();
            }
            return messages;
        }

        public static void SetPermissionForUser(int userID,int messageID,Permission permission)
        {
            using (DataContext db = new DataContext())
            {
                AllowedMessage message = db.AllowedMessages.FirstOrDefault(x => x.UserID == userID && x.MessageID == messageID);
                message.PermissionLevel = (int)permission;
                db.SaveChanges();
            }
            
        }

        public static void SetDefaultPermissionsForUser(int userID,Permission permission = Permission.Unavailable)
        {
            using (DataContext db = new DataContext())
            {
                List<int> messageIDs = db.Messages.Select(x => x.MessageID).ToList();
                foreach(int messageID in messageIDs)
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
        }

        public static bool CheckMessageExists(MessageViewModel model)
        {
            using (DataContext db = new DataContext())
            {
                return db.Messages.FirstOrDefault(x => x.MessageID == model.MessageID && x.Text == model.MessageText) != null;
            }
        }

        public static void FillPermissions(int messageID)
        {
            using (DataContext db = new DataContext())
            {
                List<int> users = db.Users.Select(x => x.UserID).ToList();
                foreach(int user in users)
                {
                    var exists = db.AllowedMessages.FirstOrDefault(x => x.UserID == user && x.MessageID == messageID);
                    if(exists == null)
                    {
                        InsertAllowedMessage(user, messageID, Permission.Unavailable);
                    }
                }
            }
        }
        public static List<MessagePermissionViewModel> GetMessagePermissions(int messageID)
        {
            using (DataContext db = new DataContext())
            {
                return db.AllowedMessages.Where(x => x.MessageID == messageID).Select(x => new MessagePermissionViewModel()
                {
                    MessageID = messageID,
                    PermissionLevel = x.PermissionLevel,
                    UserID = x.UserID,
                    UserLogin = x.User.UserLogin
                }).ToList();
            }
        }

        public static bool CheckMessagePermission(int userID,int messageID,Permission permission = Permission.FullAccess)
        {
            MessagePermissionViewModel model = GetUsersPermissions(messageID).Where(x => x.UserID == userID).FirstOrDefault();
            return model?.UserID == userID && model.PermissionLevel >= (int)permission;
        }

        public static List<MessagePermissionViewModel> GetUsersPermissions(int messageID)
        {
            using (DataContext db = new DataContext())
            {
                return db.AllowedMessages.Where(x => x.MessageID == messageID && x.PermissionLevel == (int)Permission.Owner).Select(x => new MessagePermissionViewModel()
                {
                    MessageID = x.MessageID,
                    UserID = x.UserID,
                    PermissionLevel = x.PermissionLevel
                }).ToList();
            }
        }

        public static MessagePermissionViewModel GetUserPermission(int userID,int messageID)
        {
            using (DataContext db = new DataContext())
            {
                AllowedMessage message = db.AllowedMessages.FirstOrDefault(x => x.MessageID == messageID && x.UserID == userID);
                return new MessagePermissionViewModel()
                {
                    MessageID = messageID,
                    PermissionLevel = message.PermissionLevel,
                    UserID = userID,
                    UserLogin = message.User.UserLogin
                };
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
            TransactionOptions options = new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TimeSpan.MaxValue
            };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                using (DataContext db = new DataContext())
                {
                    m = db.Messages.Add(new Message()
                    {
                        Text = Content,
                        Mod = DateTime.Now,
                    });
                    db.SaveChanges();
                }
                InsertAllowedMessage(userID, m.MessageID);
                List<int> usersID = UserService.GetUserIDs();
                foreach (int ID in usersID.Where(x => x != userID))
                {
                    InsertAllowedMessage(ID, m.MessageID, Permission.Unavailable);
                }
                scope.Complete();
            }
            
        }

        public static void InsertAllowedMessage(int userID, int messageID, Permission permission = Permission.Owner)
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
            TransactionOptions options = new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TimeSpan.MaxValue
            };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
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
                scope.Complete();
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