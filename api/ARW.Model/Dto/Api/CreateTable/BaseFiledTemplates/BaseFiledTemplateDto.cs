using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ARW.Model.Models.Business.CreateTable.BaseFiledTemplates;

namespace ARW.Model.Dto.Api.CreateTable.BaseFiledTemplates
{

    /// <summary>
    /// 基础字段模板查询对象Api
    /// </summary>
    public class BaseFiledTemplateQueryApiDto : PagerInfo 
    {
        public string BaseFiledTemplateName { get; set; }

        [Required(ErrorMessage = "ToolCustomerGuid不能为空")]
        public long ToolCustomerGuid { get; set; }
    }
	
	
	/// <summary>
    /// 基础字段模板详情输入对象Api
    /// </summary>
    public class BaseFiledTemplateApiDto
    {
        [Required(ErrorMessage = "BaseFiledTemplateGuid不能为空")]
        public long BaseFiledTemplateGuid { get; set; }
    }
	
}
