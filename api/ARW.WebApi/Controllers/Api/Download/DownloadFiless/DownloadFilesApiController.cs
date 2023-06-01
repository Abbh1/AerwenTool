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
using ARW.Model.Dto.Api.Download.DownloadFiless;
using ARW.Service.Api.IBusinessService.Download.DownloadFiless;
using ARW.Model.Models.Business.Download.DownloadFiless;
using ARW.Model.Vo.Api.Download.DownloadFiless;
using Microsoft.AspNetCore.Authorization;
using Geocoding;
using ARW.Service.Business.IBusinessService.Download.DownloadFiless;
using ARW.Model.Dto.Business.Download.DownloadFiless;

namespace ARW.WebApi.Controllers.Api.Download.DownloadFiless
{
    /// <summary>
    /// 下载文件控制器Api
    /// </summary>
    [Verify]
    [Route("api/[controller]")]
    public class DownloadFilesApiController : BaseController
    {
        private readonly IDownloadFilesServiceApi _DownloadFilesServiceApi;
        private readonly IDownloadFilesService _DownloadFilesService;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="DownloadFilesServiceApi">下载文件下载文件Api服务</param>
        /// <param name="downloadFilesService">下载文件下载文件服务</param>
        public DownloadFilesApiController(IDownloadFilesServiceApi DownloadFilesServiceApi, IDownloadFilesService downloadFilesService)
        {
            _DownloadFilesServiceApi = DownloadFilesServiceApi;
            _DownloadFilesService = downloadFilesService;
        }


        /// <summary>
        /// 获取下载文件列表(Api)
        /// </summary>
        /// <param name="parm">查询参数</param>
        /// <returns></returns>
        [HttpGet("getDownloadFilesList")]
        public async Task<IActionResult> GetDownloadFilesListApi([FromQuery] DownloadFilesQueryApiDto parm)
        {
            var res = await _DownloadFilesServiceApi.GetDownloadFilesListApi(parm);
            return SUCCESS(res);
        }

        /// <summary>
        /// 添加或修改下载文件
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost("addOrUpdateDownloadFiles")]
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
        /// 获取DownloadFiles详情(Api)
        /// </summary>
        /// <param name="parm">查询参数</param>
        /// <returns></returns>
        [HttpGet("getDownloadFilesDetails")]
        public async Task<IActionResult> GetDownloadFilesDetails([FromQuery] DownloadFilesApiDto parm)
        {
            //if (parm == null) throw new CustomException("参数错误！");

            var res = await _DownloadFilesServiceApi.GetDownloadFilesDetails(parm);

            if (res != "[]")
            {	
                res = res.Remove(0, 1);
                res = res.Substring(0, res.Length - 1);
                var data = res.FromJSON<DownloadFilesApiDetailsVo>();
                return SUCCESS(data);
            }
            else
            {
                return SUCCESS(res);
            }
        }

    }
}
