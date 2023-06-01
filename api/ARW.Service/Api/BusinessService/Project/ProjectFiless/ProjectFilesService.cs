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
using ARW.Repository.Business.Project.ProjectFiless;
using ARW.Service.Api.IBusinessService.Project.ProjectFiless;
using ARW.Model.Dto.Api.Project.ProjectFiless;
using ARW.Model.Models.Business.Project.ProjectFiless;
using ARW.Model.Vo.Api.Project.ProjectFiless;
using ARW.Repository.System;

namespace ARW.Service.Api.BusinessService.Project.ProjectFiless
{
    /// <summary>
    /// 项目配置接口实现类
    /// </summary>
    [AppService(ServiceType = typeof(IProjectFilesServiceApi), ServiceLifetime = LifeTime.Transient)]
    public class ProjectFilesServiceImplApi : BaseService<ProjectFiles>, IProjectFilesServiceApi
    {
        private readonly ProjectFilesRepository _ProjectFilesRepository;
        private SysDictDataRepository DictDataRepository;

        public ProjectFilesServiceImplApi(ProjectFilesRepository ProjectFilesRepository, SysDictDataRepository dictDataRepository)
        {
            this._ProjectFilesRepository = ProjectFilesRepository;
            DictDataRepository = dictDataRepository;
        }

        #region Api接口代码


        /// <summary>
        /// 查询项目配置列表(Api)
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public Task<List<ProjectFilesApiVo>> GetProjectFilesListApi(ProjectFilesQueryApiDto parm)
        {
            //开始拼装查询条件d
            var predicate = Expressionable.Create<ProjectFiles>();

            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.ProjectFilesTitle), it => it.ProjectFilesTitle.Contains(parm.ProjectFilesTitle));
            predicate = predicate.AndIF(parm.ProjectFilesOpenMethodType != null, it => it.ProjectFilesOpenMethodType == parm.ProjectFilesOpenMethodType);
            predicate = predicate.AndIF(parm.ProjectFilesFileOpenType != null, it => it.ProjectFilesFileOpenType == parm.ProjectFilesFileOpenType);
            predicate = predicate.AndIF(parm.ProjectFilesIsGit != null, it => it.ProjectFilesIsGit == parm.ProjectFilesIsGit);
            var query = _ProjectFilesRepository
                .Queryable()
                .Where(predicate.ToExpression())
                .OrderBy(s => s.Update_time, OrderByType.Desc)
                .Where(s => s.ProjectGuid == parm.ProjectGuid)
                .Where(s => s.CustomerGuid == parm.CustomerGuid)
                .Select(s => new ProjectFilesApiVo
                {
                    ProjectFilesId = s.ProjectFilesId,
                    ProjectFilesGuid = s.ProjectFilesGuid,
                    CustomerGuid = s.CustomerGuid,
                    ProjectGuid = s.ProjectGuid,
                    ProjectFilesTitle = s.ProjectFilesTitle,
                    ProjectFilesOpenMethodType = s.ProjectFilesOpenMethodType,
                    ProjectFilesOpenMethodPath = s.ProjectFilesOpenMethodPath,
                    ProjectFilesFileOpenType = s.ProjectFilesFileOpenType,
                    ProjectFilesIsOpen = s.ProjectFilesIsOpen,
                    ProjectFilesFileOpenPath = s.ProjectFilesFileOpenPath,
                    ProjectFilesIsGit = s.ProjectFilesIsGit,
                    ProjectFilesGitPath = s.ProjectFilesGitPath,
                });


            return query.ToListAsync();
        }

        /// <summary>
        /// 查询项目配置详情(Api)
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public Task<string> GetProjectFilesDetails(ProjectFilesApiDto parm)
        {

            var query = _ProjectFilesRepository
                .Queryable()
                .Where(s => s.ProjectFilesGuid == parm.ProjectFilesGuid)
                .Select(s => new ProjectFilesApiDetailsVo
                {
                    ProjectFilesId = s.ProjectFilesId,
                    ProjectFilesGuid = s.ProjectFilesGuid,
                    CustomerGuid = s.CustomerGuid,
                    ProjectGuid = s.ProjectGuid,
                    ProjectFilesTitle = s.ProjectFilesTitle,
                    ProjectFilesOpenMethodType = s.ProjectFilesOpenMethodType,
                    ProjectFilesOpenMethodPath = s.ProjectFilesOpenMethodPath,
                    ProjectFilesFileOpenType = s.ProjectFilesFileOpenType,
                    ProjectFilesFileOpenPath = s.ProjectFilesFileOpenPath,
                    ProjectFilesIsGit = s.ProjectFilesIsGit,
                    ProjectFilesGitPath = s.ProjectFilesGitPath,
                }).Take(1);


            return query.ToJsonAsync();
        }


        #endregion

    }
}
