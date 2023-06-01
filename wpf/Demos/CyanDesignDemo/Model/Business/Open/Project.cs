using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Runtime.Remoting.Messaging;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CyanDesignDemo.Model.Business.Open
{
    public class Project : BaseModel
    {
        public int ProjectId { get; set; }
        
        public long ProjectGuid { get; set; }
        
        public long ProjectCustomerGuid { get; set; }
        
        public long ProjectGroupGuid { get; set; }
        
        public string ProjectName { get; set; }
        
        public string ProjectImg { get; set; }
        
        public string ProjectIntro { get; set; }
        
        public int? ProjectSort { get; set; }
        
        public string CustomerName { get; set; }
        
        public string ProjectGroupName { get; set; }

        public List<Project> ProjectList { get; set; }

        public Project ProjectForm { get; set; }

        public bool IsRefresh { get; set; }
    }

}
