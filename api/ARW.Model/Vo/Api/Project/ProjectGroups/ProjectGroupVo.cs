using Newtonsoft.Json;
using OfficeOpenXml.Attributes;
using SqlSugar;
using System;
using ARW.Model.Models.Business.Project.ProjectGroups;
using System.Collections.Generic;
using ARW.Model.Vo.Business.Project.ProjectGroups;

namespace ARW.Model.Vo.Api.Project.ProjectGroups
{
    /// <summary>
    /// 项目分组展示对象
    /// </summary>
    public class ProjectGroupApiVo
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

        public string ParentName { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [SugarColumn(IsIgnore = true)]
        public List<ProjectGroupApiVo> Children { get; set; }
    }
	
	
	/// <summary>
    /// 项目分组详情展示对象Api
    /// </summary>
    public class ProjectGroupApiDetailsVo
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

        public string ParentName { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [SugarColumn(IsIgnore = true)]
        public List<ProjectGroupVo> Children { get; set; }
    }
	
}
