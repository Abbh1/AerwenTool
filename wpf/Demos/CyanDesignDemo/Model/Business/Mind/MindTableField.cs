using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyanDesignDemo.Model.Business.Mind
{
    public class MindTableField : BaseModel
    {

        /// <summary>
        /// 字段名称
        /// </summary>
        public string MindTableFieldName { get;set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public string MindTableFieldType { get; set; }

        /// <summary>
        /// 字段大小
        /// </summary>
        public string MindTableFieldSize { get; set; }

        /// <summary>
        /// 字段小数点
        /// </summary>
        public string MindTableFieldDecimal { get; set; }

        /// <summary>
        /// 是否为空
        /// </summary>
        public string IsNull { get; set; }

        /// <summary>
        /// 字段注释
        /// </summary>
        public string MindTableFieldAnnotation { get; set; }


    }
}
