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
using ARW.Model.Dto.Api.Project.Projects;
using ARW.Service.Api.IBusinessService.Project.Projects;
using ARW.Model.Models.Business.Project.Projects;
using ARW.Model.Vo.Api.Project.Projects;
using Microsoft.AspNetCore.Authorization;
using Geocoding;
using ARW.Service.Business.IBusinessService.Project.Projects;
using ARW.Model.Dto.Business.Project.Projects;
using ARW.Service.Api.IBusinessService.Project.ProjectFiless;

namespace ARW.WebApi.Controllers.Api.Project.Projects
{
    /// <summary>
    /// 项目控制器Api
    /// </summary>
    [Verify]
    [Route("api/[controller]")]
    public class ProjectesApiController : BaseController
    {
        private readonly IProjectesServiceApi _ProjectesServiceApi;
        private readonly IProjectesService _ProjectesService;
        private readonly IProjectFilesServiceApi _ProjectFilesServiceApi;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="ProjectesServiceApi">项目项目Api服务</param>
        /// <param name="projectesService">项目项目服务</param>
        /// <param name="projectFilesServiceApi">项目配置文件服务</param>
        public ProjectesApiController(IProjectesServiceApi ProjectesServiceApi, IProjectesService projectesService, IProjectFilesServiceApi projectFilesServiceApi)
        {
            _ProjectesServiceApi = ProjectesServiceApi;
            _ProjectesService = projectesService;
            _ProjectFilesServiceApi = projectFilesServiceApi;
        }


        /// <summary>
        /// 获取项目列表(Api)
        /// </summary>
        /// <param name="parm">查询参数</param>
        /// <returns></returns>
        [HttpGet("getProjectesList")]
        public async Task<IActionResult> GetProjectesListApi([FromQuery] ProjectesQueryApiDto parm)
        {
            var res = await _ProjectesServiceApi.GetProjectesListApi(parm);
            return SUCCESS(res);
        }


        /// <summary>
        /// 添加或修改项目
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost("addOrUpdateProjectes")]
        [Log(Title = "添加或修改项目", BusinessType = BusinessType.ADDORUPDATE)]
        public async Task<IActionResult> AddOrUpdateProjectes([FromBody] ProjectesDto parm)
        {
            if (parm == null) { throw new CustomException("请求参数错误"); }

            var modal = new Projectes();
            if (parm.ProjectId != 0) modal = parm.Adapt<Projectes>().ToUpdate(HttpContext);
            else modal = parm.Adapt<Projectes>().ToCreate(HttpContext);

            var res = await _ProjectesService.AddOrUpdateProjectes(modal);
            return SUCCESS(res);
        }

        /// <summary>
        /// 删除项目
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{ids}")]
        [Log(Title = "项目删除", BusinessType = BusinessType.DELETE)]
        public IActionResult Delete(string ids)
        {
            long[] idsArr = Tools.SpitLongArrary(ids);
            if (idsArr.Length <= 0) { return ToResponse(ApiResult.Error($"删除失败Id 不能为空")); }
            var response = _ProjectesService.Delete(idsArr);
            return SUCCESS("删除成功!");
        }

        /// <summary>
        /// 获取项目的配置列表(Api)
        /// </summary>
        /// <param name="parm">查询参数</param>
        /// <returns></returns>
        [HttpGet("getProjectesOfFilesList")]
        public async Task<IActionResult> GetProjectesOfFilesListApi([FromQuery] ProjectesQueryApiDto parm)
        {
            var res = await _ProjectFilesServiceApi.GetListAsync(s => s.CustomerGuid == parm.ToolCustomerGuid && s.ProjectGuid == parm.ProjectGuid);
            return SUCCESS(res);
        }



        /// <summary>
        /// 获取Projectes详情(Api)
        /// </summary>
        /// <param name="parm">查询参数</param>
        /// <returns></returns>
        [HttpGet("getProjectesDetails")]
        public async Task<IActionResult> GetProjectesDetails([FromQuery] ProjectesApiDto parm)
        {
            //if (parm == null) throw new CustomException("参数错误！");

            var res = await _ProjectesServiceApi.GetProjectesDetails(parm);

            if (res != "[]")
            {
                res = res.Remove(0, 1);
                res = res.Substring(0, res.Length - 1);
                var data = res.FromJSON<ProjectesApiDetailsVo>();
                return SUCCESS(data);
            }
            else
            {
                return SUCCESS(res);
            }
        }

    }
}
