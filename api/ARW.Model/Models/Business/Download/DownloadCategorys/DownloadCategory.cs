using System;
using System.Collections.Generic;
using SqlSugar;
using OfficeOpenXml.Attributes;
using Newtonsoft.Json;

namespace ARW.Model.Models.Business.Download.DownloadCategorys
{
    /// <summary>
    /// 下载分类，数据实体对象
    ///
    /// @author admin
    /// @date 2023-05-25
    /// </summary>
    [SugarTable("tb_download_category")]
    public class DownloadCategory : BusinessBase
    {

        /// <summary>
        /// 描述 : 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "DownloadCategoryId")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "download_category_id")]
        public int DownloadCategoryId { get; set; }


        /// <summary>
        /// 描述 : 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "DownloadCategoryGuid")]
		[JsonConverter(typeof(ValueToStringConverter))]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false, ColumnName = "download_category_guid")]
        public long DownloadCategoryGuid { get; set; }


        /// <summary>
        /// 描述 :父级guid 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "父级guid")]
		[JsonConverter(typeof(ValueToStringConverter))]
        [SugarColumn(ColumnName = "download_category_parent_guid")]
        public long DownloadCategoryParentGuid { get; set; }


        /// <summary>
        /// 描述 :祖级guid 
        /// 空值 : true  
        /// </summary>
        [EpplusTableColumn(Header = "祖级guid")]
		[JsonConverter(typeof(ValueToStringConverter))]
        [SugarColumn(ColumnName = "download_category_ancestral_guid")]
        public string DownloadCategoryAncestralGuid { get; set; }


        /// <summary>
        /// 描述 :名称 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "名称")]
        [SugarColumn(ColumnName = "download_category_name")]
        public string DownloadCategoryName { get; set; }


        /// <summary>
        /// 描述 :排序 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "排序")]
        [SugarColumn(ColumnName = "download_category_sort")]
        public int DownloadCategorySort { get; set; }


        /// <summary>
        /// 描述 :审核状态 
        /// 空值 : true  
        /// </summary>
        [EpplusTableColumn(Header = "审核状态")]
        [SugarColumn(ColumnName = "download_category_audit_status")]
        public int? DownloadCategoryAuditStatus { get; set; }


        /// <summary>
        /// 描述 :审核人guid 
        /// 空值 : true  
        /// </summary>
        [EpplusTableColumn(Header = "审核人guid")]
		[JsonConverter(typeof(ValueToStringConverter))]
        [SugarColumn(ColumnName = "download_category_audit_user_guid")]
        public long? DownloadCategoryAuditUserGuid { get; set; }






		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [SugarColumn(IsIgnore = true)]
        public List<DownloadCategory> Children { get; set; }
    }
}