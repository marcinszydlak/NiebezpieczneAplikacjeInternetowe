using Bai_APP.Entity;
using Bai_APP.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bai_APP.Services
{
    public static class UserService
    {     
        public static LoggedUserModel Login(UserLoginViewModel model)
        {
            using (DataContext db = new DataContext())
            {
                return db.Users.Where(x => x.UserLogin == model.Login && x.PasswordHash == model.Password)
                .Select(x => new LoggedUserModel()
                {
                    UserID = x.UserID,
                    Login = x.UserLogin,
                    Messages = db.AllowedMessages.Where(y => y.User.UserLogin == model.Login).Select(y => y.Message).Select(y => new MessageViewModel()
                    {
                        MessageID = y.MessageID,
                        MessageText = y.Text
                    }).ToList()
                }).FirstOrDefault();
            }  
        }

        public static List<int> GetUserIDs()
        {
            using (DataContext db = new DataContext())
            {
                return db.Users.Select(x => x.UserID).ToList();
            }
        }

        public static void RegisterUser(RegisterUserViewModel model)
        {
            using (DataContext db = new DataContext())
            {
                db.Users.Add(new User()
                {
                    UserLogin = model.UserLogin,
                    PasswordHash = model.Password1
                });
                db.SaveChanges();
            }

        }
    }
}