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
                    AttemptsToLockAccount = x.AttemptsToLockAccount,
                    AccountLockedTo = x.AccountLockedTo
                }).FirstOrDefault();
            }
        }

        public static bool IsAccountLocked(string login)
        {
            UserSettingsViewModel userSettingsViewModel = GetUserSettings(login);

            return userSettingsViewModel.AccountLockedTo > DateTime.UtcNow;
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

        internal static void HandleSuccessLogin(DataContext db, string login)
        {
            User user = db.Users.Where(x => x.UserLogin == login).FirstOrDefault();

            if (user != null && !IsAccountLocked(login))
            {
                user.LastSuccessLoginDate = user.CurrentLoginDate;
                user.CurrentLoginDate = DateTime.UtcNow;

                user.FailedLoginAttemptsCountSinceLastSuccessful =
                    user.FailedLoginAttemptsCountSinceLastSuccessful > 0
                    ? user.FailedLoginAttemptsCountSinceLastSuccessful - 1
                    : 0;

                user.AccountLockedTo = DateTime.UtcNow;

                db.SaveChanges();
            }
        }

        public static void HandleFailedLogin(UserLoginViewModel model, DataContext db)
        {
            bool userExists = db.Users.Any(x => x.UserLogin == model.Login);

            if (userExists)
            {
                SaveBadLoginAttempt(model.Login, db);
            }
            else
            {
                SaveAnonymousLoginAttempt(model.Login, db);
            }
        }

        private static void SaveBadLoginAttempt(string login, DataContext db)
        {
            User user = db.Users.Where(x => x.UserLogin == login).FirstOrDefault();

            if (user != null)
            {
                user.FailedLoginAttemptsCountSinceLastSuccessful = user.FailedLoginAttemptsCountSinceLastSuccessful + 1;
                user.LastFailLoginDate = DateTime.UtcNow;
                user.AccountLockedTo = DateTime.UtcNow.AddMinutes(user.FailedLoginAttemptsCountSinceLastSuccessful);

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