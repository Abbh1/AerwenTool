using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ARW.Model.Models.Business.Download.DownloadFiless;

namespace ARW.Model.Dto.Api.Download.DownloadFiless
{

    /// <summary>
    /// 下载文件查询对象Api
    /// </summary>
    public class DownloadFilesQueryApiDto : PagerInfo 
    {
        public long DownloadCategoryGuid { get; set; }
        public string DownloadFilesName { get; set; }
        public string DownloadFilesLink { get; set; }
        public int? DownloadFilesAuditStatus { get; set; }
    }
	
	
	/// <summary>
    /// 下载文件详情输入对象Api
    /// </summary>
    public class DownloadFilesApiDto
    {
        [Required(ErrorMessage = "DownloadFilesGuid不能为空")]
        public long DownloadFilesGuid { get; set; }
    }
	
}
