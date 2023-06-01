using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ARW.Model.Models.Business.Download.DownloadFiless;

namespace ARW.Model.Dto.Business.Download.DownloadFiless
{
    /// <summary>
    /// 下载文件输入对象
    /// </summary>
    public class DownloadFilesDto
    {
        public int DownloadFilesId { get; set; }
        public long DownloadFilesGuid { get; set; }
        [Required(ErrorMessage = "下载分类guid不能为空")]
        public long DownloadCategoryGuid { get; set; }
        [Required(ErrorMessage = "图标不能为空")]
        public string DownloadFilesIcon { get; set; }
        [Required(ErrorMessage = "名称不能为空")]
        public string DownloadFilesName { get; set; }
        public string DownloadFilesIntro { get; set; }
        [Required(ErrorMessage = "下载链接不能为空")]
        public string DownloadFilesLink { get; set; }
        public string DownloadFilesAttachment { get; set; }
        [Required(ErrorMessage = "大小不能为空")]
        public decimal DownloadFilesSize { get; set; }
        [Required(ErrorMessage = "下载量不能为空")]
        public int DownloadFilesVolume { get; set; }
        public int DownloadFilesAuditStatus { get; set; }
        public long DownloadFilesAuditUserGuid { get; set; }
    }


    /// <summary>
    /// 下载文件查询对象
    /// </summary>
    public class DownloadFilesQueryDto : PagerInfo 
    {
        public string DownloadFilesName { get; set; }
        public string DownloadFilesLink { get; set; }
        public int? DownloadFilesAuditStatus { get; set; }
    
        public string ids { get; set; }
    }


		/// <summary>
        /// 审核对象
        /// </summary>
        public class DownloadFilesAuditDto
        {
            public int DownloadFilesAuditStatus { get; set; }
            public string ids { get; set; }
        }


}
