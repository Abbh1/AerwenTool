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
using ARW.Model.Dto.Business.CreateTable.BaseFiledTemplates;
using ARW.Service.Business.IBusinessService.CreateTable.BaseFiledTemplates;
using ARW.Admin.WebApi.Controllers;
using ARW.Model.Models.Business.CreateTable.BaseFiledTemplates;
using ARW.Model.Vo.Business.CreateTable.BaseFiledTemplates;
using Microsoft.AspNetCore.Authorization;
using ARW.Admin.WebApi.Framework;


namespace ARW.WebApi.Controllers.Business.CreateTable.BaseFiledTemplates
{
    /// <summary>
    /// 基础字段模板控制器
    /// </summary>
    [Verify]
    [Route("business/[controller]")]
    public class BaseFiledTemplateController : BaseController
    {
        private readonly IBaseFiledTemplateService _BaseFiledTemplateService;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="BaseFiledTemplateService">基础字段模板基础字段模板服务</param>
        public BaseFiledTemplateController(IBaseFiledTemplateService BaseFiledTemplateService)
        {
            _BaseFiledTemplateService = BaseFiledTemplateService;
        }


        /// <summary>
        /// 获取基础字段模板列表
        /// </summary>
        /// <param name="parm">查询参数</param>
        /// <returns></returns>
        [HttpGet("getBaseFiledTemplateList")]
        [ActionPermissionFilter(Permission = "business:basefiledtemplate:list")]
        public async Task<IActionResult> GetBaseFiledTemplateList([FromQuery] BaseFiledTemplateQueryDto parm)
        {
            var res = await _BaseFiledTemplateService.GetBaseFiledTemplateList(parm);
            return SUCCESS(res);
        }

        /// <summary>
        /// 添加或修改基础字段模板
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost("addOrUpdateBaseFiledTemplate")]
        [ActionPermissionFilter(Permission = "business:basefiledtemplate:addOrUpdate")]
        [Log(Title = "添加或修改基础字段模板", BusinessType = BusinessType.ADDORUPDATE)]
        public async Task<IActionResult> AddOrUpdateBaseFiledTemplate([FromBody] BaseFiledTemplateDto parm)
        {
            if (parm == null) { throw new CustomException("请求参数错误"); }

            var modal = new BaseFiledTemplate();
            if (parm.BaseFiledTemplateId != 0) modal = parm.Adapt<BaseFiledTemplate>().ToUpdate(HttpContext);
            else modal = parm.Adapt<BaseFiledTemplate>().ToCreate(HttpContext);

            var res = await _BaseFiledTemplateService.AddOrUpdateBaseFiledTemplate(modal);
            return SUCCESS(res);
        }

        /// <summary>
        /// 删除基础字段模板
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{ids}")]
        [ActionPermissionFilter(Permission = "business:basefiledtemplate:delete")]
        [Log(Title = "基础字段模板删除", BusinessType = BusinessType.DELETE)]
        public IActionResult Delete(string ids)
        {
            long[] idsArr = Tools.SpitLongArrary(ids);
            if (idsArr.Length <= 0) { return ToResponse(ApiResult.Error($"删除失败Id 不能为空")); }
            var response = _BaseFiledTemplateService.Delete(idsArr);
            return SUCCESS("删除成功!");
        }


        /// <summary>
        /// 导出基础字段模板
        /// </summary>
        /// <returns></returns>
        [Log(Title = "基础字段模板导出", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [HttpGet("exportBaseFiledTemplate")]
        [ActionPermissionFilter(Permission = "business:basefiledtemplate:export")]
        public async Task<IActionResult> ExportExcel([FromQuery] BaseFiledTemplateQueryDto parm)
        {
            parm.PageSize = 10000;
            var list = await _BaseFiledTemplateService.GetBaseFiledTemplateList(parm);
            var data = list.Result;

            // 选中数据
            if (!string.IsNullOrEmpty(parm.ids))
            {
                int[] idsArr = Tools.SpitIntArrary(parm.ids);
                var selectDataList = new List<BaseFiledTemplateVo>();
                foreach (var item in idsArr)
                {
                    var select_data = data.Where(s => s.BaseFiledTemplateId == item).First();
                    selectDataList.Add(select_data);
                }
                data = selectDataList;
            }



            // 导出数据处理
            var handleData = await _BaseFiledTemplateService.HandleExportData(data);

            string sFileName = ExportExcel(handleData, "BaseFiledTemplate", "基础字段模板列表");
            return SUCCESS(new { path = "/export/" + sFileName, fileName = sFileName });
        }




    }
}
