using Aliyun.OSS;
using ARW.Admin.WebApi.Controllers;
using ARW.Admin.WebApi.Extensions;
using ARW.Admin.WebApi.Filters;
using ARW.Common;
using ARW.Model.Dto.Api.Project.ProjectGroups;
using ARW.Model.Dto.Business.Project.ProjectGroups;
using ARW.Model.Models.Business.Project.ProjectGroups;
using ARW.Model.Vo.Api.Project.ProjectGroups;
using ARW.Service.Api.IBusinessService.Project.ProjectGroups;
using ARW.Service.Business.IBusinessService.Project.ProjectGroups;
using Geocoding;
using Infrastructure;
using Infrastructure.Attribute;
using Infrastructure.Enums;
using Infrastructure.Model;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace ARW.WebApi.Controllers.Api.Project.ProjectGroups
{
    /// <summary>
    /// 项目分组控制器Api
    /// </summary>
    [Verify]
    [Route("api/[controller]")]
    public class ProjectGroupApiController : BaseController
    {
        private readonly IProjectGroupServiceApi _ProjectGroupServiceApi;
        private readonly IProjectGroupService _ProjectGroupService;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="ProjectGroupServiceApi">项目分组项目分组Api服务</param>
        /// <param name="projectGroupService">项目分组项目分组服务</param>
        public ProjectGroupApiController(IProjectGroupServiceApi ProjectGroupServiceApi, IProjectGroupService projectGroupService)
        {
            _ProjectGroupServiceApi = ProjectGroupServiceApi;
            _ProjectGroupService = projectGroupService;
        }


        /// <summary>
        /// 获取项目分组树形列表(Api)
        /// </summary>
        /// <param name="parm">查询参数</param>
        /// <returns></returns>
        [HttpGet("getProjectGroupTreeList")]
        public IActionResult GetProjectGroupTreeListApi([FromQuery] ProjectGroupQueryApiDto parm)
        {
            var res = _ProjectGroupServiceApi.GetProjectGroupTreeListApi(parm);
            if (res == null)
                res = new List<ProjectGroupApiVo>();

            return SUCCESS(res);
        }

        /// <summary>
        /// 获取项目分组列表(Api)
        /// </summary>
        /// <param name="parm">查询参数</param>
        /// <returns></returns>
        [HttpGet("getProjectGroupList")]
        public IActionResult GetProjectGroupListApi([FromQuery] ProjectGroupQueryApiDto parm)
        {
            var res = _ProjectGroupServiceApi.GetProjectGroupListApi(parm);
            if (res == null)
                res = new List<ProjectGroupApiVo>();

            return SUCCESS(res);
        }

        /// <summary>
        /// 添加或修改项目分组(Api)
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost("addOrUpdateProjectGroup")]
        [Log(Title = "添加或修改项目分组", BusinessType = BusinessType.ADDORUPDATE)]
        public async Task<IActionResult> AddOrUpdateProjectGroup([FromBody] ProjectGroupDto parm)
        {
            if (parm == null) { throw new CustomException("请求参数错误"); }

            var modal = new ProjectGroup();
            if (parm.ProjectGroupId != 0) modal = parm.Adapt<ProjectGroup>().ToUpdate(HttpContext);
            else modal = parm.Adapt<ProjectGroup>().ToCreate(HttpContext);

            var res = await _ProjectGroupService.AddOrUpdateProjectGroup(modal);
            return SUCCESS(res);
        }

        /// <summary>
        /// 删除项目分组(Api)
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{ids}")]
        [Log(Title = "项目分组删除", BusinessType = BusinessType.DELETE)]
        public async Task<IActionResult> Delete(string ids)
        {
            int[] idsArr = Tools.SpitIntArrary(ids);
            if (idsArr.Length <= 0) { return ToResponse(ApiResult.Error($"删除失败Id 不能为空")); }

            foreach (var item in idsArr)
            {
                var newIdsList = new List<int>();
                var nowItem = await _ProjectGroupService.GetFirstAsync(s => s.ProjectGroupId == item);
                if (nowItem == null) throw new Exception("项目分组不存在");
                newIdsList.Add(item);
                var children = await _ProjectGroupService.GetListAsync(s => s.ProjectGroupParentGuid == nowItem.ProjectGroupGuid);

                if(children.Count > 0)
                {
                    newIdsList.AddRange(children.Select(_item => _item.ProjectGroupId));
                }

                var response = _ProjectGroupService.Delete(newIdsList.ToArray());
            }

            return SUCCESS("删除成功!");
        }

        /// <summary>
        /// 获取ProjectGroup详情(Api)
        /// </summary>
        /// <param name="parm">查询参数</param>
        /// <returns></returns>
        [HttpGet("getProjectGroupDetails")]
        public async Task<IActionResult> GetProjectGroupDetails([FromQuery] ProjectGroupApiDto parm)
        {
            //if (parm == null) throw new CustomException("参数错误！");

            var res = await _ProjectGroupServiceApi.GetProjectGroupDetails(parm);

            if (res != "[]")
            {
                res = res.Remove(0, 1);
                res = res.Substring(0, res.Length - 1);
                var data = res.FromJSON<ProjectGroupApiDetailsVo>();
                return SUCCESS(data);
            }
            else
            {
                return SUCCESS(res);
            }
        }

    }
}
