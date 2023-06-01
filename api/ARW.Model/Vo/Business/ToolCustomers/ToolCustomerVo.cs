using Newtonsoft.Json;
using OfficeOpenXml.Attributes;
using SqlSugar;
using System;

namespace ARW.Model.Vo.Business.ToolCustomers
{
    /// <summary>
    /// Tool客户展示对象
    /// </summary>
    public class ToolCustomerVo
    {
		[EpplusIgnore]
        public int ToolCustomerId { get; set; }
		[JsonConverter(typeof(ValueToStringConverter))]
		[EpplusIgnore]
        public long ToolCustomerGuid { get; set; }
        [EpplusTableColumn(Header = "用户名")]
        public string ToolCustomerName { get; set; }
		[EpplusIgnore]
        public string ToolCustomerPhoneNumber { get; set; }
		[EpplusIgnore]
        public string ToolCustomerEmail { get; set; }

    }
}
