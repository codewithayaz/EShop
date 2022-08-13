using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Core
{
    public static class Helper
    {
        public static string RandomString(int length = 8)
        {
            Random rand = new Random();
            const string pool = "!@#$abcdefghijklmnopqrstuvwyxzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var chars = Enumerable.Range(0, length)
                .Select(x => pool[rand.Next(0, pool.Length)]);
            return new string(chars.ToArray());
        }
    }
}
