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
using ARW.Model.Dto.Business.Project.Projects;
using ARW.Service.Business.IBusinessService.Project.Projects;
using ARW.Admin.WebApi.Controllers;
using ARW.Model.Models.Business.Project.Projects;
using ARW.Model.Vo.Business.Project.Projects;
using Microsoft.AspNetCore.Authorization;
using ARW.Admin.WebApi.Framework;


namespace ARW.WebApi.Controllers.Business.Project.Projects
{
    /// <summary>
    /// 项目控制器
    /// </summary>
    [Verify]
    [Route("business/[controller]")]
    public class ProjectesController : BaseController
    {
        private readonly IProjectesService _ProjectesService;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="ProjectesService">项目项目服务</param>
        public ProjectesController(IProjectesService ProjectesService)
        {
            _ProjectesService = ProjectesService;
        }


        /// <summary>
        /// 获取项目列表
        /// </summary>
        /// <param name="parm">查询参数</param>
        /// <returns></returns>
        [HttpGet("getProjectesList")]
        [ActionPermissionFilter(Permission = "business:project:list")]
        public async Task<IActionResult> GetProjectesList([FromQuery] ProjectesQueryDto parm)
        {
            var res = await _ProjectesService.GetProjectesList(parm);
            return SUCCESS(res);
        }

        /// <summary>
        /// 添加或修改项目
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost("addOrUpdateProjectes")]
        [ActionPermissionFilter(Permission = "business:project:addOrUpdate")]
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
        [ActionPermissionFilter(Permission = "business:project:delete")]
        [Log(Title = "项目删除", BusinessType = BusinessType.DELETE)]
        public IActionResult Delete(string ids)
        {
            long[] idsArr = Tools.SpitLongArrary(ids);
            if (idsArr.Length <= 0) { return ToResponse(ApiResult.Error($"删除失败Id 不能为空")); }
            var response = _ProjectesService.Delete(idsArr);
            return SUCCESS("删除成功!");
        }

        /// <summary>
        /// 导入项目
        /// </summary>
        /// <param name="formFile">使用IFromFile必须使用name属性否则获取不到文件</param>
        /// <param name="updateSupport">是否需要更新</param>
        /// <returns></returns>
        [HttpPost("importData")]
        [Log(Title = "项目导入", BusinessType = BusinessType.IMPORT, IsSaveRequestData = false, IsSaveResponseData = false)]
        [ActionPermissionFilter(Permission = "business:business:project:import")]
        public async Task<IActionResult> ImportExcel([FromForm(Name = "file")] IFormFile formFile, bool updateSupport)
        {
            var isUpdateSupport = updateSupport;
            IEnumerable<ProjectesVo> parm = ExcelHelper<ProjectesVo>.ImportData(formFile.OpenReadStream());

            var i = 0;
            var msgList = new List<string>();
            foreach (ProjectesVo item in parm)
            {
                i++;
                var Projectes = await _ProjectesService.HandleImportData(item);
                var modal = Projectes.Adapt<Projectes>().ToCreate(HttpContext);
                var user = JwtUtil.GetLoginUser(App.HttpContext).UserName;
                var msg = await _ProjectesService.ImportExcel(modal, i, isUpdateSupport, user);
                msgList.Add(msg);
            }

            return SUCCESS(msgList.ToArray());
        }


        /// <summary>
        /// 项目导入模板下载
        /// </summary>
        /// <returns></returns> 
        [HttpGet("importTemplate")]
        [Log(Title = "项目模板", BusinessType = BusinessType.EXPORT, IsSaveRequestData = false, IsSaveResponseData = false)]
        [AllowAnonymous]
        public IActionResult ImportTemplateExcel()
        {
            List<ProjectesVo> Projectes = new List<ProjectesVo>();
            MemoryStream stream = new MemoryStream();

            // 示例数据
            var values = new List<string>() { "111", "222", "333" };
            string sFileName = DownloadImportTemplate(Projectes, stream, "项目导入模板", values);

            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{sFileName}");
        }

        /// <summary>
        /// 导出项目
        /// </summary>
        /// <returns></returns>
        [Log(Title = "项目导出", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [HttpGet("exportProjectes")]
        [ActionPermissionFilter(Permission = "business:project:export")]
        public async Task<IActionResult> ExportExcel([FromQuery] ProjectesQueryDto parm)
        {
            parm.PageSize = 10000;
            var list = await _ProjectesService.GetProjectesList(parm);
            var data = list.Result;

            // 选中数据
            if (!string.IsNullOrEmpty(parm.ids))
            {
                int[] idsArr = Tools.SpitIntArrary(parm.ids);
                var selectDataList = new List<ProjectesVo>();
                foreach (var item in idsArr)
                {
                    var select_data = data.Where(s => s.ProjectId == item).First();
                    selectDataList.Add(select_data);
                }
                data = selectDataList;
            }



            // 导出数据处理
            var handleData = await _ProjectesService.HandleExportData(data);

            string sFileName = ExportExcel(handleData, "Projectes", "项目列表");
            return SUCCESS(new { path = "/export/" + sFileName, fileName = sFileName });
        }




    }
}
