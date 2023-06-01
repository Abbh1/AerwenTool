using Infrastructure;
using Infrastructure.Attribute;
using Infrastructure.Enums;
using Infrastructure.Model;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ARW.Admin.WebApi.Extensions;
using ARW.Admin.WebApi.Filters;
using ARW.Common;
using ARW.Model.Dto.Business.ToolCustomers;
using ARW.Service.Business.IBusinessService.ToolCustomers;
using ARW.Admin.WebApi.Controllers;
using ARW.Model.Models.Business.ToolCustomers;
using ARW.Model.Vo.Business.ToolCustomers;
using Microsoft.AspNetCore.Authorization;
using ARW.Admin.WebApi.Framework;


namespace ARW.WebApi.Controllers.Business.ToolCustomers
{
    /// <summary>
    /// Tool客户控制器
    /// </summary>
    [Verify]
    [Route("business/[controller]")]
    public class ToolCustomerController : BaseController
    {
        private readonly IToolCustomerService _ToolCustomerService;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="ToolCustomerService">Tool客户Tool客户服务</param>
        public ToolCustomerController(IToolCustomerService ToolCustomerService)
        {
            _ToolCustomerService = ToolCustomerService;
        }


        /// <summary>
        /// 获取Tool客户列表
        /// </summary>
        /// <param name="parm">查询参数</param>
        /// <returns></returns>
        [HttpGet("getToolCustomerList")]
        [ActionPermissionFilter(Permission = "business:toolcustomer:list")]
        public async Task<IActionResult> GetToolCustomerList([FromQuery] ToolCustomerQueryDto parm)
        {
            var res = await _ToolCustomerService.GetToolCustomerList(parm);
            return SUCCESS(res);
        }

        /// <summary>
        /// 添加或修改Tool客户
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost("addOrUpdateToolCustomer")]
        [ActionPermissionFilter(Permission = "business:toolcustomer:addOrUpdate")]
        [Log(Title = "添加或修改Tool客户", BusinessType = BusinessType.ADDORUPDATE)]
        public async Task<IActionResult> AddOrUpdateToolCustomer([FromBody] ToolCustomerDto parm)
        {
            if (parm == null) { throw new CustomException("请求参数错误"); }

            var modal = new ToolCustomer();
            if (parm.ToolCustomerId != 0) modal = parm.Adapt<ToolCustomer>().ToUpdate(HttpContext);
            else modal = parm.Adapt<ToolCustomer>().ToCreate(HttpContext);

            var res = await _ToolCustomerService.AddOrUpdateToolCustomer(modal);
            return SUCCESS(res);
        }

        /// <summary>
        /// 删除Tool客户
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{ids}")]
        [ActionPermissionFilter(Permission = "business:toolcustomer:delete")]
        [Log(Title = "Tool客户删除", BusinessType = BusinessType.DELETE)]
        public IActionResult Delete(string ids)
        {
            long[] idsArr = Tools.SpitLongArrary(ids);
            if (idsArr.Length <= 0) { return ToResponse(ApiResult.Error($"删除失败Id 不能为空")); }
            var response = _ToolCustomerService.Delete(idsArr);
            return SUCCESS("删除成功!");
        }


        /// <summary>
        /// 导出Tool客户
        /// </summary>
        /// <returns></returns>
        [Log(Title = "Tool客户导出", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [HttpGet("exportToolCustomer")]
        [ActionPermissionFilter(Permission = "business:toolcustomer:export")]
        public async Task<IActionResult> ExportExcel([FromQuery] ToolCustomerQueryDto parm)
        {
            parm.PageSize = 10000;
            var list = await _ToolCustomerService.GetToolCustomerList(parm);
            var data = list.Result;

            // 选中数据
            if (!string.IsNullOrEmpty(parm.ids))
            {
                int[] idsArr = Tools.SpitIntArrary(parm.ids);
                var selectDataList = new List<ToolCustomerVo>();
                foreach (var item in idsArr)
                {
                    var select_data = data.Where(s => s.ToolCustomerId == item).First();
                    selectDataList.Add(select_data);
                }
                data = selectDataList;
            }



            // 导出数据处理
            var handleData = await _ToolCustomerService.HandleExportData(data);

            string sFileName = ExportExcel(handleData, "ToolCustomer", "Tool客户列表");
            return SUCCESS(new { path = "/export/" + sFileName, fileName = sFileName });
        }




    }
}
