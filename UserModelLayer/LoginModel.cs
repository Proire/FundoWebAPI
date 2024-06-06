using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserModelLayer
{
    public class LoginModel(string username, string password)
    {
        public string UserName { get; set; } = username;
        public string Password { get; set; } = password;
    }
}
