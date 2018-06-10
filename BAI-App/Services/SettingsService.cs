using System;
using System.Linq;
using Bai_APP.Entity;
using Bai_APP.Entity.ViewModels;
using Bai_APP.Models.ViewModels;

namespace Bai_APP.Services
{
    public class SettingsService
    {
        public static UserSettingsViewModel _userSettingsViewModel { get; private set; }

        public static UserSettingsViewModel GetUserSettings(string userLogin)
        {
            using (var db = new DataContext())
            {
                UserSettingsViewModel userSettingsViewModel = db.Users.Where(x => x.UserLogin == userLogin).Select(x => new UserSettingsViewModel()
                {
                    UserId = x.UserID,
                    FailedLoginAttemptsCountSinceLastSuccessful = x.FailedLoginAttemptsCountSinceLastSuccessful,
                    LastFailLoginDate = x.LastFailLoginDate,
                    LastSuccessLoginDate = x.LastSuccessLoginDate,
                    AttemptsToLockAccount = x.AttemptsToLockAccount,
                    AccountLockedTo = x.AccountLockedTo,
                    IsAccountLockedPermamently = x.IsAccountLockedPermamently
                }).FirstOrDefault();

                userSettingsViewModel.LastFailLoginDate = userSettingsViewModel.LastFailLoginDate.AddHours(2);
                userSettingsViewModel.LastSuccessLoginDate = userSettingsViewModel.LastSuccessLoginDate.AddHours(2);
                userSettingsViewModel.AccountLockedTo = userSettingsViewModel.AccountLockedTo.AddHours(2);

                return userSettingsViewModel;
            }
        }

        public static bool IsAccountLocked(string login)
        {
            return GetUserSettings(login).IsAccountLockedPermamently;
        }

        public static bool IsLoggingDelayedFor(string login)
        {
            _userSettingsViewModel = GetUserSettings(login);

            return _userSettingsViewModel.AccountLockedTo > DateTime.UtcNow.AddHours(2);
        }

        public static void UpdateAttemptsToLockAccount(string login, int attemptsToLockAccount)
        {
            using (var db = new DataContext())
            {
                User user = db.Users.Where(x => x.UserLogin == login).FirstOrDefault();

                if (user != null)
                {
                    user.AttemptsToLockAccount = attemptsToLockAccount;
                    db.SaveChanges();
                }
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
                AnonymousService.SaveAnonymousLoginAttempt(model.Login);
            }
        }

        public static void HandleSuccessLogin(DataContext db, string login)
        {
            User user = db.Users.Where(x => x.UserLogin == login).FirstOrDefault();

            if (user != null && !IsLoggingDelayedFor(login))
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

        private static void SaveBadLoginAttempt(string login, DataContext db)
        {
            User user = db.Users.Where(x => x.UserLogin == login).FirstOrDefault();

            if (user == null)
            {
                return;
            }

            user.FailedLoginAttemptsCountSinceLastSuccessful = user.FailedLoginAttemptsCountSinceLastSuccessful + 1;
            user.LastFailLoginDate = DateTime.UtcNow;
            user.AccountLockedTo = DateTime.UtcNow.AddSeconds(user.FailedLoginAttemptsCountSinceLastSuccessful * 5);

            if (user.AttemptsToLockAccount <= user.FailedLoginAttemptsCountSinceLastSuccessful)
            {
                user.IsAccountLockedPermamently = true;
            }

            db.SaveChanges();
        }
    }
}