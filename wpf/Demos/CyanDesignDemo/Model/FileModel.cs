using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyanDesignDemo.Model
{
    public class FileModel
    {
        public long FileId { get; set; }

        public string FileName { get; set; }

        public string url { get; set; }

    }
}
