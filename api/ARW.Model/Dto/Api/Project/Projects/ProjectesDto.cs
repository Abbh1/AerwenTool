using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ARW.Model.Models.Business.Project.Projects;

namespace ARW.Model.Dto.Api.Project.Projects
{

    /// <summary>
    /// 项目查询对象Api
    /// </summary>
    public class ProjectesQueryApiDto : PagerInfo 
    {
        public string ProjectName { get; set; }
        public long ToolCustomerGuid { get; set; }
        public long ProjectGroupGuid { get; set; }
        public long ProjectGuid { get; set; }
    }


    /// <summary>
    /// 项目详情输入对象Api
    /// </summary>
    public class ProjectesApiDto
    {
        [Required(ErrorMessage = "ProjectesGuid不能为空")]
        public long ProjectesGuid { get; set; }
    }
	
}
