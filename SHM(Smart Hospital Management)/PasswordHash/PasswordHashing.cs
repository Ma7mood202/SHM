using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.PasswordHash
{
    public static class PasswordHashing
    {
        public static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var asBytes = Encoding.Default.GetBytes(password);
            var hashed = Convert.ToBase64String(sha.ComputeHash(asBytes));
            return hashed;
        }
    }
}
