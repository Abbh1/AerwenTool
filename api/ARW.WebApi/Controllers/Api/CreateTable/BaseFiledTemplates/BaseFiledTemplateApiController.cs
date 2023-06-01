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
using ARW.Admin.WebApi.Controllers;
using ARW.Model.Dto.Api.CreateTable.BaseFiledTemplates;
using ARW.Service.Api.IBusinessService.CreateTable.BaseFiledTemplates;
using ARW.Model.Models.Business.CreateTable.BaseFiledTemplates;
using ARW.Model.Vo.Api.CreateTable.BaseFiledTemplates;
using Microsoft.AspNetCore.Authorization;
using Geocoding;
using ARW.Model.Dto.Business.CreateTable.BaseFiledTemplates;
using ARW.Service.Business.IBusinessService.CreateTable.BaseFiledTemplates;

namespace ARW.WebApi.Controllers.Api.CreateTable.BaseFiledTemplates
{
    /// <summary>
    /// 基础字段模板控制器Api
    /// </summary>
    [Verify]
    [Route("api/[controller]")]
    public class BaseFiledTemplateApiController : BaseController
    {
        private readonly IBaseFiledTemplateServiceApi _BaseFiledTemplateServiceApi;
        private readonly IBaseFiledTemplateService _BaseFiledTemplateService;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="BaseFiledTemplateServiceApi">基础字段模板基础字段模板Api服务</param>
        /// <param name="baseFiledTemplateService">基础字段模板基础字段模板服务</param>
        public BaseFiledTemplateApiController(IBaseFiledTemplateServiceApi BaseFiledTemplateServiceApi, IBaseFiledTemplateService baseFiledTemplateService)
        {
            _BaseFiledTemplateServiceApi = BaseFiledTemplateServiceApi;
            _BaseFiledTemplateService = baseFiledTemplateService;
        }


        /// <summary>
        /// 获取基础字段模板列表(Api)
        /// </summary>
        /// <param name="parm">查询参数</param>
        /// <returns></returns>
        [HttpGet("getBaseFiledTemplateList")]
        public async Task<IActionResult> GetBaseFiledTemplateListApi([FromQuery] BaseFiledTemplateQueryApiDto parm)
        {
            var res = await _BaseFiledTemplateServiceApi.GetBaseFiledTemplateListApi(parm);
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
        [Log(Title = "基础字段模板删除", BusinessType = BusinessType.DELETE)]
        public IActionResult Delete(string ids)
        {
            long[] idsArr = Tools.SpitLongArrary(ids);
            if (idsArr.Length <= 0) { return ToResponse(ApiResult.Error($"删除失败Id 不能为空")); }
            var response = _BaseFiledTemplateService.Delete(idsArr);
            return SUCCESS("删除成功!");
        }

        /// <summary>
        /// 获取BaseFiledTemplate详情(Api)
        /// </summary>
        /// <param name="parm">查询参数</param>
        /// <returns></returns>
        [HttpGet("getBaseFiledTemplateDetails")]
        public async Task<IActionResult> GetBaseFiledTemplateDetails([FromQuery] BaseFiledTemplateApiDto parm)
        {
            //if (parm == null) throw new CustomException("参数错误！");

            var res = await _BaseFiledTemplateServiceApi.GetBaseFiledTemplateDetails(parm);

            if (res != "[]")
            {	
                res = res.Remove(0, 1);
                res = res.Substring(0, res.Length - 1);
                var data = res.FromJSON<BaseFiledTemplateApiDetailsVo>();
                return SUCCESS(data);
            }
            else
            {
                return SUCCESS(res);
            }
        }

    }
}
