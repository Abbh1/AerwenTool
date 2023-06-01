using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyanDesignDemo.Model.Business.Gen
{
    public class GenVersion : BaseModel
    {

        public bool IsMysql { get; set; } = true;

        public bool IsSqlserver { get; set; }

    }
}
