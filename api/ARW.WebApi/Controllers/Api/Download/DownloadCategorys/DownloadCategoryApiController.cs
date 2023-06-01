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
using ARW.Model.Dto.Api.Download.DownloadCategorys;
using ARW.Service.Api.IBusinessService.Download.DownloadCategorys;
using ARW.Model.Models.Business.Download.DownloadCategorys;
using ARW.Model.Vo.Api.Download.DownloadCategorys;
using Microsoft.AspNetCore.Authorization;
using Geocoding;
using ARW.Model.Dto.Business.Download.DownloadCategorys;
using ARW.Service.Business.IBusinessService.Download.DownloadCategorys;

namespace ARW.WebApi.Controllers.Api.Download.DownloadCategorys
{
    /// <summary>
    /// 下载分类控制器Api
    /// </summary>
    [Verify]
    [Route("api/[controller]")]
    public class DownloadCategoryApiController : BaseController
    {
        private readonly IDownloadCategoryServiceApi _DownloadCategoryServiceApi;
        private readonly IDownloadCategoryService _DownloadCategoryService;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="DownloadCategoryServiceApi">下载分类下载分类Api服务</param>
        /// <param name="downloadCategoryService">下载分类下载分类服务</param>
        public DownloadCategoryApiController(IDownloadCategoryServiceApi DownloadCategoryServiceApi, IDownloadCategoryService downloadCategoryService)
        {
            _DownloadCategoryServiceApi = DownloadCategoryServiceApi;
            _DownloadCategoryService = downloadCategoryService;
        }


        /// <summary>
        /// 获取下载分类树形列表(Api)
        /// </summary>
        /// <param name="parm">查询参数</param>
        /// <returns></returns>
        [HttpGet("getDownloadCategoryTreeList")]
        public async Task<IActionResult> GetDownloadCategoryTreeListApi([FromQuery] DownloadCategoryQueryDtoApi parm)
        {
            var res = await _DownloadCategoryServiceApi.GetDownloadCategoryTreeListApi(parm);
            if (res == null)
                res = new List<DownloadCategoryVoApi>();

            return SUCCESS(res);
        }


        /// <summary>
        /// 获取下载分类列表(Api)
        /// </summary>
        /// <param name="parm">查询参数</param>
        /// <returns></returns>
        [HttpGet("getDownloadCategoryList")]
        public async Task<IActionResult> GetDownloadCategoryListApi([FromQuery] DownloadCategoryQueryDtoApi parm)
        {
            var res = await _DownloadCategoryServiceApi.GetDownloadCategoryListApi(parm);
            if (res == null)
                res = new List<DownloadCategoryVoApi>();

            return SUCCESS(res);
        }


        /// <summary>
        /// 添加或修改下载分类
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost("addOrUpdateDownloadCategory")]
        [Log(Title = "添加或修改下载分类", BusinessType = BusinessType.ADDORUPDATE)]
        public async Task<IActionResult> AddOrUpdateDownloadCategory([FromBody] DownloadCategoryDto parm)
        {
            if (parm == null) { throw new CustomException("请求参数错误"); }

            var modal = new DownloadCategory();
            if (parm.DownloadCategoryId != 0) modal = parm.Adapt<DownloadCategory>().ToUpdate(HttpContext);
            else modal = parm.Adapt<DownloadCategory>().ToCreate(HttpContext);

            var res = await _DownloadCategoryService.AddOrUpdateDownloadCategory(modal);
            return SUCCESS(res);
        }


        /// <summary>
        /// 获取DownloadCategory详情(Api)
        /// </summary>
        /// <param name="parm">查询参数</param>
        /// <returns></returns>
        [HttpGet("getDownloadCategoryDetails")]
        public async Task<IActionResult> GetDownloadCategoryDetails([FromQuery] DownloadCategoryDtoApi parm)
        {
            //if (parm == null) throw new CustomException("参数错误！");

            var res = await _DownloadCategoryServiceApi.GetDownloadCategoryDetails(parm);

            if (res != "[]")
            {
                res = res.Remove(0, 1);
                res = res.Substring(0, res.Length - 1);
                var data = res.FromJSON<DownloadCategoryApiDetailsVo>();
                return SUCCESS(data);
            }
            else
            {
                return SUCCESS(res);
            }
        }

    }
}
