using System;
using Newtonsoft.Json;
using SqlSugar;
using OfficeOpenXml.Attributes;

namespace ARW.Model.Models.Business
{

    [EpplusTable(PrintHeaders = true, AutofitColumns = true, AutoCalculate = true, ShowTotal = true)]
    public class BusinessBase
    {
        [SugarColumn(IsOnlyIgnoreUpdate = true, IsNullable = true, ColumnName = "create_by", ColumnDescription = "创建者")]//设置后修改不会有此字段
        [JsonProperty(propertyName: "CreateBy")]
        [EpplusIgnore]
        public string Create_by { get; set; }

        [SugarColumn(IsOnlyIgnoreUpdate = true, IsNullable = true, ColumnName = "create_time", ColumnDescription = "创建时间")]//设置后修改不会有此字段
        [JsonProperty(propertyName: "CreateTime")]
        [EpplusTableColumn(NumberFormat = "yyyy-MM-dd HH:mm:ss")]
        public DateTime Create_time { get; set; } = DateTime.Now;

        [JsonIgnore]
        [JsonProperty(propertyName: "UpdateBy")]
        [SugarColumn(IsOnlyIgnoreInsert = true, IsNullable = true, ColumnName = "update_by", ColumnDescription = "更新者")]
        [EpplusIgnore]
        public string Update_by { get; set; }

        //[JsonIgnore]
        [SugarColumn(IsOnlyIgnoreInsert = true, IsNullable = true, ColumnName = "update_time", ColumnDescription = "更新时间")]//设置后插入数据不会有此字段
        [JsonProperty(propertyName: "UpdateTime")]
        [EpplusIgnore]
        public DateTime? Update_time { get; set; }

        [SugarColumn(IsIgnore = true)]
        [JsonIgnore]
        [EpplusIgnore]
        public DateTime? BeginTime { get; set; }

        /// <summary>
        /// 用于搜索使用
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [JsonIgnore]
        [EpplusIgnore]
        public DateTime? EndTime { get; set; }

        [JsonProperty(propertyName: "IsDelete")]
        [SugarColumn(IsNullable = true, ColumnDescription = "是否删除")]
        [EpplusIgnore]
        public bool IsDelete { get; set; }
    }
}
