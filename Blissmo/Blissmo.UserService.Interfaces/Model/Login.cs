using Blissmo.Helpers.Encryption;
using System;

namespace Blissmo.UserService.Interfaces.Model
{
    public class Login
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public User User { get; set; }

        public static string SetPasswordHash(string value)
        {
            var hash = new PasswordHash(value);
            byte[] hashBytes = hash.ToArray();
            return Convert.ToBase64String(hashBytes);
        }
    }
}
