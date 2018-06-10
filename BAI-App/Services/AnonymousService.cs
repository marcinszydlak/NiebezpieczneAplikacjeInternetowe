using System;
using System.Linq;
using Bai_APP.Entity;
using Bai_APP.Models;

namespace Bai_APP.Services
{
    public class AnonymousService
    {
        public static void SaveAnonymousLoginAttempt(string login)
        {
            using (var db = new DataContext())
            {
                if (db.AnonymousUsers.Any(x => x.Login == login))
                {
                    AnonymousUser user = db.AnonymousUsers.Where(x => x.Login == login).FirstOrDefault();

                    user.Time = DateTime.UtcNow;
                    user.FailedLoginAttempts = user.FailedLoginAttempts + 1;
                    user.AccountLockedTo = DateTime.Now.AddSeconds(user.FailedLoginAttempts * 5);
                }
                else
                {
                    db.AnonymousUsers.Add(new AnonymousUser()
                    {
                        Login = login,
                        Time = DateTime.UtcNow,
                        FailedLoginAttempts = 1,
                        AccountLockedTo = DateTime.UtcNow.AddSeconds(-1)
                    });
                }

                db.SaveChanges();
            }
        }

        public static DateTime GetAccountLockedToTime(string login)
        {
            using (var db = new DataContext())
            {
                return db.AnonymousUsers.Where(x => x.Login == login).Select(x => x.AccountLockedTo).FirstOrDefault();
            }
        }
    }
}