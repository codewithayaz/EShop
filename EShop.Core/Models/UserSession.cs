using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Core.Interfaces;

namespace EShop.Core.Models
{
    public class UserSession : IUserSession
    {
        public bool IsAuthenticated { get; set; }
        public string UserId { get; set; }
        public string AccountType { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> Roles { get; set; }
        public List<KeyValuePair<string, string>> ExposedClaims { get; set; }

        public UserSession()
        {
        }

        public UserSession(string userId, string userName)
        {
            UserId = userId;
            UserName = userName;
        }
    }
}
