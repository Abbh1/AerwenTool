using Newtonsoft.Json;
using OfficeOpenXml.Attributes;
using SqlSugar;
using System;
using ARW.Model.Models.Business.Project.ProjectGroups;
using System.Collections.Generic;

namespace ARW.Model.Vo.Business.Project.ProjectGroups
{
    /// <summary>
    /// 项目分组展示对象
    /// </summary>
    public class ProjectGroupVo
    {
		[EpplusIgnore]
        public int ProjectGroupId { get; set; }
		[JsonConverter(typeof(ValueToStringConverter))]
		[SugarColumn(IsTreeKey = true)]
		[EpplusIgnore]
        public long ProjectGroupGuid { get; set; }
		[JsonConverter(typeof(ValueToStringConverter))]
		[SugarColumn(IsTreeKey = true)]
		[EpplusIgnore]
        public long? ProjectGroupParentGuid { get; set; }
		[JsonConverter(typeof(ValueToStringConverter))]
		[SugarColumn(IsTreeKey = true)]
		[EpplusIgnore]
        public string ProjectGroupAncestralGuid { get; set; }
		[JsonConverter(typeof(ValueToStringConverter))]
		[SugarColumn(IsTreeKey = true)]
        [EpplusTableColumn(Header = "客户guid")]
        public long ProjectGroupCustomerGuid { get; set; }
        [EpplusTableColumn(Header = "分组名称")]
        public string ProjectGroupName { get; set; }
		[EpplusIgnore]
        public int? ProjectGroupSort { get; set; }

        [EpplusIgnore]
        public string ParentName { get; set; }

        [EpplusIgnore]
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [SugarColumn(IsIgnore = true)]
        public List<ProjectGroupVo> Children { get; set; }
    }
}
