using System;
using System.Collections.Generic;
using SqlSugar;
using OfficeOpenXml.Attributes;
using Newtonsoft.Json;

namespace ARW.Model.Models.Business.ToolCustomers
{
    /// <summary>
    /// Tool客户，数据实体对象
    ///
    /// @author admin
    /// @date 2023-05-22
    /// </summary>
    [SugarTable("tb_tool_customer")]
    public class ToolCustomer : BusinessBase
    {

        /// <summary>
        /// 描述 : 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "ToolCustomerId")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "tool_customer_id")]
        public int ToolCustomerId { get; set; }


        /// <summary>
        /// 描述 : 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "ToolCustomerGuid")]
		[JsonConverter(typeof(ValueToStringConverter))]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false, ColumnName = "tool_customer_guid")]
        public long ToolCustomerGuid { get; set; }


        /// <summary>
        /// 描述 :用户名 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "用户名")]
        [SugarColumn(ColumnName = "tool_customer_name")]
        public string ToolCustomerName { get; set; }


        /// <summary>
        /// 描述 :密码 
        /// 空值 : false  
        /// </summary>
        [EpplusTableColumn(Header = "密码")]
        [SugarColumn(ColumnName = "tool_customer_password")]
        public string ToolCustomerPassword { get; set; }


        /// <summary>
        /// 描述 :手机号 
        /// 空值 : true  
        /// </summary>
        [EpplusTableColumn(Header = "手机号")]
        [SugarColumn(ColumnName = "tool_customer_phone_number")]
        public string ToolCustomerPhoneNumber { get; set; }


        /// <summary>
        /// 描述 :邮箱 
        /// 空值 : true  
        /// </summary>
        [EpplusTableColumn(Header = "邮箱")]
        [SugarColumn(ColumnName = "tool_customer_email")]
        public string ToolCustomerEmail { get; set; }






    }
}