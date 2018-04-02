using System;
using System.Collections.Generic;
using System.Linq;
using Bai_APP.Entity;
using Bai_APP.Entity.ViewModels;
using Bai_APP.Models.Enums;

namespace Bai_APP.Services
{
    public static class UserService
    {
        public static LoggedUserViewModel Login(UserLoginViewModel model)
        {
            using (DataContext db = new DataContext())
            {
                return GetUser(model, db);
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

        private static LoggedUserViewModel GetUser(UserLoginViewModel model, DataContext db)
        {
            LoggedUserViewModel loggedUserViewModel = db.Users
                .Where(x => x.UserLogin == model.Login && x.PasswordHash == model.Password)
                .Select(x => new LoggedUserViewModel()
                {
                    UserID = x.UserID,
                    Login = x.UserLogin,
                    Messages = db.AllowedMessages
                        .Where(y => y.UserID == x.UserID && y.PermissionLevel != (int)Permission.Unavailable)
                        .Select(y => y.Message).Select(y => new MessageViewModel()
                        {
                            MessageID = y.MessageID,
                            MessageText = y.Text
                        }).ToList(),
                }).FirstOrDefault();

            if (loggedUserViewModel == null)
            {
                SaveLoginAttempt(model, db);
            }

            return loggedUserViewModel;
        }

        private static void SaveLoginAttempt(UserLoginViewModel model, DataContext db)
        {
            bool userExists = db.Users.Any(x => x.UserLogin == model.Login);

            if (userExists)
            {
                IncrementFailedLoginAttempts(model.Login, db);
            }
            else
            {
                SaveAnonymousLoginAttempt(model.Login, db);
            }
        }

        private static void IncrementFailedLoginAttempts(string login, DataContext db)
        {
            User user = db.Users.Where(x => x.UserLogin == login).FirstOrDefault();

            if (user != null)
            {
                user.FailedLoginAttemptsCountSinceLastSuccessful = user.FailedLoginAttemptsCountSinceLastSuccessful + 1;
                db.SaveChanges();
            }
        }

        private static void SaveAnonymousLoginAttempt(string login, DataContext db)
        {
            db.AnonymousUsers.Add(new Models.AnonymousUser()
            {
                Login = login,
                Time = DateTime.UtcNow
            });

            db.SaveChanges();
        }        
    }
}