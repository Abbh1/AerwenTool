using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARW.Model;
using ARW.Model.Dto.Business.Download.DownloadFiless;
using ARW.Model.Models.Business.Download.DownloadFiless;
using ARW.Model.Vo.Business.Download.DownloadFiless;

namespace ARW.Service.Business.IBusinessService.Download.DownloadFiless
{
    public interface IDownloadFilesService : IBaseService<DownloadFiles>
    {
		/// <summary>
        /// 获取下载文件分页列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<PagedInfo<DownloadFilesVo>> GetDownloadFilesList(DownloadFilesQueryDto parm);

		
		/// <summary>
        /// 添加或修改下载文件
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<string> AddOrUpdateDownloadFiles(DownloadFiles parm);



        /// <summary>
        /// Excel导出
        /// </summary>
        Task<List<DownloadFilesVo>> HandleExportData(List<DownloadFilesVo> data);

        /// <summary>
        /// 审核
        /// </summary>
        Task<string> Audit(int idsArr, int status,long userGuid);

    }
}
