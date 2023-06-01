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
using ARW.Repository.Business.Project.ProjectFiless;
using ARW.Service.Business.IBusinessService.Project.ProjectFiless;
using ARW.Model.Dto.Business.Project.ProjectFiless;
using ARW.Model.Models.Business.Project.ProjectFiless;
using ARW.Model.Vo.Business.Project.ProjectFiless;

namespace ARW.Service.Business.BusinessService.Project.ProjectFiless
{
    /// <summary>
    /// 项目配置接口实现类
    /// </summary>
    [AppService(ServiceType = typeof(IProjectFilesService), ServiceLifetime = LifeTime.Transient)]
    public class ProjectFilesServiceImpl : BaseService<ProjectFiles>, IProjectFilesService
    {
        private readonly ProjectFilesRepository _ProjectFilesRepository;

        public ProjectFilesServiceImpl(ProjectFilesRepository ProjectFilesRepository)
        {
            this._ProjectFilesRepository = ProjectFilesRepository;
        }

        #region 业务逻辑代码


        /// <summary>
        /// 查询项目配置分页列表
        /// </summary>
        public Task<PagedInfo<ProjectFilesVo>> GetProjectFilesList(ProjectFilesQueryDto parm)
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
                .Select(s => new ProjectFilesVo
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
                    ProjectFilesIsOpen = s.ProjectFilesIsOpen
                });


            return query.ToPageAsync(parm);
        }

        /// <summary>
        ///  添加或修改项目配置
        /// </summary>
        public async Task<string> AddOrUpdateProjectFiles(ProjectFiles model)
        {
            if (model.ProjectFilesId != 0)
            {
                var response = await _ProjectFilesRepository.UpdateAsync(model);
                return "修改成功！";
            }
            else
            {

                var response = await _ProjectFilesRepository.InsertReturnSnowflakeIdAsync(model);
                return "添加成功！";
            }
        }

        #region Excel处理


        /// <summary>
        /// Excel数据导出处理
        /// </summary>
        public async Task<List<ProjectFilesVo>> HandleExportData(List<ProjectFilesVo> data)
        {
            return data;
        }

        #endregion



        #endregion

    }
}
