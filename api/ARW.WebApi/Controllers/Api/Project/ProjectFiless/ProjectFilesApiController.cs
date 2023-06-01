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
using ARW.Model.Dto.Api.Project.ProjectFiless;
using ARW.Service.Api.IBusinessService.Project.ProjectFiless;
using ARW.Model.Models.Business.Project.ProjectFiless;
using ARW.Model.Vo.Api.Project.ProjectFiless;
using Microsoft.AspNetCore.Authorization;
using Geocoding;
using ARW.Service.Business.IBusinessService.Project.ProjectFiless;
using ARW.Model.Dto.Business.Project.ProjectFiless;

namespace ARW.WebApi.Controllers.Api.Project.ProjectFiless
{
    /// <summary>
    /// 项目配置控制器Api
    /// </summary>
    [Verify]
    [Route("api/[controller]")]
    public class ProjectFilesApiController : BaseController
    {
        private readonly IProjectFilesServiceApi _ProjectFilesServiceApi;
        private readonly IProjectFilesService _ProjectFilesService;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="ProjectFilesServiceApi">项目配置项目配置Api服务</param>
        /// <param name="projectFilesService">项目配置项目配置文件服务</param>
        public ProjectFilesApiController(IProjectFilesServiceApi ProjectFilesServiceApi, IProjectFilesService projectFilesService)
        {
            _ProjectFilesServiceApi = ProjectFilesServiceApi;
            _ProjectFilesService = projectFilesService;
        }


        /// <summary>
        /// 获取项目配置列表(Api)
        /// </summary>
        /// <param name="parm">查询参数</param>
        /// <returns></returns>
        [HttpGet("getProjectFilesList")]
        public async Task<IActionResult> GetProjectFilesListApi([FromQuery] ProjectFilesQueryApiDto parm)
        {
            var res = await _ProjectFilesServiceApi.GetProjectFilesListApi(parm);
            return SUCCESS(res);
        }


        /// <summary>
        /// 添加或修改项目配置
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost("addOrUpdateProjectFiles")]
        [Log(Title = "添加或修改项目配置", BusinessType = BusinessType.ADDORUPDATE)]
        public async Task<IActionResult> AddOrUpdateProjectFiles([FromBody] ProjectFilesDto parm)
        {
            if (parm == null) { throw new CustomException("请求参数错误"); }

            var modal = new ProjectFiles();
            if (parm.ProjectFilesId != 0) modal = parm.Adapt<ProjectFiles>().ToUpdate(HttpContext);
            else modal = parm.Adapt<ProjectFiles>().ToCreate(HttpContext);

            var res = await _ProjectFilesService.AddOrUpdateProjectFiles(modal);
            return SUCCESSApi(res);
        }

        /// <summary>
        /// 删除项目配置
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{ids}")]
        [Log(Title = "项目配置删除", BusinessType = BusinessType.DELETE)]
        public IActionResult Delete(string ids)
        {
            long[] idsArr = Tools.SpitLongArrary(ids);
            if (idsArr.Length <= 0) { return ToResponse(ApiResult.Error($"删除失败Id 不能为空")); }
            var response = _ProjectFilesService.Delete(idsArr);
            return SUCCESS("删除成功!");
        }


        /// <summary>
        /// 获取ProjectFiles详情(Api)
        /// </summary>
        /// <param name="parm">查询参数</param>
        /// <returns></returns>
        [HttpGet("getProjectFilesDetails")]
        public async Task<IActionResult> GetProjectFilesDetails([FromQuery] ProjectFilesApiDto parm)
        {
            //if (parm == null) throw new CustomException("参数错误！");

            var res = await _ProjectFilesServiceApi.GetProjectFilesDetails(parm);

            if (res != "[]")
            {
                res = res.Remove(0, 1);
                res = res.Substring(0, res.Length - 1);
                var data = res.FromJSON<ProjectFilesApiDetailsVo>();
                return SUCCESS(data);
            }
            else
            {
                return SUCCESS(res);
            }
        }

    }
}
