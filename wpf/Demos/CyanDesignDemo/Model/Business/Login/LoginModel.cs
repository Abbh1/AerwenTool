
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CyanDesignDemo.Model.Business.Login
{
    public class LoginModel
    {

        public int ToolCustomerId { get; set; }

        public long ToolCustomerGuid { get; set; }

        public string ToolCustomerName { get; set; }

        public string ToolCustomerPassword { get; set; }

        public string ToolCustomerPhoneNumber { get; set; }

        public string ToolCustomerEmail { get; set; }

        public string jwt { get; set; }
        public DateTime ExpirationDate { get; set; }

    }
}
