using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ARW.Model.Models.Business.Project.ProjectGroups;

namespace ARW.Model.Dto.Business.Project.ProjectGroups
{
    /// <summary>
    /// 项目分组输入对象
    /// </summary>
    public class ProjectGroupDto
    {
        public int ProjectGroupId { get; set; }
        public long ProjectGroupGuid { get; set; }
        public long? ProjectGroupParentGuid { get; set; }
        public string ProjectGroupAncestralGuid { get; set; }
        [Required(ErrorMessage = "客户guid不能为空")]
        public long ProjectGroupCustomerGuid { get; set; }
        [Required(ErrorMessage = "分组名称不能为空")]
        public string ProjectGroupName { get; set; }
        public int? ProjectGroupSort { get; set; }
    }


    /// <summary>
    /// 项目分组查询对象
    /// </summary>
    public class ProjectGroupQueryDto : PagerInfo 
    {
        public string ProjectGroupName { get; set; }
    
        public string ids { get; set; }
    }




}
