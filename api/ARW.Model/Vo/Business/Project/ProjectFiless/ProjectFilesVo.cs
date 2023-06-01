using Newtonsoft.Json;
using OfficeOpenXml.Attributes;
using SqlSugar;
using System;

namespace ARW.Model.Vo.Business.Project.ProjectFiless
{
    /// <summary>
    /// 项目配置展示对象
    /// </summary>
    public class ProjectFilesVo
    {
		[EpplusIgnore]
        public int ProjectFilesId { get; set; }
		[JsonConverter(typeof(ValueToStringConverter))]
		[EpplusIgnore]
        public long ProjectFilesGuid { get; set; }
		[JsonConverter(typeof(ValueToStringConverter))]
        [EpplusTableColumn(Header = "客户guid")]
        public long CustomerGuid { get; set; }
		[JsonConverter(typeof(ValueToStringConverter))]
        [EpplusTableColumn(Header = "项目guid")]
        public long ProjectGuid { get; set; }
        [EpplusTableColumn(Header = "标题")]
        public string ProjectFilesTitle { get; set; }
        [EpplusTableColumn(Header = "打开方式类型")]
        public int ProjectFilesOpenMethodType { get; set; }
		[EpplusIgnore]
        public string ProjectFilesOpenMethodPath { get; set; }
        [EpplusTableColumn(Header = "文件打开类型")]
        public int ProjectFilesFileOpenType { get; set; }
        [EpplusTableColumn(Header = "文件打开径")]
        public string ProjectFilesFileOpenPath { get; set; }
        [EpplusTableColumn(Header = "是否需要启动Git拉取")]
        public int ProjectFilesIsGit { get; set; }
        public int ProjectFilesIsOpen { get; set; }
        [EpplusIgnore]
        public string ProjectFilesGitPath { get; set; }

    }
}
