using Newtonsoft.Json;
using OfficeOpenXml.Attributes;
using SqlSugar;
using System;

namespace ARW.Model.Vo.Business.CreateTable.BaseFiledTemplates
{
    /// <summary>
    /// 基础字段模板展示对象
    /// </summary>
    public class BaseFiledTemplateVo
    {

        /// <summary>
        /// 描述 : 
        /// </summary>
		[EpplusIgnore]
        public int BaseFiledTemplateId { get; set; }

        /// <summary>
        /// 描述 : 
        /// </summary>
		[JsonConverter(typeof(ValueToStringConverter))]
		[EpplusIgnore]
        public long BaseFiledTemplateGuid { get; set; }

        /// <summary>
        /// 描述 :客户guid 
        /// </summary>
		[JsonConverter(typeof(ValueToStringConverter))]
        public long BaseFiledTemplateCustomerGuid { get; set; }

        /// <summary>
        /// 描述 :客户名称
        /// </summary>
        [JsonConverter(typeof(ValueToStringConverter))]
        [EpplusTableColumn(Header = "客户名称")]
        public string BaseFiledTemplateCustomerName { get; set; }

        /// <summary>
        /// 描述 :名称 
        /// </summary>
        [EpplusTableColumn(Header = "名称")]
        public string BaseFiledTemplateName { get; set; }

        /// <summary>
        /// 描述 :内容 
        /// </summary>
        [EpplusTableColumn(Header = "内容")]
        public string BaseFiledTemplateContent { get; set; }

    }
}
