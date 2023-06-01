using Infrastructure.Attribute;
using Microsoft.AspNetCore.Http;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARW.Model;
using ARW.Repository;
using ARW.Repository.Business.Download.DownloadFiless;
using ARW.Service.Api.IBusinessService.Download.DownloadFiless;
using ARW.Model.Dto.Api.Download.DownloadFiless;
using ARW.Model.Models.Business.Download.DownloadFiless;
using ARW.Model.Vo.Api.Download.DownloadFiless;
using Senparc.CO2NET.Extensions;
using ARW.Model.Models.Business.Project.ProjectGroups;
using ARW.Repository.Business.Project.ProjectGroups;
using ARW.Repository.Business.Download.DownloadCategorys;
using ARW.Model.Models.Business.Download.DownloadCategorys;

namespace ARW.Service.Api.BusinessService.Download.DownloadFiless
{
    /// <summary>
    /// 下载文件接口实现类
    /// </summary>
    [AppService(ServiceType = typeof(IDownloadFilesServiceApi), ServiceLifetime = LifeTime.Transient)]
    public class DownloadFilesServiceImplApi : BaseService<DownloadFiles>, IDownloadFilesServiceApi
    {
        private readonly DownloadFilesRepository _DownloadFilesRepository;
        private readonly DownloadCategoryRepository _DownloadCategoryRepository;

        public DownloadFilesServiceImplApi(DownloadFilesRepository DownloadFilesRepository, DownloadCategoryRepository downloadCategoryRepository)
        {
            this._DownloadFilesRepository = DownloadFilesRepository;
            _DownloadCategoryRepository = downloadCategoryRepository;
        }

        #region Api接口代码


        /// <summary>
        /// 查询下载文件列表(Api)
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<List<DownloadFilesApiVo>> GetDownloadFilesListApi(DownloadFilesQueryApiDto parm)
        {
            //开始拼装查询条件d
            var predicate = Expressionable.Create<DownloadFiles>();

            if (parm.DownloadCategoryGuid != 0)
            {
                var data = await _DownloadCategoryRepository.GetListAsync();

                var newDownloadCategory = data.FindAll(delegate (DownloadCategory downloadCategory)
                {
                    string[] parentProjectGroupId = downloadCategory.DownloadCategoryAncestralGuid.Split(",", StringSplitOptions.RemoveEmptyEntries);
                    return parentProjectGroupId.Contains(parm.DownloadCategoryGuid.ToString());
                });
                string[] downloadCategoryArr = newDownloadCategory.Select(s => s.DownloadCategoryGuid.ToString()).ToArray();
                predicate = predicate.AndIF(parm.DownloadCategoryGuid != 0, s => s.DownloadCategoryGuid == parm.DownloadCategoryGuid || downloadCategoryArr.Contains(s.DownloadCategoryGuid.ToString()));
            }

            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.DownloadFilesName), it => it.DownloadFilesName.Contains(parm.DownloadFilesName));
            var query = _DownloadFilesRepository
                .Queryable()
                .Where(predicate.ToExpression())
                .Where(s => s.DownloadFilesAuditStatus == 2)
                .OrderBy(s => s.Update_time, OrderByType.Desc)
                .Select(s => new DownloadFilesApiVo
                {
                    DownloadFilesId = s.DownloadFilesId,
                    DownloadFilesGuid = s.DownloadFilesGuid,
                    DownloadCategoryGuid = s.DownloadCategoryGuid,
                    DownloadFilesIcon = s.DownloadFilesIcon,
                    DownloadFilesName = s.DownloadFilesName,
                    DownloadFilesIntro = s.DownloadFilesIntro,
                    DownloadFilesLink = s.DownloadFilesLink,
                    DownloadFilesAttachment = s.DownloadFilesAttachment,
                    DownloadFilesSize = s.DownloadFilesSize,
                    DownloadFilesVolume = s.DownloadFilesVolume,
                    DownloadFilesAuditStatus = s.DownloadFilesAuditStatus,
                    DownloadFilesAuditUserGuid = s.DownloadFilesAuditUserGuid,
                    CreateBy = s.Create_by
                });


            return await query.ToListAsync();
        }

        /// <summary>
        /// 查询下载文件详情(Api)
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public Task<string> GetDownloadFilesDetails(DownloadFilesApiDto parm)
        {

            var query = _DownloadFilesRepository
                .Queryable()
                .Where(s => s.DownloadFilesGuid == parm.DownloadFilesGuid)
                .Select(s => new DownloadFilesApiDetailsVo
                {
                    DownloadFilesId = s.DownloadFilesId,
                    DownloadFilesGuid = s.DownloadFilesGuid,
                    DownloadCategoryGuid = s.DownloadCategoryGuid,
                    DownloadFilesIcon = s.DownloadFilesIcon,
                    DownloadFilesName = s.DownloadFilesName,
                    DownloadFilesIntro = s.DownloadFilesIntro,
                    DownloadFilesLink = s.DownloadFilesLink,
                    DownloadFilesAttachment = s.DownloadFilesAttachment,
                    DownloadFilesSize = s.DownloadFilesSize,
                    DownloadFilesVolume = s.DownloadFilesVolume,
                    DownloadFilesAuditStatus = s.DownloadFilesAuditStatus,
                    DownloadFilesAuditUserGuid = s.DownloadFilesAuditUserGuid,
                }).Take(1);


            return query.ToJsonAsync();
        }


        #endregion

    }
}
