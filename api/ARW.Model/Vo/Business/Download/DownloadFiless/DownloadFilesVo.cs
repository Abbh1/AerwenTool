using Newtonsoft.Json;
using OfficeOpenXml.Attributes;
using SqlSugar;
using System;

namespace ARW.Model.Vo.Business.Download.DownloadFiless
{
    /// <summary>
    /// 下载文件展示对象
    /// </summary>
    public class DownloadFilesVo
    {

        /// <summary>
        /// 描述 : 
        /// </summary>
		[EpplusIgnore]
        public int DownloadFilesId { get; set; }

        /// <summary>
        /// 描述 : 
        /// </summary>
		[JsonConverter(typeof(ValueToStringConverter))]
		[EpplusIgnore]
        public long DownloadFilesGuid { get; set; }

        /// <summary>
        /// 描述 :下载分类guid 
        /// </summary>
		[JsonConverter(typeof(ValueToStringConverter))]
        [EpplusTableColumn(Header = "下载分类guid")]
        public long DownloadCategoryGuid { get; set; }

        /// <summary>
        /// 描述 :图标 
        /// </summary>
        [EpplusTableColumn(Header = "图标")]
        public string DownloadFilesIcon { get; set; }

        /// <summary>
        /// 描述 :名称 
        /// </summary>
        [EpplusTableColumn(Header = "名称")]
        public string DownloadFilesName { get; set; }

        /// <summary>
        /// 描述 :简介 
        /// </summary>
		[EpplusIgnore]
        public string DownloadFilesIntro { get; set; }

        /// <summary>
        /// 描述 :下载链接 
        /// </summary>
        [EpplusTableColumn(Header = "下载链接")]
        public string DownloadFilesLink { get; set; }

        /// <summary>
        /// 描述 :附件 
        /// </summary>
		[EpplusIgnore]
        public string DownloadFilesAttachment { get; set; }

        /// <summary>
        /// 描述 :大小 
        /// </summary>
        [EpplusTableColumn(Header = "大小")]
        public decimal DownloadFilesSize { get; set; }

        /// <summary>
        /// 描述 :下载量 
        /// </summary>
        [EpplusTableColumn(Header = "下载量")]
        public int DownloadFilesVolume { get; set; }

        /// <summary>
        /// 描述 :审核状态 
        /// </summary>
        [EpplusTableColumn(Header = "审核状态")]
        public int DownloadFilesAuditStatus { get; set; }

        /// <summary>
        /// 描述 :审核人guid 
        /// </summary>
		[JsonConverter(typeof(ValueToStringConverter))]
        [EpplusTableColumn(Header = "审核人guid")]
        public long DownloadFilesAuditUserGuid { get; set; }

    }
}
