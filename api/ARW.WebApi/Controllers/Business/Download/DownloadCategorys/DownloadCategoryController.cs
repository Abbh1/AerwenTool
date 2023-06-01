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
using ARW.Model.Dto.Business.Download.DownloadCategorys;
using ARW.Service.Business.IBusinessService.Download.DownloadCategorys;
using ARW.Admin.WebApi.Controllers;
using ARW.Model.Models.Business.Download.DownloadCategorys;
using ARW.Model.Vo.Business.Download.DownloadCategorys;
using Microsoft.AspNetCore.Authorization;
using ARW.Admin.WebApi.Framework;


namespace ARW.WebApi.Controllers.Business.Download.DownloadCategorys
{
    /// <summary>
    /// 下载分类控制器
    /// </summary>
    [Verify]
    [Route("business/[controller]")]
    public class DownloadCategoryController : BaseController
    {
        private readonly IDownloadCategoryService _DownloadCategoryService;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="DownloadCategoryService">下载分类下载分类服务</param>
        public DownloadCategoryController(IDownloadCategoryService DownloadCategoryService)
        {
            _DownloadCategoryService = DownloadCategoryService;
        }


        /// <summary>
        /// 获取下载分类树形列表
        /// </summary>
        /// <param name="parm">查询参数</param>
        /// <returns></returns>
        [HttpGet("getDownloadCategoryTreeList")]
        [ActionPermissionFilter(Permission = "business:downloadcategory:treelist")]
        public async Task<IActionResult> GetDownloadCategoryList([FromQuery] DownloadCategoryQueryDto parm)
        {
            var res = await _DownloadCategoryService.GetDownloadCategoryTreeList(parm);
            res ??= new List<DownloadCategoryVo>();

            return SUCCESS(res);
        }

        /// <summary>
        /// 添加或修改下载分类
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost("addOrUpdateDownloadCategory")]
        [ActionPermissionFilter(Permission = "business:downloadcategory:addOrUpdate")]
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
        /// 删除下载分类
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{ids}")]
        [ActionPermissionFilter(Permission = "business:downloadcategory:delete")]
        [Log(Title = "下载分类删除", BusinessType = BusinessType.DELETE)]
        public IActionResult Delete(string ids)
        {
            long[] idsArr = Tools.SpitLongArrary(ids);
            if (idsArr.Length <= 0) { return ToResponse(ApiResult.Error($"删除失败Id 不能为空")); }
            var response = _DownloadCategoryService.Delete(idsArr);
            return SUCCESS("删除成功!");
        }


        /// <summary>
        /// 导出下载分类
        /// </summary>
        /// <returns></returns>
        [Log(Title = "下载分类导出", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [HttpGet("exportDownloadCategory")]
        [ActionPermissionFilter(Permission = "business:downloadcategory:export")]
        public async Task<IActionResult> ExportExcel([FromQuery] DownloadCategoryQueryDto parm)
        {
            var list = await _DownloadCategoryService.GetDownloadCategoryList(parm);
            var data = list;

            // 选中数据
            if (!string.IsNullOrEmpty(parm.ids))
            {
                int[] idsArr = Tools.SpitIntArrary(parm.ids);
                var selectDataList = new List<DownloadCategoryVo>();
                foreach (var item in idsArr)
                {
                    var select_data = data.Where(s => s.DownloadCategoryId == item).First();
                    selectDataList.Add(select_data);

                    // 查看当前数据有没有子级
                    var newDownloadCategorys = data.FindAll(delegate (DownloadCategoryVo downloadCategory)
                    {
                        string[] parentDownloadCategoryId = downloadCategory.DownloadCategoryAncestralGuid.Split(",", StringSplitOptions.RemoveEmptyEntries);
                        return parentDownloadCategoryId.Contains(select_data.DownloadCategoryGuid.ToString());
                    });
                    string[] downloadCategoryArr = newDownloadCategorys.Select(x => x.DownloadCategoryGuid.ToString()).ToArray();
                    var ancestorArr = data.Where(s => downloadCategoryArr.Contains(s.DownloadCategoryGuid.ToString())).ToList();
                    selectDataList.AddRange(ancestorArr);
                }
                data = selectDataList;
            }



            // 导出数据处理
            var handleData = await _DownloadCategoryService.HandleExportData(data);

            string sFileName = ExportExcel(handleData, "DownloadCategory", "下载分类列表");
            return SUCCESS(new { path = "/export/" + sFileName, fileName = sFileName });
        }

        /// <summary>
        /// 审核下载分类
        /// </summary>
        /// <returns></returns>
        [HttpPut("audit")]
        [ActionPermissionFilter(Permission = "business:downloadcategory:audit")]
        [Log(Title = "审核下载分类", BusinessType = BusinessType.AUDIT)]
        public async Task<IActionResult> AuditDownloadCategory([FromBody] DownloadCategoryAuditDto param)
        {
            var user = JwtUtil.GetLoginUser(App.HttpContext);
            int[] idsArr = Tools.SpitIntArrary(param.ids);
            if (idsArr.Length <= 0) { return ToResponse(ApiResult.Error($"审核失败 Id 不能为空")); }

            var msgList = new List<string>();
            foreach (var item in idsArr)
            {
                var msg = await _DownloadCategoryService.Audit(item, param.DownloadCategoryAuditStatus, user.UserId);
                msgList.Add(msg);
            }

            return SUCCESS(msgList.ToArray());
        }



    }
}
