using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ARW.Model.Models.Business.Project.ProjectFiless;

namespace ARW.Model.Dto.Business.Project.ProjectFiless
{
    /// <summary>
    /// 项目配置输入对象
    /// </summary>
    public class ProjectFilesDto
    {
        public int ProjectFilesId { get; set; }
        public long ProjectFilesGuid { get; set; }
        [Required(ErrorMessage = "客户guid不能为空")]
        public long CustomerGuid { get; set; }
        [Required(ErrorMessage = "项目guid不能为空")]
        public long ProjectGuid { get; set; }
        [Required(ErrorMessage = "标题不能为空")]
        public string ProjectFilesTitle { get; set; }
        [Required(ErrorMessage = "打开方式类型不能为空")]
        public int ProjectFilesOpenMethodType { get; set; }
        public string ProjectFilesOpenMethodPath { get; set; }
        [Required(ErrorMessage = "文件打开类型不能为空")]
        public int ProjectFilesFileOpenType { get; set; }
        [Required(ErrorMessage = "文件打开路径不能为空")]
        public string ProjectFilesFileOpenPath { get; set; }
        [Required(ErrorMessage = "是否需要启动Git拉取不能为空")]
        public int ProjectFilesIsGit { get; set; }
        public int ProjectFilesIsOpen { get; set; }

        public string ProjectFilesGitPath { get; set; }
    }


    /// <summary>
    /// 项目配置查询对象
    /// </summary>
    public class ProjectFilesQueryDto : PagerInfo 
    {
        public string ProjectFilesTitle { get; set; }
        public int? ProjectFilesOpenMethodType { get; set; }
        public int? ProjectFilesFileOpenType { get; set; }
        public int? ProjectFilesIsGit { get; set; }
    
        public string ids { get; set; }
    }




}
