using System;
using System.Collections.Generic;
using SqlSugar;
using OfficeOpenXml.Attributes;
using Newtonsoft.Json;

namespace ARW.Model.Models.Business.CreateTable.BaseFiledTemplates
{
    /// <summary>
    /// 基础字段模板，数据实体对象
    ///
    /// @author admin
    /// @date 2023-05-26
    /// </summary>
    [SugarTable("tb_base_filed_template")]
    public class BaseFiledTemplate : BusinessBase
    {

        /// <summary>
        /// 描述 : 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "BaseFiledTemplateId")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "base_filed_template_id")]
        public int BaseFiledTemplateId { get; set; }


        /// <summary>
        /// 描述 : 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "BaseFiledTemplateGuid")]
		[JsonConverter(typeof(ValueToStringConverter))]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false, ColumnName = "base_filed_template_guid")]
        public long BaseFiledTemplateGuid { get; set; }


        /// <summary>
        /// 描述 :客户guid 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "客户guid")]
		[JsonConverter(typeof(ValueToStringConverter))]
        [SugarColumn(ColumnName = "base_filed_template_customer_guid")]
        public long BaseFiledTemplateCustomerGuid { get; set; }


        /// <summary>
        /// 描述 :名称 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "名称")]
        [SugarColumn(ColumnName = "base_filed_template_name")]
        public string BaseFiledTemplateName { get; set; }


        /// <summary>
        /// 描述 :内容 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "内容")]
        [SugarColumn(ColumnName = "base_filed_template_content")]
        public string BaseFiledTemplateContent { get; set; }






    }
}