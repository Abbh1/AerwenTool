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
using ARW.Repository.Business.Download.DownloadFiless;
using ARW.Service.Business.IBusinessService.Download.DownloadFiless;
using ARW.Model.Dto.Business.Download.DownloadFiless;
using ARW.Model.Models.Business.Download.DownloadFiless;
using ARW.Model.Vo.Business.Download.DownloadFiless;

namespace ARW.Service.Business.BusinessService.Download.DownloadFiless
{
    /// <summary>
    /// 下载文件接口实现类
    /// </summary>
    [AppService(ServiceType = typeof(IDownloadFilesService), ServiceLifetime = LifeTime.Transient)]
    public class DownloadFilesServiceImpl : BaseService<DownloadFiles>, IDownloadFilesService
    {
        private readonly DownloadFilesRepository _DownloadFilesRepository;

        public DownloadFilesServiceImpl(DownloadFilesRepository DownloadFilesRepository)
        {
            this._DownloadFilesRepository = DownloadFilesRepository;
        }

	#region 业务逻辑代码
	
		
		/// <summary>
        /// 查询下载文件分页列表
        /// </summary>
		public Task<PagedInfo<DownloadFilesVo>> GetDownloadFilesList(DownloadFilesQueryDto parm)
        {
            //开始拼装查询条件d
            var predicate = Expressionable.Create<DownloadFiles>();

            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.DownloadFilesName), it => it.DownloadFilesName.Contains(parm.DownloadFilesName));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.DownloadFilesLink), it => it.DownloadFilesLink == parm.DownloadFilesLink);
            predicate = predicate.AndIF(parm.DownloadFilesAuditStatus != null, it => it.DownloadFilesAuditStatus == parm.DownloadFilesAuditStatus);
            var query = _DownloadFilesRepository
                .Queryable()
                .Where(predicate.ToExpression())
                .OrderBy(s => s.Update_time,OrderByType.Desc)
                .Select(s => new DownloadFilesVo
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
                                       });
                

		return query.ToPageAsync(parm);
        }

		/// <summary>
        ///  添加或修改下载文件
        /// </summary>
        public async Task<string> AddOrUpdateDownloadFiles(DownloadFiles model)
        {
            if (model.DownloadFilesId != 0)
            {
                var response = await _DownloadFilesRepository.UpdateAsync(model);
                return "修改成功！";
            }
            else
            {

                var response = await _DownloadFilesRepository.InsertReturnSnowflakeIdAsync(model);
                return "添加成功！";
            }
        }

        #region Excel处理

    
        /// <summary>
        /// Excel数据导出处理
        /// </summary>
        public async Task<List<DownloadFilesVo>> HandleExportData(List<DownloadFilesVo> data)
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
                var res = await _DownloadFilesRepository.GetFirstAsync(s => s.DownloadFilesId == id);
                await UseTranAsync(async () =>
                    {
                        await _DownloadFilesRepository.UpdateAsync(f => new DownloadFiles { DownloadFilesAuditStatus = status, DownloadFilesAuditUserGuid = userGuid, Update_time = DateTime.Now, Update_by = userGuid.ToString() }, s => s.DownloadFilesId == id);
                    });
                if (res.DownloadFilesAuditStatus == 2)
                {
                    var errorRes = $"下载文件：【{res.DownloadFilesName}】<span style='color:red'>已通过审核！</span><br>";
                    return errorRes;
                }
                if (res.DownloadFilesAuditStatus == 3)
                {
                    var errorRes = $"下载文件：【{res.DownloadFilesName}】<span style='color:red'>已被驳回！</span><br>";
                    return errorRes;
                }
                else if (res.DownloadFilesAuditStatus == 1)
                {
                    var addStr = $"下载文件：【{res.DownloadFilesName}】<span style='color:#27af49'>审核通过!</span><br>";
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
