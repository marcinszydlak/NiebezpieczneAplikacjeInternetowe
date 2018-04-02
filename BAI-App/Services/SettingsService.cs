using System.Linq;
using Bai_APP.Entity;
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
    }
}