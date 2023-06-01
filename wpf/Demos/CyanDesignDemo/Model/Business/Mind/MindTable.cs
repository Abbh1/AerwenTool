using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyanDesignDemo.Model.Business.Mind
{
    public class MindTable : BaseModel
    {

        /// <summary>
        /// 表名称
        /// </summary>
        public string MindTableName { get;set; }

        /// <summary>
        /// 表注释
        /// </summary>
        public string MindTableAnnotation { get; set; }

        /// <summary>
        /// 字段列表
        /// </summary>
        public ObservableCollection<MindTableField> MindTableFieldList { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected { get; set; }
    }
}
