using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Login.Demo.Web.Entities
{
    public class ResponseLogin
    {
        public string Token { get; set; }
        public Account UserInfo { get; set; }
    }
}
