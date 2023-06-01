using System;
using System.Collections.Generic;
using SqlSugar;
using OfficeOpenXml.Attributes;
using Newtonsoft.Json;

namespace ARW.Model.Models.Business.Project.ProjectFiless
{
    /// <summary>
    /// 项目配置，数据实体对象
    ///
    /// @author admin
    /// @date 2023-05-23
    /// </summary>
    [SugarTable("tb_project_files")]
    public class ProjectFiles : BusinessBase
    {

        /// <summary>
        /// 描述 : 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "ProjectFilesId")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "project_files_id")]
        public int ProjectFilesId { get; set; }


        /// <summary>
        /// 描述 : 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "ProjectFilesGuid")]
		[JsonConverter(typeof(ValueToStringConverter))]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false, ColumnName = "project_files_guid")]
        public long ProjectFilesGuid { get; set; }


        /// <summary>
        /// 描述 :客户guid 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "客户guid")]
		[JsonConverter(typeof(ValueToStringConverter))]
        [SugarColumn(ColumnName = "customer_guid")]
        public long CustomerGuid { get; set; }


        /// <summary>
        /// 描述 :项目guid 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "项目guid")]
		[JsonConverter(typeof(ValueToStringConverter))]
        [SugarColumn(ColumnName = "project_guid")]
        public long ProjectGuid { get; set; }


        /// <summary>
        /// 描述 :标题 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "标题")]
        [SugarColumn(ColumnName = "project_files_title")]
        public string ProjectFilesTitle { get; set; }


        /// <summary>
        /// 描述 :打开方式类型 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "打开方式类型")]
        [SugarColumn(ColumnName = "project_files_open_method_type")]
        public int ProjectFilesOpenMethodType { get; set; }


        /// <summary>
        /// 描述 :打开方式路径 
        /// 空值 : true  
        /// </summary>
        [EpplusTableColumn(Header = "打开方式路径")]
        [SugarColumn(ColumnName = "project_files_open_method_path")]
        public string ProjectFilesOpenMethodPath { get; set; }


        /// <summary>
        /// 描述 :文件打开类型 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "文件打开类型")]
        [SugarColumn(ColumnName = "project_files_file_open_type")]
        public int ProjectFilesFileOpenType { get; set; }


        /// <summary>
        /// 描述 :文件打开径 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "文件打开径")]
        [SugarColumn(ColumnName = "project_files_file_open_path")]
        public string ProjectFilesFileOpenPath { get; set; }

        /// <summary>
        /// 描述 :是否需要打开 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "是否需要打开")]
        [SugarColumn(ColumnName = "project_files_is_open")]
        public int ProjectFilesIsOpen { get; set; }

        /// <summary>
        /// 描述 :是否需要启动Git拉取 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "是否需要启动Git拉取")]
        [SugarColumn(ColumnName = "project_files_is_git")]
        public int ProjectFilesIsGit { get; set; }


        /// <summary>
        /// 描述 :Git项目仓库路径 
        /// 空值 : true  
        /// </summary>
        [EpplusTableColumn(Header = "Git项目仓库路径")]
        [SugarColumn(ColumnName = "project_files_git_path")]
        public string ProjectFilesGitPath { get; set; }






    }
}