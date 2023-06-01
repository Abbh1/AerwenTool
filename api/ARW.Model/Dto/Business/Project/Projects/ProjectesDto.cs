using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ARW.Model.Models.Business.Project.Projects;

namespace ARW.Model.Dto.Business.Project.Projects
{
    /// <summary>
    /// 项目输入对象
    /// </summary>
    public class ProjectesDto
    {
        public int ProjectId { get; set; }
        public long ProjectGuid { get; set; }
        [Required(ErrorMessage = "客户guid不能为空")]
        public long ProjectCustomerGuid { get; set; }
        public long ProjectGroupGuid { get; set; }
        [Required(ErrorMessage = "项目名称不能为空")]
        public string ProjectName { get; set; }
        public string ProjectImg { get; set; }
        public string ProjectIntro { get; set; }
        public int? ProjectSort { get; set; }
    }


    /// <summary>
    /// 项目查询对象
    /// </summary>
    public class ProjectesQueryDto : PagerInfo 
    {
        public string ProjectName { get; set; }
    
        public string ids { get; set; }
    }




}
