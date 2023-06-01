using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ARW.Model.Models.Business.Download.DownloadCategorys;

namespace ARW.Model.Dto.Business.Download.DownloadCategorys
{
    /// <summary>
    /// 下载分类输入对象
    /// </summary>
    public class DownloadCategoryDto
    {
        public int DownloadCategoryId { get; set; }
        public long DownloadCategoryGuid { get; set; }
        [Required(ErrorMessage = "父级guid不能为空")]
        public long DownloadCategoryParentGuid { get; set; }
        public string DownloadCategoryAncestralGuid { get; set; }
        [Required(ErrorMessage = "名称不能为空")]
        public string DownloadCategoryName { get; set; }
        [Required(ErrorMessage = "排序不能为空")]
        public int DownloadCategorySort { get; set; }
        public int? DownloadCategoryAuditStatus { get; set; }
        public long? DownloadCategoryAuditUserGuid { get; set; }
    }


    /// <summary>
    /// 下载分类查询对象
    /// </summary>
    public class DownloadCategoryQueryDto : PagerInfo 
    {
        public string DownloadCategoryName { get; set; }
        public int? DownloadCategoryAuditStatus { get; set; }
    
        public string ids { get; set; }
    }


		/// <summary>
        /// 审核对象
        /// </summary>
        public class DownloadCategoryAuditDto
        {
            public int DownloadCategoryAuditStatus { get; set; }
            public string ids { get; set; }
        }


}
