using Bai_APP.Entity;
using Bai_APP.Entity.ViewModels;
using Bai_APP.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bai_APP.Services
{
    public static class UserService
    {     
        public static LoggedUserViewModel Login(UserLoginViewModel model)
        {
            using (DataContext db = new DataContext())
            {
                return db.Users.Where(x => x.UserLogin == model.Login && x.PasswordHash == model.Password)
                .Select(x => new LoggedUserViewModel()
                {
                    UserID = x.UserID,
                    Login = x.UserLogin,
                    Messages = db.AllowedMessages.Where(y => y.UserID == x.UserID && y.PermissionLevel != (int)Permission.Unavailable).Select(y => y.Message).Select(y => new MessageViewModel()
                    {
                        MessageID = y.MessageID,
                        MessageText = y.Text
                    }).ToList(),
                }).FirstOrDefault();
            }  
        }

        public static bool CheckUserLoginExists(string login)
        {
            using (DataContext db = new DataContext())
            {
                return db.Users.FirstOrDefault(x => x.UserLogin == login) != null;
            }
        }

        public static List<int> GetUserIDs()
        {
            using (DataContext db = new DataContext())
            {
                return db.Users.Select(x => x.UserID).ToList();
            }
        }

        public static int RegisterUser(RegisterUserViewModel model)
        {
            using (DataContext db = new DataContext())
            {
                User u = new User()
                {
                    UserLogin = model.UserLogin,
                    PasswordHash = model.Password1,
                    LastLogin = DateTime.Now,
                    Salt = string.Empty
                };
                db.Users.Add(u);
                db.SaveChanges();
                return u.UserID;
            }

        }
    }
}