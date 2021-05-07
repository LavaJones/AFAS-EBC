using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;

using log4net;

/// <summary>
/// Summary description for HashedPassword
/// </summary>
namespace HashPassword
{

    public class HashedPassword
    {

        public static String CreateSalt(String username)
        {


            Rfc2898DeriveBytes hasher = new Rfc2898DeriveBytes(username, System.Text.Encoding.UTF8.GetBytes(Authorization.BaseSalt), Authorization.NumberOfSaltPasses);

            String derivedHash = Convert.ToBase64String(hasher.GetBytes(25));

            return derivedHash;
        
        }

        public static String HashPassword(String salt, String password)
        {
            
            Rfc2898DeriveBytes Hasher = new Rfc2898DeriveBytes(password, System.Text.Encoding.UTF8.GetBytes(salt), Authorization.NumberOfPasswordPasses);

            String derivedHash = Convert.ToBase64String(Hasher.GetBytes(25));

            return derivedHash;
        
        }

        public static Boolean VerifyHashedPassword(String hashedPassword, String username, String password)
        {
            
            String salt = CreateSalt(username);
            String hashEnteredPassword = HashPassword(salt, password);

            HashedPassword.Log.Debug(String.Format("Do they Match? {0}.", hashedPassword == hashEnteredPassword));

            if (hashedPassword == hashEnteredPassword)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private static ILog Log = LogManager.GetLogger(typeof(HashedPassword));

    }

}