using Newtonsoft.Json;
using OfficeOpenXml.Attributes;
using SqlSugar;
using System;
using ARW.Model.Models.Business.Download.DownloadCategorys;
using System.Collections.Generic;

namespace ARW.Model.Vo.Api.Download.DownloadCategorys
{
    /// <summary>
    /// 下载分类展示对象
    /// </summary>
    public class DownloadCategoryVoApi
    {

        /// <summary>
        /// 描述 : 
        /// </summary>
		[EpplusIgnore]
        public int DownloadCategoryId { get; set; }

        /// <summary>
        /// 描述 : 
        /// </summary>
		[JsonConverter(typeof(ValueToStringConverter))]
		[SugarColumn(IsTreeKey = true)]
		[EpplusIgnore]
        public long DownloadCategoryGuid { get; set; }

        /// <summary>
        /// 描述 :父级guid 
        /// </summary>
		[JsonConverter(typeof(ValueToStringConverter))]
		[SugarColumn(IsTreeKey = true)]
        [EpplusTableColumn(Header = "父级guid")]
        public long DownloadCategoryParentGuid { get; set; }

        /// <summary>
        /// 描述 :祖级guid 
        /// </summary>
		[JsonConverter(typeof(ValueToStringConverter))]
		[SugarColumn(IsTreeKey = true)]
		[EpplusIgnore]
        public string DownloadCategoryAncestralGuid { get; set; }

        /// <summary>
        /// 描述 :名称 
        /// </summary>
        [EpplusTableColumn(Header = "名称")]
        public string DownloadCategoryName { get; set; }

        /// <summary>
        /// 描述 :排序 
        /// </summary>
        [EpplusTableColumn(Header = "排序")]
        public int DownloadCategorySort { get; set; }

        /// <summary>
        /// 描述 :审核状态 
        /// </summary>
		[EpplusIgnore]
        public int? DownloadCategoryAuditStatus { get; set; }

        /// <summary>
        /// 描述 :审核人guid 
        /// </summary>
		[JsonConverter(typeof(ValueToStringConverter))]
		[SugarColumn(IsTreeKey = true)]
		[EpplusIgnore]
        public long? DownloadCategoryAuditUserGuid { get; set; }

        public string ParentName { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [SugarColumn(IsIgnore = true)]
        public List<DownloadCategoryVoApi> Children { get; set; }
    }
	
	
	/// <summary>
    /// 下载分类详情展示对象Api
    /// </summary>
    public class DownloadCategoryApiDetailsVo
    {
		[EpplusIgnore]
        public int DownloadCategoryId { get; set; }
		[JsonConverter(typeof(ValueToStringConverter))]
		[SugarColumn(IsTreeKey = true)]
		[EpplusIgnore]
        public long DownloadCategoryGuid { get; set; }
		[JsonConverter(typeof(ValueToStringConverter))]
		[SugarColumn(IsTreeKey = true)]
        [EpplusTableColumn(Header = "父级guid")]
        public long DownloadCategoryParentGuid { get; set; }
		[JsonConverter(typeof(ValueToStringConverter))]
		[SugarColumn(IsTreeKey = true)]
		[EpplusIgnore]
        public string DownloadCategoryAncestralGuid { get; set; }
        [EpplusTableColumn(Header = "名称")]
        public string DownloadCategoryName { get; set; }
        [EpplusTableColumn(Header = "排序")]
        public int DownloadCategorySort { get; set; }
		[EpplusIgnore]
        public int? DownloadCategoryAuditStatus { get; set; }
		[JsonConverter(typeof(ValueToStringConverter))]
		[SugarColumn(IsTreeKey = true)]
		[EpplusIgnore]
        public long? DownloadCategoryAuditUserGuid { get; set; }

        public string ParentName { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [SugarColumn(IsIgnore = true)]
        public List<DownloadCategoryVoApi> Children { get; set; }
    }
	
}
