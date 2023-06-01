using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARW.Model;
using ARW.Model.Dto.Api.Download.DownloadCategorys;
using ARW.Model.Models.Business.Download.DownloadCategorys;
using ARW.Model.Vo.Api.Download.DownloadCategorys;

namespace ARW.Service.Api.IBusinessService.Download.DownloadCategorys
{
    public interface IDownloadCategoryServiceApi : IBaseService<DownloadCategory>
    {
        /// <summary>
        /// 获取下载分类树形列表(Api)
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<List<DownloadCategoryVoApi>> GetDownloadCategoryTreeListApi(DownloadCategoryQueryDtoApi parm);

        /// <summary>
        /// 获取下载分类列表(Api)
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<List<DownloadCategoryVoApi>> GetDownloadCategoryListApi(DownloadCategoryQueryDtoApi parm);

        /// <summary>
        /// 获取下载分类详情(Api)
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<string> GetDownloadCategoryDetails(DownloadCategoryDtoApi parm);

    }
}
