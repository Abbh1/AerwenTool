using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyanDesignDemo.Model.Business.Gen
{
    public class GenTable : BaseModel
    {
        /// <summary>
        /// 表注释
        /// </summary>
        public string TableAnnotation { get; set; }

        /// <summary>
        /// 表名称
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 是否开启表前缀
        /// </summary>
        public bool IsTableNamePrefix { get; set; } = false;

        /// <summary>
        /// 表前缀
        /// </summary>
        public string TableNamePrefix { get; set; } = "";


        /// <summary>
        /// 是否开启字段前缀
        /// </summary>
        public bool IsTablePrefix { get; set; } = true;

        /// <summary>
        /// 字段前缀
        /// </summary>
        public string TablePrefix { get; set; }

        public long CreateTableTemplateGuid { get; set; }

        /// <summary>
        /// 基础字段模板名称
        /// </summary>
        public string CreateTableTemplateName { get; set; }

        /// <summary>
        /// 基础字段模板内容
        /// </summary>
        public string CreateTableTemplateContent { get; set; }

    }
}
