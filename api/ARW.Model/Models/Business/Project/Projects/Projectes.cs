using System;
using System.Collections.Generic;
using SqlSugar;
using OfficeOpenXml.Attributes;
using Newtonsoft.Json;

namespace ARW.Model.Models.Business.Project.Projects
{
    /// <summary>
    /// 项目，数据实体对象
    ///
    /// @author admin
    /// @date 2023-05-19
    /// </summary>
    [SugarTable("tb_project")]
    public class Projectes : BusinessBase
    {

        /// <summary>
        /// 描述 : 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "ProjectId")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "project_id")]
        public int ProjectId { get; set; }


        /// <summary>
        /// 描述 : 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "ProjectGuid")]
		[JsonConverter(typeof(ValueToStringConverter))]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false, ColumnName = "project_guid")]
        public long ProjectGuid { get; set; }


        /// <summary>
        /// 描述 :客户guid 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "客户guid")]
		[JsonConverter(typeof(ValueToStringConverter))]
        [SugarColumn(ColumnName = "project_customer_guid")]
        public long ProjectCustomerGuid { get; set; }


        /// <summary>
        /// 描述 :项目分组guid 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "项目分组guid")]
		[JsonConverter(typeof(ValueToStringConverter))]
        [SugarColumn(ColumnName = "project_group_guid")]
        public long ProjectGroupGuid { get; set; }


        /// <summary>
        /// 描述 :项目名称 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "项目名称")]
        [SugarColumn(ColumnName = "project_name")]
        public string ProjectName { get; set; }


        /// <summary>
        /// 描述 :项目图片 
        /// 空值 : true  
        /// </summary>
        [EpplusTableColumn(Header = "项目图片")]
        [SugarColumn(ColumnName = "project_img")]
        public string ProjectImg { get; set; }


        /// <summary>
        /// 描述 :项目简介 
        /// 空值 : true  
        /// </summary>
        [EpplusTableColumn(Header = "项目简介")]
        [SugarColumn(ColumnName = "project_intro")]
        public string ProjectIntro { get; set; }


        /// <summary>
        /// 描述 :项目排序 
        /// 空值 : true  
        /// </summary>
        [EpplusTableColumn(Header = "项目排序")]
        [SugarColumn(ColumnName = "project_sort")]
        public int? ProjectSort { get; set; }






    }
}