using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Core.Interfaces
{
    public interface IUserSession
    {
        string UserId { get; set; }
        string UserName { get; set; }
        string AccountType { get; set; }
        List<string> Roles { get; set; }
        List<KeyValuePair<string, string>> ExposedClaims { get; set; }
    }
}
