using Newtonsoft.Json;
using OfficeOpenXml.Attributes;
using SqlSugar;
using System;

namespace ARW.Model.Vo.Business.Project.Projects
{
    /// <summary>
    /// 项目展示对象
    /// </summary>
    public class ProjectesVo
    {
		[EpplusIgnore]
        public int ProjectId { get; set; }
		[JsonConverter(typeof(ValueToStringConverter))]
		[EpplusIgnore]
        public long ProjectGuid { get; set; }
		[JsonConverter(typeof(ValueToStringConverter))]
        [EpplusTableColumn(Header = "客户guid")]
        public long ProjectCustomerGuid { get; set; }
		[JsonConverter(typeof(ValueToStringConverter))]
        [EpplusTableColumn(Header = "项目分组guid")]
        public long ProjectGroupGuid { get; set; }
        [EpplusTableColumn(Header = "项目名称")]
        public string ProjectName { get; set; }
		[EpplusIgnore]
        public string ProjectImg { get; set; }
		[EpplusIgnore]
        public string ProjectIntro { get; set; }
		[EpplusIgnore]
        public int? ProjectSort { get; set; }

        [EpplusIgnore]
        public string CustomerName { get; set; }

        [EpplusIgnore]
        public string ProjectGroupName { get; set; }

    }
}
