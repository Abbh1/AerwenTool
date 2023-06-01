using System;
using System.Collections.Generic;
using SqlSugar;
using OfficeOpenXml.Attributes;
using Newtonsoft.Json;

namespace ARW.Model.Models.Business.Download.DownloadFiless
{
    /// <summary>
    /// 下载文件，数据实体对象
    ///
    /// @author admin
    /// @date 2023-05-28
    /// </summary>
    [SugarTable("tb_download_files")]
    public class DownloadFiles : BusinessBase
    {

        /// <summary>
        /// 描述 : 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "DownloadFilesId")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "download_files_id")]
        public int DownloadFilesId { get; set; }


        /// <summary>
        /// 描述 : 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "DownloadFilesGuid")]
		[JsonConverter(typeof(ValueToStringConverter))]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false, ColumnName = "download_files_guid")]
        public long DownloadFilesGuid { get; set; }


        /// <summary>
        /// 描述 :下载分类guid 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "下载分类guid")]
		[JsonConverter(typeof(ValueToStringConverter))]
        [SugarColumn(ColumnName = "download_category_guid")]
        public long DownloadCategoryGuid { get; set; }


        /// <summary>
        /// 描述 :图标 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "图标")]
        [SugarColumn(ColumnName = "download_files_icon")]
        public string DownloadFilesIcon { get; set; }


        /// <summary>
        /// 描述 :名称 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "名称")]
        [SugarColumn(ColumnName = "download_files_name")]
        public string DownloadFilesName { get; set; }


        /// <summary>
        /// 描述 :简介 
        /// 空值 : true  
        /// </summary>
        [EpplusTableColumn(Header = "简介")]
        [SugarColumn(ColumnName = "download_files_intro")]
        public string DownloadFilesIntro { get; set; }


        /// <summary>
        /// 描述 :下载链接 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "下载链接")]
        [SugarColumn(ColumnName = "download_files_link")]
        public string DownloadFilesLink { get; set; }


        /// <summary>
        /// 描述 :附件 
        /// 空值 : true  
        /// </summary>
        [EpplusTableColumn(Header = "附件")]
        [SugarColumn(ColumnName = "download_files_attachment")]
        public string DownloadFilesAttachment { get; set; }


        /// <summary>
        /// 描述 :大小 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "大小")]
        [SugarColumn(ColumnName = "download_files_size")]
        public decimal DownloadFilesSize { get; set; }


        /// <summary>
        /// 描述 :下载量 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "下载量")]
        [SugarColumn(ColumnName = "download_files_volume")]
        public int DownloadFilesVolume { get; set; }


        /// <summary>
        /// 描述 :审核状态 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "审核状态")]
        [SugarColumn(ColumnName = "download_files_audit_status")]
        public int DownloadFilesAuditStatus { get; set; }


        /// <summary>
        /// 描述 :审核人guid 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "审核人guid")]
		[JsonConverter(typeof(ValueToStringConverter))]
        [SugarColumn(ColumnName = "download_files_audit_user_guid")]
        public long DownloadFilesAuditUserGuid { get; set; }






    }
}