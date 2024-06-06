using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace UserRLL.Utilities
{
    public static class PasswordHasher
    {
        // Method to hash a password
        public static string HashPassword(string password)
        {
            // Generate the hash with a default salt
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Method to verify a password against a hash
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            // Verify the password with the stored hash
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }

}
