using System;
using System.Collections.Generic;
using SqlSugar;
using OfficeOpenXml.Attributes;
using Newtonsoft.Json;

namespace ARW.Model.Models.Business.Project.ProjectGroups
{
    /// <summary>
    /// 项目分组，数据实体对象
    ///
    /// @author admin
    /// @date 2023-05-19
    /// </summary>
    [SugarTable("tb_project_group")]
    public class ProjectGroup : BusinessBase
    {

        /// <summary>
        /// 描述 : 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "ProjectGroupId")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "project_group_id")]
        public int ProjectGroupId { get; set; }


        /// <summary>
        /// 描述 : 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "ProjectGroupGuid")]
		[JsonConverter(typeof(ValueToStringConverter))]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false, ColumnName = "project_group_guid")]
        public long ProjectGroupGuid { get; set; }


        /// <summary>
        /// 描述 :父级guid 
        /// 空值 : true  
        /// </summary>
        [EpplusTableColumn(Header = "父级guid")]
		[JsonConverter(typeof(ValueToStringConverter))]
        [SugarColumn(ColumnName = "project_group_parent_guid")]
        public long? ProjectGroupParentGuid { get; set; }


        /// <summary>
        /// 描述 :祖级guid 
        /// 空值 : true  
        /// </summary>
        [EpplusTableColumn(Header = "祖级guid")]
		[JsonConverter(typeof(ValueToStringConverter))]
        [SugarColumn(ColumnName = "project_group_ancestral_guid")]
        public string ProjectGroupAncestralGuid { get; set; }


        /// <summary>
        /// 描述 :客户guid 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "客户guid")]
		[JsonConverter(typeof(ValueToStringConverter))]
        [SugarColumn(ColumnName = "project_group_customer_guid")]
        public long ProjectGroupCustomerGuid { get; set; }


        /// <summary>
        /// 描述 :分组名称 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "分组名称")]
        [SugarColumn(ColumnName = "project_group_name")]
        public string ProjectGroupName { get; set; }


        /// <summary>
        /// 描述 :分组排序 
        /// 空值 : true  
        /// </summary>
        [EpplusTableColumn(Header = "分组排序")]
        [SugarColumn(ColumnName = "project_group_sort")]
        public int? ProjectGroupSort { get; set; }






		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [SugarColumn(IsIgnore = true)]
        public List<ProjectGroup> Children { get; set; }
    }
}