using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARW.Model;
using ARW.Model.Dto.Api.Download.DownloadFiless;
using ARW.Model.Models.Business.Download.DownloadFiless;
using ARW.Model.Vo.Api.Download.DownloadFiless;

namespace ARW.Service.Api.IBusinessService.Download.DownloadFiless
{
    public interface IDownloadFilesServiceApi : IBaseService<DownloadFiles>
    {
		/// <summary>
        /// 获取下载文件分页列表(Api)
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<List<DownloadFilesApiVo>> GetDownloadFilesListApi(DownloadFilesQueryApiDto parm);

		/// <summary>
        /// 获取下载文件详情(Api)
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<string> GetDownloadFilesDetails(DownloadFilesApiDto parm);

    }
}
