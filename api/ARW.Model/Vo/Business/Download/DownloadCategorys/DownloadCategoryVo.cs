using Newtonsoft.Json;
using OfficeOpenXml.Attributes;
using SqlSugar;
using System;
using ARW.Model.Models.Business.Download.DownloadCategorys;
using System.Collections.Generic;

namespace ARW.Model.Vo.Business.Download.DownloadCategorys
{
    /// <summary>
    /// 下载分类展示对象
    /// </summary>
    public class DownloadCategoryVo
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

        [EpplusIgnore]
        public string ParentName { get; set; }

        [EpplusIgnore]
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [SugarColumn(IsIgnore = true)]
        public List<DownloadCategoryVo> Children { get; set; }
    }
}
