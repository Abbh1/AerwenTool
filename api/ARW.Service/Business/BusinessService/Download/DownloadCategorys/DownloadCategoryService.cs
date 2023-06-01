using Infrastructure.Attribute;
using Microsoft.AspNetCore.Http;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using ARW.Model;
using ARW.Repository;
using ARW.Repository.Business.Download.DownloadCategorys;
using ARW.Service.Business.IBusinessService.Download.DownloadCategorys;
using ARW.Model.Dto.Business.Download.DownloadCategorys;
using ARW.Model.Models.Business.Download.DownloadCategorys;
using ARW.Model.Vo.Business.Download.DownloadCategorys;

namespace ARW.Service.Business.BusinessService.Download.DownloadCategorys
{
    /// <summary>
    /// 下载分类接口实现类
    /// </summary>
    [AppService(ServiceType = typeof(IDownloadCategoryService), ServiceLifetime = LifeTime.Transient)]
    public class DownloadCategoryServiceImpl : BaseService<DownloadCategory>, IDownloadCategoryService
    {
        private readonly DownloadCategoryRepository _DownloadCategoryRepository;

        public DownloadCategoryServiceImpl(DownloadCategoryRepository DownloadCategoryRepository)
        {
            this._DownloadCategoryRepository = DownloadCategoryRepository;
        }

        #region 业务逻辑代码


        /// <summary>
        /// 查询下载分类树形列表
        /// </summary>
        public async Task<List<DownloadCategoryVo>> GetDownloadCategoryTreeList(DownloadCategoryQueryDto parm)
        {
            //开始拼装查询条件
            var predicate = Expressionable.Create<DownloadCategory>();

            predicate = predicate.AndIF(parm.DownloadCategoryAuditStatus != null, s => s.DownloadCategoryAuditStatus == parm.DownloadCategoryAuditStatus);
            var query = _DownloadCategoryRepository
                .Queryable()
                .LeftJoin<DownloadCategory>((s, c) => s.DownloadCategoryParentGuid == c.DownloadCategoryGuid)
                .Where(predicate.ToExpression())
                .OrderBy(s => s.DownloadCategorySort, OrderByType.Asc)
                .Select((s, c) => new DownloadCategoryVo
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
        /// 查询下载分类列表
        /// </summary>
		public Task<List<DownloadCategoryVo>> GetDownloadCategoryList(DownloadCategoryQueryDto parm)
        {
            //开始拼装查询条件d
            var predicate = Expressionable.Create<DownloadCategory>();

            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.DownloadCategoryName), s => s.DownloadCategoryName.Contains(parm.DownloadCategoryName));
            predicate = predicate.AndIF(parm.DownloadCategoryAuditStatus != null, s => s.DownloadCategoryAuditStatus == parm.DownloadCategoryAuditStatus);
            var query = _DownloadCategoryRepository
                .Queryable()
                .Where(predicate.ToExpression())
                .OrderBy(s => s.DownloadCategorySort, OrderByType.Asc)
                .Select(s => new DownloadCategoryVo
                {
                    DownloadCategoryId = s.DownloadCategoryId,
                    DownloadCategoryGuid = s.DownloadCategoryGuid,
                    DownloadCategoryParentGuid = s.DownloadCategoryParentGuid,
                    DownloadCategoryAncestralGuid = s.DownloadCategoryAncestralGuid,
                    DownloadCategoryName = s.DownloadCategoryName,
                    DownloadCategorySort = s.DownloadCategorySort,
                    DownloadCategoryAuditStatus = s.DownloadCategoryAuditStatus,
                    DownloadCategoryAuditUserGuid = s.DownloadCategoryAuditUserGuid,
                });


            return query.ToListAsync();
        }

        /// <summary>
        ///  添加或修改下载分类
        /// </summary>
        public async Task<string> AddOrUpdateDownloadCategory(DownloadCategory model)
        {
            if (model.DownloadCategoryId != 0)
            {
                var type = await _DownloadCategoryRepository.GetListAsync(s => s.DownloadCategoryParentGuid == model.DownloadCategoryGuid);
                if (type != null)
                {
                    foreach (var item in type)
                    {
                        if (model.DownloadCategoryParentGuid == item.DownloadCategoryGuid) throw new CustomException("上级菜单不能选择自己的子级！");
                    }
                }
                if (model.DownloadCategoryParentGuid == model.DownloadCategoryGuid) throw new CustomException("上级菜单不能选择与当前菜单一样的！");

                var response = await _DownloadCategoryRepository.UpdateAsync(model);
                return "修改成功！";
            }
            else
            {
                var info = _DownloadCategoryRepository.GetFirst(it => it.DownloadCategoryGuid == model.DownloadCategoryParentGuid);
                model.DownloadCategoryAncestralGuid = "0";
                if (info != null) model.DownloadCategoryAncestralGuid = info.DownloadCategoryAncestralGuid + "," + model.DownloadCategoryParentGuid;

                //model.DownloadCategoryAuditStatus = 2;
                //model.DownloadCategoryAuditUserGuid = 1;
                var response = await _DownloadCategoryRepository.InsertReturnSnowflakeIdAsync(model);
                return "添加成功！";
            }
        }

        #region Excel处理


        /// <summary>
        /// Excel数据导出处理
        /// </summary>
        public async Task<List<DownloadCategoryVo>> HandleExportData(List<DownloadCategoryVo> data)
        {
            return data;
        }

        #endregion


        /// <summary>
        /// 审核
        /// </summary>
        public async Task<string> Audit(int id, int status, long userGuid)
        {
            try
            {
                var res = await _DownloadCategoryRepository.GetFirstAsync(s => s.DownloadCategoryId == id);
                await UseTranAsync(async () =>
                {
                    await _DownloadCategoryRepository.UpdateAsync(f => new DownloadCategory { DownloadCategoryAuditStatus = status, DownloadCategoryAuditUserGuid = userGuid, Update_time = DateTime.Now, Update_by = userGuid.ToString() }, s => s.DownloadCategoryId == id);
                });
                if (res.DownloadCategoryAuditStatus == 2)
                {
                    var errorRes = $"下载分类：【{res.DownloadCategoryName}】<span style='color:red'>已通过审核！</span><br>";
                    return errorRes;
                }
                if (res.DownloadCategoryAuditStatus == 3)
                {
                    var errorRes = $"下载分类：【{res.DownloadCategoryName}】<span style='color:red'>已被驳回！</span><br>";
                    return errorRes;
                }
                else if (res.DownloadCategoryAuditStatus == 1)
                {
                    var addStr = $"下载分类：【{res.DownloadCategoryName}】<span style='color:#27af49'>审核通过!</span><br>";
                    return addStr;
                }
                return "";
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

    }
}
