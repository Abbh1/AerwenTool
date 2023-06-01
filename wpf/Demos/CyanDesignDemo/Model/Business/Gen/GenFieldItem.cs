using DMSkin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyanDesignDemo.Model.Business.Gen
{
    public class GenFieldItem : BaseModel
    {

        public string FiledName { get; set; } 

        public string FiledAnnotate { get; set; }

        public string FiledType{ get; set; }

        public string FiledSize { get; set; }

        public string FiledDecimal { get; set; }

        private bool _isNull;
        public bool IsNull
        {
            get { return _isNull; }
            set
            {
                _isNull = value;
                OnPropertyChanged(nameof(IsNull));
            }
        }

        public bool FiledSort { get; set; }

        public bool IsSelected { get; set; }
    }
}
