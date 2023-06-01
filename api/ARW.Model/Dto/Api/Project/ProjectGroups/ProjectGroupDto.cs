using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ARW.Model.Models.Business.Project.ProjectGroups;

namespace ARW.Model.Dto.Api.Project.ProjectGroups
{

    /// <summary>
    /// 项目分组查询对象Api
    /// </summary>
    public class ProjectGroupQueryApiDto : PagerInfo 
    {
        public string ProjectGroupName { get; set; }

        public long ToolCustomerGuid { get; set; }

    }


    /// <summary>
    /// 项目分组详情输入对象Api
    /// </summary>
    public class ProjectGroupApiDto
    {
        [Required(ErrorMessage = "ProjectGroupGuid不能为空")]
        public long ProjectGroupGuid { get; set; }
    }
	
}
