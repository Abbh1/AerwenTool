using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CyanDesignDemo.Model.Business.CreateTable
{
    public class BaseFiledTemplate : BaseModel
    {

        public int BaseFiledTemplateId { get; set; }

        public long BaseFiledTemplateGuid { get; set; }

        public long BaseFiledTemplateCustomerGuid { get; set; }

        public string BaseFiledTemplateName { get; set; }

        public string BaseFiledTemplateContent { get; set; }

        public bool IsShow { get; set; }
    }
}
