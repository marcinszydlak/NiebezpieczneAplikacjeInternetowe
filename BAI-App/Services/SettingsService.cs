using System;
using System.Linq;
using Bai_APP.Entity;
using Bai_APP.Entity.ViewModels;
using Bai_APP.Models.ViewModels;

namespace Bai_APP.Services
{
    public class SettingsService
    {
        public static UserSettingsViewModel GetUserSettings(string userLogin)
        {
            using (var db = new DataContext())
            {
                return db.Users.Where(x => x.UserLogin == userLogin).Select(x => new UserSettingsViewModel()
                {
                    UserId = x.UserID,
                    FailedLoginAttemptsCountSinceLastSuccessful = x.FailedLoginAttemptsCountSinceLastSuccessful,
                    LastFailLoginDate = x.LastFailLoginDate,
                    LastSuccessLoginDate = x.LastSuccessLoginDate,
                    AttemptsToLockAccount = x.AttemptsToLockAccount
                }).First();
            }
        }

        public static bool IsAccountLocked(string login)
        {
            UserSettingsViewModel userSettingsViewModel = GetUserSettings(login);

            return userSettingsViewModel.FailedLoginAttemptsCountSinceLastSuccessful >= userSettingsViewModel.AttemptsToLockAccount;
        }

        public static void UpdateAttemptsToLockAccount(string login, int attemptsToLockAccount)
        {
            using (DataContext db = new DataContext())
            {
                User user = db.Users.Where(x => x.UserLogin == login).FirstOrDefault();

                if (user != null)
                {
                    user.AttemptsToLockAccount = attemptsToLockAccount;
                    db.SaveChanges();
                }
            }
        }

        public static void SaveLoginAttempt(UserLoginViewModel model, DataContext db)
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