using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ARW.Model.Models.Business.Download.DownloadCategorys;

namespace ARW.Model.Dto.Api.Download.DownloadCategorys
{

    /// <summary>
    /// 下载分类查询对象Api
    /// </summary>
    public class DownloadCategoryQueryDtoApi : PagerInfo 
    {
        public string DownloadCategoryName { get; set; }
        public int? DownloadCategoryAuditStatus { get; set; }
    }
	
	
	/// <summary>
    /// 下载分类详情输入对象Api
    /// </summary>
    public class DownloadCategoryDtoApi
    {
        [Required(ErrorMessage = "DownloadCategoryGuid不能为空")]
        public long DownloadCategoryGuid { get; set; }
    }
	
}
