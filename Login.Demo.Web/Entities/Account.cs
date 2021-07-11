using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Login.Demo.Web.Entities
{
    public class Account
    {
        public Guid AccountId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public int?  Age { get; set; }
        public int? Gender { get; set; }
        public string Role { get; set; }
    }
}
