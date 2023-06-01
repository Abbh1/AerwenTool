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
using ARW.Model.Dto.Business.Project.ProjectGroups;
using ARW.Service.Business.IBusinessService.Project.ProjectGroups;
using ARW.Admin.WebApi.Controllers;
using ARW.Model.Models.Business.Project.ProjectGroups;
using ARW.Model.Vo.Business.Project.ProjectGroups;
using Microsoft.AspNetCore.Authorization;
using ARW.Admin.WebApi.Framework;


namespace ARW.WebApi.Controllers.Business.Project.ProjectGroups
{
    /// <summary>
    /// 项目分组控制器
    /// </summary>
    [Verify]
    [Route("business/[controller]")]
    public class ProjectGroupController : BaseController
    {
        private readonly IProjectGroupService _ProjectGroupService;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="ProjectGroupService">项目分组项目分组服务</param>
        public ProjectGroupController(IProjectGroupService ProjectGroupService)
        {
            _ProjectGroupService = ProjectGroupService;
        }


        /// <summary>
        /// 获取项目分组树形列表
        /// </summary>
        /// <param name="parm">查询参数</param>
        /// <returns></returns>
        [HttpGet("getProjectGroupTreeList")]
        [ActionPermissionFilter(Permission = "business:projectgroup:treelist")]
        public async Task<IActionResult> GetProjectGroupList([FromQuery] ProjectGroupQueryDto parm)
        {
            var res = await _ProjectGroupService.GetProjectGroupTreeList(parm);
            res ??= new List<ProjectGroupVo>();
				
            return SUCCESS(res);
        }

        /// <summary>
        /// 添加或修改项目分组
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost("addOrUpdateProjectGroup")]
        [ActionPermissionFilter(Permission = "business:projectgroup:addOrUpdate")]
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
        /// 删除项目分组
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{ids}")]
        [ActionPermissionFilter(Permission = "business:projectgroup:delete")]
        [Log(Title = "项目分组删除", BusinessType = BusinessType.DELETE)]
        public IActionResult Delete(string ids)
        {
            long[] idsArr = Tools.SpitLongArrary(ids);
            if (idsArr.Length <= 0) { return ToResponse(ApiResult.Error($"删除失败Id 不能为空")); }
            var response = _ProjectGroupService.Delete(idsArr);
            return SUCCESS("删除成功!");
        }
		
		/// <summary>
        /// 导入项目分组
        /// </summary>
        /// <param name="formFile">使用IFromFile必须使用name属性否则获取不到文件</param>
        /// <param name="updateSupport">是否需要更新</param>
        /// <returns></returns>
        [HttpPost("importData")]
        [Log(Title = "项目分组导入", BusinessType = BusinessType.IMPORT, IsSaveRequestData = false, IsSaveResponseData = false)]
        [ActionPermissionFilter(Permission = "business:business:projectgroup:import")]
        public async Task<IActionResult> ImportExcel([FromForm(Name = "file")] IFormFile formFile,bool updateSupport)
        {
            var isUpdateSupport = updateSupport;
            IEnumerable<ProjectGroupVo> parm = ExcelHelper<ProjectGroupVo>.ImportData(formFile.OpenReadStream());

            var i = 0;
            var msgList = new List<string>();
            foreach (ProjectGroupVo item in parm)
            {
                i++;
                var ProjectGroup = await _ProjectGroupService.HandleImportData(item);
                var modal = ProjectGroup.Adapt<ProjectGroup>().ToCreate(HttpContext);
                var user = JwtUtil.GetLoginUser(App.HttpContext).UserName;
                var msg = await _ProjectGroupService.ImportExcel(modal,i,isUpdateSupport,user);
                msgList.Add(msg);
            }

            return SUCCESS(msgList.ToArray());
        }


        /// <summary>
        /// 项目分组导入模板下载
        /// </summary>
        /// <returns></returns> 
        [HttpGet("importTemplate")]
        [Log(Title = "项目分组模板", BusinessType = BusinessType.EXPORT, IsSaveRequestData = false, IsSaveResponseData = false)]
        [AllowAnonymous]
        public IActionResult ImportTemplateExcel()
        {
            List<ProjectGroupVo> ProjectGroup = new List<ProjectGroupVo>();
            MemoryStream stream = new MemoryStream();

            // 示例数据
            var allValues = new List<List<string>>();
            var values = new List<string>() { "111", "222", "333" };
            var values2 = new List<string>() { "444", "555", "666" };
            allValues.Add(values);
            allValues.Add(values2);
            string sFileName = DownloadImportTemplate(ProjectGroup, stream, "项目分组导入模板", allValues);

            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{sFileName}");
        }
		
		/// <summary>
        /// 导出项目分组
        /// </summary>
        /// <returns></returns>
        [Log(Title = "项目分组导出", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [HttpGet("exportProjectGroup")]
        [ActionPermissionFilter(Permission = "business:projectgroup:export")]
        public async Task<IActionResult> ExportExcel([FromQuery] ProjectGroupQueryDto parm)                  
        {
            var list = await _ProjectGroupService.GetProjectGroupList(parm);
            var data = list;

            // 选中数据
            if (!string.IsNullOrEmpty(parm.ids))
            {
                int[] idsArr = Tools.SpitIntArrary(parm.ids);
                var selectDataList = new List<ProjectGroupVo>();
                foreach (var item in idsArr)
                {
                    var select_data = data.Where(s => s.ProjectGroupId == item).First();
                    selectDataList.Add(select_data);

                    // 查看当前数据有没有子级
                    var newProjectGroups = data.FindAll(delegate (ProjectGroupVo projectGroup)
                    {
                        string[] parentProjectGroupId = projectGroup.ProjectGroupAncestralGuid.Split(",", StringSplitOptions.RemoveEmptyEntries);
                        return parentProjectGroupId.Contains(select_data.ProjectGroupGuid.ToString());
                    });
                    string[] projectGroupArr = newProjectGroups.Select(x => x.ProjectGroupGuid.ToString()).ToArray();
                    var ancestorArr = data.Where(s => projectGroupArr.Contains(s.ProjectGroupGuid.ToString())).ToList();
                    selectDataList.AddRange(ancestorArr);
                }
                data = selectDataList;
            }

            

            // 导出数据处理
            var handleData = await _ProjectGroupService.HandleExportData(data);

            string sFileName = ExportExcel(handleData, "ProjectGroup", "项目分组列表");
            return SUCCESS(new { path = "/export/" + sFileName, fileName = sFileName });
        }




    }
}
