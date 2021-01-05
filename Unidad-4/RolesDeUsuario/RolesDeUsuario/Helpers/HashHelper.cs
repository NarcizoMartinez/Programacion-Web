using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

namespace RolesDeUsuario.Helpers
{
    public class HashHelper
    {
        public static string GetHash(string chain)
        {
            var t = SHA256.Create();
            byte[] encode = Encoding.UTF8.GetBytes(chain);
            byte[] hash = t.ComputeHash(encode);
            string c = "";
            foreach (var item in hash)
            {
                c += item.ToString("x2");
            }
            return c;
        }
    }
}
