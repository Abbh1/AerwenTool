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
    public class ProjectGroupForm : BaseModel
    {

        public int ProjectGroupId { get; set; }


        public long ProjectGroupGuid { get; set; }


        public long? ProjectGroupParentGuid { get; set; }

        public string ProjectGroupAncestralGuid { get; set; }

        public long ProjectGroupCustomerGuid { get; set; }

        public string ProjectGroupName { get; set; }

        public int? ProjectGroupSort { get; set; }

        public List<ProjectGroup> Children { get; set; }
        public bool IsShow { get; set; } = false;

        public string Title { get; set; }

    }

}
