using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ARW.Model.Models.Business.CreateTable.BaseFiledTemplates;

namespace ARW.Model.Dto.Business.CreateTable.BaseFiledTemplates
{
    /// <summary>
    /// 基础字段模板输入对象
    /// </summary>
    public class BaseFiledTemplateDto
    {
        public int BaseFiledTemplateId { get; set; }
        public long BaseFiledTemplateGuid { get; set; }
        [Required(ErrorMessage = "客户guid不能为空")]
        public long BaseFiledTemplateCustomerGuid { get; set; }
        [Required(ErrorMessage = "名称不能为空")]
        public string BaseFiledTemplateName { get; set; }
        [Required(ErrorMessage = "内容不能为空")]
        public string BaseFiledTemplateContent { get; set; }
    }


    /// <summary>
    /// 基础字段模板查询对象
    /// </summary>
    public class BaseFiledTemplateQueryDto : PagerInfo 
    {
        public string BaseFiledTemplateName { get; set; }
    
        public string ids { get; set; }
    }




}
