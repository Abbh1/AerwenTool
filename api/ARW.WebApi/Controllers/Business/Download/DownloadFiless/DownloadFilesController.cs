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
using ARW.Model.Dto.Business.Download.DownloadFiless;
using ARW.Service.Business.IBusinessService.Download.DownloadFiless;
using ARW.Admin.WebApi.Controllers;
using ARW.Model.Models.Business.Download.DownloadFiless;
using ARW.Model.Vo.Business.Download.DownloadFiless;
using Microsoft.AspNetCore.Authorization;
using ARW.Admin.WebApi.Framework;


namespace ARW.WebApi.Controllers.Business.Download.DownloadFiless
{
    /// <summary>
    /// 下载文件控制器
    /// </summary>
    [Verify]
    [Route("business/[controller]")]
    public class DownloadFilesController : BaseController
    {
        private readonly IDownloadFilesService _DownloadFilesService;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="DownloadFilesService">下载文件下载文件服务</param>
        public DownloadFilesController(IDownloadFilesService DownloadFilesService)
        {
            _DownloadFilesService = DownloadFilesService;
        }


        /// <summary>
        /// 获取下载文件列表
        /// </summary>
        /// <param name="parm">查询参数</param>
        /// <returns></returns>
        [HttpGet("getDownloadFilesList")]
        [ActionPermissionFilter(Permission = "business:downloadfiles:list")]
        public async Task<IActionResult> GetDownloadFilesList([FromQuery] DownloadFilesQueryDto parm)
        {
            var res = await _DownloadFilesService.GetDownloadFilesList(parm);
            return SUCCESS(res);
        }

        /// <summary>
        /// 添加或修改下载文件
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost("addOrUpdateDownloadFiles")]
        [ActionPermissionFilter(Permission = "business:downloadfiles:addOrUpdate")]
        [Log(Title = "添加或修改下载文件", BusinessType = BusinessType.ADDORUPDATE)]
        public async Task<IActionResult> AddOrUpdateDownloadFiles([FromBody] DownloadFilesDto parm)
        {
            if (parm == null) { throw new CustomException("请求参数错误"); }

	var modal = new DownloadFiles();
            if (parm.DownloadFilesId != 0) modal = parm.Adapt<DownloadFiles>().ToUpdate(HttpContext);
            else modal = parm.Adapt<DownloadFiles>().ToCreate(HttpContext);

            var res = await _DownloadFilesService.AddOrUpdateDownloadFiles(modal);
            return SUCCESS(res);
        }

        /// <summary>
        /// 删除下载文件
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{ids}")]
        [ActionPermissionFilter(Permission = "business:downloadfiles:delete")]
        [Log(Title = "下载文件删除", BusinessType = BusinessType.DELETE)]
        public IActionResult Delete(string ids)
        {
            long[] idsArr = Tools.SpitLongArrary(ids);
            if (idsArr.Length <= 0) { return ToResponse(ApiResult.Error($"删除失败Id 不能为空")); }
            var response = _DownloadFilesService.Delete(idsArr);
            return SUCCESS("删除成功!");
        }
		
		
		/// <summary>
        /// 导出下载文件
        /// </summary>
        /// <returns></returns>
        [Log(Title = "下载文件导出", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [HttpGet("exportDownloadFiles")]
        [ActionPermissionFilter(Permission = "business:downloadfiles:export")]
        public async Task<IActionResult> ExportExcel([FromQuery] DownloadFilesQueryDto parm)                  
        {
            parm.PageSize = 10000;
            var list = await _DownloadFilesService.GetDownloadFilesList(parm);
            var data = list.Result;

            // 选中数据
            if (!string.IsNullOrEmpty(parm.ids))
            {
                int[] idsArr = Tools.SpitIntArrary(parm.ids);
                var selectDataList = new List<DownloadFilesVo>();
                foreach (var item in idsArr)
                {
                    var select_data = data.Where(s => s.DownloadFilesId == item).First();
                    selectDataList.Add(select_data);
                }
                data = selectDataList;
            }

            

            // 导出数据处理
            var handleData = await _DownloadFilesService.HandleExportData(data);

            string sFileName = ExportExcel(handleData, "DownloadFiles", "下载文件列表");
            return SUCCESS(new { path = "/export/" + sFileName, fileName = sFileName });
        }

		/// <summary>
        /// 审核下载文件
        /// </summary>
        /// <returns></returns>
        [HttpPut("audit")]
        [ActionPermissionFilter(Permission = "business:downloadfiles:audit")]
        [Log(Title = "审核下载文件", BusinessType = BusinessType.AUDIT)]
        public async Task<IActionResult> AuditDownloadFiles([FromBody] DownloadFilesAuditDto param)
        {
            var user = JwtUtil.GetLoginUser(App.HttpContext);
            int[] idsArr = Tools.SpitIntArrary(param.ids);
            if (idsArr.Length <= 0) { return ToResponse(ApiResult.Error($"审核失败 Id 不能为空")); }

            var msgList = new List<string>();
            foreach (var item in idsArr)
            {
                var msg = await _DownloadFilesService.Audit(item, param.DownloadFilesAuditStatus, user.UserId);
                msgList.Add(msg);
            }

            return SUCCESS(msgList.ToArray());
        }



    }
}
