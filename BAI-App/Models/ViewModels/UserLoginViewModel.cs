using System.ComponentModel;

namespace Bai_APP.Entity.ViewModels
{
    public class UserLoginViewModel
    {
        [DisplayName("Login")]
        public string Login { get; set; }
        [DisplayName("Hasło")]
        public string Password { get; set; }
        public override string ToString()
        {
            return Login;
        }
    }
}