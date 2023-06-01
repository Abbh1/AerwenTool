using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARW.Model;
using ARW.Model.Dto.Business.Download.DownloadCategorys;
using ARW.Model.Models.Business.Download.DownloadCategorys;
using ARW.Model.Vo.Business.Download.DownloadCategorys;

namespace ARW.Service.Business.IBusinessService.Download.DownloadCategorys
{
    public interface IDownloadCategoryService : IBaseService<DownloadCategory>
    {
        /// <summary>
        /// 获取下载分类树形列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<List<DownloadCategoryVo>> GetDownloadCategoryTreeList(DownloadCategoryQueryDto parm);

        /// <summary>
        /// 获取下载分类列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<List<DownloadCategoryVo>> GetDownloadCategoryList(DownloadCategoryQueryDto parm);

		
		/// <summary>
        /// 添加或修改下载分类
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<string> AddOrUpdateDownloadCategory(DownloadCategory parm);



        /// <summary>
        /// Excel导出
        /// </summary>
        Task<List<DownloadCategoryVo>> HandleExportData(List<DownloadCategoryVo> data);

        /// <summary>
        /// 审核
        /// </summary>
        Task<string> Audit(int idsArr, int status,long userGuid);

    }
}
