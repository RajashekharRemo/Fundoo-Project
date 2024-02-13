using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class PasswordConversion
    {
        public static string Key = "gdvade!@edef";

        public static string EncryptPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) return "";
            password += Key;
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(passwordBytes);
        }

        public static string DecryptPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) { return ""; }
            var decryptBytes = Convert.FromBase64String(password);
            var result = Encoding.UTF8.GetString(decryptBytes);
            result = result.Substring(0, result.Length - Key.Length);
            return result;
        }
    }
}
