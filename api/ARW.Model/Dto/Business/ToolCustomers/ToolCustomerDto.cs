using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ARW.Model.Models.Business.ToolCustomers;

namespace ARW.Model.Dto.Business.ToolCustomers
{
    /// <summary>
    /// Tool客户输入对象
    /// </summary>
    public class ToolCustomerDto
    {
        public int ToolCustomerId { get; set; }
        public long ToolCustomerGuid { get; set; }
        [Required(ErrorMessage = "用户名不能为空")]
        public string ToolCustomerName { get; set; }
        [Required(ErrorMessage = "密码不能为空")]
        public string ToolCustomerPassword { get; set; }
        public string ToolCustomerPhoneNumber { get; set; }
        public string ToolCustomerEmail { get; set; }
    }


    /// <summary>
    /// Tool客户查询对象
    /// </summary>
    public class ToolCustomerQueryDto : PagerInfo 
    {
        public string ToolCustomerName { get; set; }
    
        public string ids { get; set; }
    }




}
