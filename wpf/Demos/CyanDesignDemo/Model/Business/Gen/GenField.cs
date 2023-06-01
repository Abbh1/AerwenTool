using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyanDesignDemo.Model.Business.Gen
{
    public class GenField : BaseModel
    {

        public string FiledName { get; set; } 

        public string FiledAnnotate { get; set; }

        public string FiledType{ get; set; } = "varchar";

        public string FiledSize { get; set; } = "255";

        public string FiledDecimal { get; set; } = "0";

        public bool IsNull { get; set; } = true;

        public bool FiledSort { get; set; }

    }
}
