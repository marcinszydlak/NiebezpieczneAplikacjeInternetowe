using System;
using System.ComponentModel;

namespace Bai_APP.Models.ViewModels
{
    public class UserSettingsViewModel
    {
        public int UserId { get; set; }

        [DisplayName("Data ostatniego poprawnego logowania")]
        public DateTime LastSuccessLoginDate { get; set; }

        [DisplayName("Data ostatniego nieudanego logowania")]
        public DateTime LastFailLoginDate { get; set; }

        [DisplayName("Liczba nieudanych logowań od ostatniego poprawnego logowania")]
        public int FailedLoginAttemptsCountSinceLastSuccessful { get; set; }

        [DisplayName("Liczba prób logowań do zablokowania konta")]
        public int AttemptsToLockAccount { get; set; }

        public DateTime AccountLockedTo { get; set; }
    }
}