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
using ARW.Model.Dto.Business.Project.ProjectFiless;
using ARW.Service.Business.IBusinessService.Project.ProjectFiless;
using ARW.Admin.WebApi.Controllers;
using ARW.Model.Models.Business.Project.ProjectFiless;
using ARW.Model.Vo.Business.Project.ProjectFiless;
using Microsoft.AspNetCore.Authorization;
using ARW.Admin.WebApi.Framework;


namespace ARW.WebApi.Controllers.Business.Project.ProjectFiless
{
    /// <summary>
    /// 项目配置控制器
    /// </summary>
    [Verify]
    [Route("business/[controller]")]
    public class ProjectFilesController : BaseController
    {
        private readonly IProjectFilesService _ProjectFilesService;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="ProjectFilesService">项目配置项目配置服务</param>
        public ProjectFilesController(IProjectFilesService ProjectFilesService)
        {
            _ProjectFilesService = ProjectFilesService;
        }


        /// <summary>
        /// 获取项目配置列表
        /// </summary>
        /// <param name="parm">查询参数</param>
        /// <returns></returns>
        [HttpGet("getProjectFilesList")]
        [ActionPermissionFilter(Permission = "business:projectfiles:list")]
        public async Task<IActionResult> GetProjectFilesList([FromQuery] ProjectFilesQueryDto parm)
        {
            var res = await _ProjectFilesService.GetProjectFilesList(parm);
            return SUCCESS(res);
        }

        /// <summary>
        /// 添加或修改项目配置
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost("addOrUpdateProjectFiles")]
        [ActionPermissionFilter(Permission = "business:projectfiles:addOrUpdate")]
        [Log(Title = "添加或修改项目配置", BusinessType = BusinessType.ADDORUPDATE)]
        public async Task<IActionResult> AddOrUpdateProjectFiles([FromBody] ProjectFilesDto parm)
        {
            if (parm == null) { throw new CustomException("请求参数错误"); }

	var modal = new ProjectFiles();
            if (parm.ProjectFilesId != 0) modal = parm.Adapt<ProjectFiles>().ToUpdate(HttpContext);
            else modal = parm.Adapt<ProjectFiles>().ToCreate(HttpContext);

            var res = await _ProjectFilesService.AddOrUpdateProjectFiles(modal);
            return SUCCESS(res);
        }

        /// <summary>
        /// 删除项目配置
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{ids}")]
        [ActionPermissionFilter(Permission = "business:projectfiles:delete")]
        [Log(Title = "项目配置删除", BusinessType = BusinessType.DELETE)]
        public IActionResult Delete(string ids)
        {
            long[] idsArr = Tools.SpitLongArrary(ids);
            if (idsArr.Length <= 0) { return ToResponse(ApiResult.Error($"删除失败Id 不能为空")); }
            var response = _ProjectFilesService.Delete(idsArr);
            return SUCCESS("删除成功!");
        }
		
		
		/// <summary>
        /// 导出项目配置
        /// </summary>
        /// <returns></returns>
        [Log(Title = "项目配置导出", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [HttpGet("exportProjectFiles")]
        [ActionPermissionFilter(Permission = "business:projectfiles:export")]
        public async Task<IActionResult> ExportExcel([FromQuery] ProjectFilesQueryDto parm)                  
        {
            parm.PageSize = 10000;
            var list = await _ProjectFilesService.GetProjectFilesList(parm);
            var data = list.Result;

            // 选中数据
            if (!string.IsNullOrEmpty(parm.ids))
            {
                int[] idsArr = Tools.SpitIntArrary(parm.ids);
                var selectDataList = new List<ProjectFilesVo>();
                foreach (var item in idsArr)
                {
                    var select_data = data.Where(s => s.ProjectFilesId == item).First();
                    selectDataList.Add(select_data);
                }
                data = selectDataList;
            }

            

            // 导出数据处理
            var handleData = await _ProjectFilesService.HandleExportData(data);

            string sFileName = ExportExcel(handleData, "ProjectFiles", "项目配置列表");
            return SUCCESS(new { path = "/export/" + sFileName, fileName = sFileName });
        }




    }
}
