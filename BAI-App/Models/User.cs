using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bai_APP.Entity
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [DisplayName("Login")]
        public string UserLogin { get; set; }

        [DisplayName("Hasło")]
        public string PasswordHash { get; set; }
        public string Salt { get; set; }

        [DisplayName("Data ostatniego poprawnego logowania")]
        public DateTime LastSuccessLoginDate { get; set; } = DateTime.Now;

        [DisplayName("Data ostatniego nieudanego logowania")]
        public DateTime LastFailLoginDate { get; set; }

        [DisplayName("Liczba nieudanych logowań od ostatniego poprawnego logowania")]
        public int FailedLoginAttemptsCountSinceLastSuccessful { get; set; }

        [DisplayName("Liczba prób logowań do zablokowania konta")]
        public int AttemptsToLockAccount { get; set; }

        public virtual ICollection<AllowedMessage> AllowedMessages { get; set; }
    }
}