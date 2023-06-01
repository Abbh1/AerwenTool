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
using ARW.Repository.Business.Download.DownloadCategorys;
using ARW.Service.Api.IBusinessService.Download.DownloadCategorys;
using ARW.Model.Dto.Api.Download.DownloadCategorys;
using ARW.Model.Models.Business.Download.DownloadCategorys;
using ARW.Model.Vo.Api.Download.DownloadCategorys;

namespace ARW.Service.Api.BusinessService.Download.DownloadCategorys
{
    /// <summary>
    /// 下载分类接口实现类
    /// </summary>
    [AppService(ServiceType = typeof(IDownloadCategoryServiceApi), ServiceLifetime = LifeTime.Transient)]
    public class DownloadCategoryServiceImplApi : BaseService<DownloadCategory>, IDownloadCategoryServiceApi
    {
        private readonly DownloadCategoryRepository _DownloadCategoryRepository;

        public DownloadCategoryServiceImplApi(DownloadCategoryRepository DownloadCategoryRepository)
        {
            this._DownloadCategoryRepository = DownloadCategoryRepository;
        }

        #region Api接口代码


        /// <summary>
        /// 查询下载分类树形列表(Api)
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<List<DownloadCategoryVoApi>> GetDownloadCategoryTreeListApi(DownloadCategoryQueryDtoApi parm)
        {
            //开始拼装查询条件d
            var predicate = Expressionable.Create<DownloadCategory>();

            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.DownloadCategoryName), s => s.DownloadCategoryName.Contains(parm.DownloadCategoryName));
            predicate = predicate.AndIF(parm.DownloadCategoryAuditStatus != null, s => s.DownloadCategoryAuditStatus == parm.DownloadCategoryAuditStatus);
            var query = _DownloadCategoryRepository
                .Queryable()
                .LeftJoin<DownloadCategory>((s, c) => s.DownloadCategoryParentGuid == c.DownloadCategoryGuid)
                .Where(predicate.ToExpression())
                .Where(s => s.DownloadCategoryAuditStatus == 2)
                .OrderBy(s => s.DownloadCategorySort, OrderByType.Asc)
                .Select((s, c) => new DownloadCategoryVoApi
                {
                    DownloadCategoryId = s.DownloadCategoryId,
                    DownloadCategoryGuid = s.DownloadCategoryGuid,
                    DownloadCategoryParentGuid = s.DownloadCategoryParentGuid,
                    DownloadCategoryAncestralGuid = s.DownloadCategoryAncestralGuid,
                    DownloadCategoryName = s.DownloadCategoryName,
                    DownloadCategorySort = s.DownloadCategorySort,
                    DownloadCategoryAuditStatus = s.DownloadCategoryAuditStatus,
                    DownloadCategoryAuditUserGuid = s.DownloadCategoryAuditUserGuid,
                    ParentName = c.DownloadCategoryName,
                });

            return await query.ToTreeAsync(it => it.Children, it => it.DownloadCategoryParentGuid, 0);
        }

        /// <summary>
        /// 查询下载分类列表(Api)
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<List<DownloadCategoryVoApi>> GetDownloadCategoryListApi(DownloadCategoryQueryDtoApi parm)
        {
            //开始拼装查询条件d
            var predicate = Expressionable.Create<DownloadCategory>();

            var query = _DownloadCategoryRepository
                .Queryable()
                .LeftJoin<DownloadCategory>((s, c) => s.DownloadCategoryParentGuid == c.DownloadCategoryGuid)
                .Where(predicate.ToExpression())
                .Where(s => s.DownloadCategoryAuditStatus == 2)
                .OrderBy(s => s.DownloadCategorySort, OrderByType.Asc)
                .Select((s, c) => new DownloadCategoryVoApi
                {
                    DownloadCategoryId = s.DownloadCategoryId,
                    DownloadCategoryGuid = s.DownloadCategoryGuid,
                    DownloadCategoryParentGuid = s.DownloadCategoryParentGuid,
                    DownloadCategoryAncestralGuid = s.DownloadCategoryAncestralGuid,
                    DownloadCategoryName = s.DownloadCategoryName,
                    DownloadCategorySort = s.DownloadCategorySort,
                    DownloadCategoryAuditStatus = s.DownloadCategoryAuditStatus,
                    DownloadCategoryAuditUserGuid = s.DownloadCategoryAuditUserGuid,
                    ParentName = c.DownloadCategoryName,
                });

            return await query.ToListAsync();
        }


        /// <summary>
        /// 查询下载分类详情(Api)
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public Task<string> GetDownloadCategoryDetails(DownloadCategoryDtoApi parm)
        {

            var query = _DownloadCategoryRepository
                .Queryable()
                .Where(s => s.DownloadCategoryGuid == parm.DownloadCategoryGuid)
                .Select(s => new DownloadCategoryApiDetailsVo
                {
                    DownloadCategoryId = s.DownloadCategoryId,
                    DownloadCategoryGuid = s.DownloadCategoryGuid,
                    DownloadCategoryParentGuid = s.DownloadCategoryParentGuid,
                    DownloadCategoryAncestralGuid = s.DownloadCategoryAncestralGuid,
                    DownloadCategoryName = s.DownloadCategoryName,
                    DownloadCategorySort = s.DownloadCategorySort,
                    DownloadCategoryAuditStatus = s.DownloadCategoryAuditStatus,
                    DownloadCategoryAuditUserGuid = s.DownloadCategoryAuditUserGuid,
                }).Take(1);


            return query.ToJsonAsync();
        }


        #endregion

    }
}
