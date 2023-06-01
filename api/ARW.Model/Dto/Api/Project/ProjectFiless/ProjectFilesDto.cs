using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ARW.Model.Models.Business.Project.ProjectFiless;

namespace ARW.Model.Dto.Api.Project.ProjectFiless
{

    /// <summary>
    /// 项目配置查询对象Api
    /// </summary>
    public class ProjectFilesQueryApiDto : PagerInfo 
    {
        public long ProjectGuid { get; set; }
        public long CustomerGuid { get; set; }
        public string ProjectFilesTitle { get; set; }
        public int? ProjectFilesOpenMethodType { get; set; }
        public int? ProjectFilesFileOpenType { get; set; }
        public int? ProjectFilesIsGit { get; set; }
    }
	
	
	/// <summary>
    /// 项目配置详情输入对象Api
    /// </summary>
    public class ProjectFilesApiDto
    {
        [Required(ErrorMessage = "ProjectFilesGuid不能为空")]
        public long ProjectFilesGuid { get; set; }
    }
	
}
