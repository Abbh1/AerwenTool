using Newtonsoft.Json;
using OfficeOpenXml.Attributes;
using SqlSugar;
using System;

namespace ARW.Model.Vo.Api.CreateTable.BaseFiledTemplates
{
    /// <summary>
    /// 基础字段模板展示对象
    /// </summary>
    public class BaseFiledTemplateApiVo
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
        [EpplusTableColumn(Header = "客户guid")]
        public long BaseFiledTemplateCustomerGuid { get; set; }

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
	
	
	/// <summary>
    /// 基础字段模板详情展示对象Api
    /// </summary>
    public class BaseFiledTemplateApiDetailsVo
    {
		[EpplusIgnore]
        public int BaseFiledTemplateId { get; set; }
		[JsonConverter(typeof(ValueToStringConverter))]
		[EpplusIgnore]
        public long BaseFiledTemplateGuid { get; set; }
		[JsonConverter(typeof(ValueToStringConverter))]
        [EpplusTableColumn(Header = "客户guid")]
        public long BaseFiledTemplateCustomerGuid { get; set; }
        [EpplusTableColumn(Header = "名称")]
        public string BaseFiledTemplateName { get; set; }
        [EpplusTableColumn(Header = "内容")]
        public string BaseFiledTemplateContent { get; set; }

    }
	
}
