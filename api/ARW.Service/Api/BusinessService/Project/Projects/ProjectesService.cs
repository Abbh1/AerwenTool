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
using ARW.Repository.Business.Project.Projects;
using ARW.Service.Api.IBusinessService.Project.Projects;
using ARW.Model.Dto.Api.Project.Projects;
using ARW.Model.Models.Business.Project.Projects;
using ARW.Model.Vo.Api.Project.Projects;
using ARW.Model.Models.Business.Project.ProjectGroups;
using ARW.Model.Vo.Business.Project.Projects;
using ARW.Model.Models.Business.ToolCustomers;
using ARW.Repository.Business.Project.ProjectGroups;

namespace ARW.Service.Api.BusinessService.Project.Projects
{
    /// <summary>
    /// 项目接口实现类
    /// </summary>
    [AppService(ServiceType = typeof(IProjectesServiceApi), ServiceLifetime = LifeTime.Transient)]
    public class ProjectesServiceImplApi : BaseService<Projectes>, IProjectesServiceApi
    {
        private readonly ProjectesRepository _ProjectesRepository;
        private readonly ProjectGroupRepository _ProjectGroupRepository;

        public ProjectesServiceImplApi(ProjectesRepository ProjectesRepository, ProjectGroupRepository projectGroupRepository)
        {
            this._ProjectesRepository = ProjectesRepository;
            _ProjectGroupRepository = projectGroupRepository;
        }

        #region Api接口代码


        /// <summary>
        /// 查询项目列表(Api)
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<List<ProjectesApiVo>> GetProjectesListApi(ProjectesQueryApiDto parm)
        {
            //开始拼装查询条件d
            var predicate = Expressionable.Create<Projectes>();

            if (parm.ProjectGroupGuid != 0)
            {
                var data = await _ProjectGroupRepository.GetListAsync();

                var newProjectGroups = data.FindAll(delegate (ProjectGroup projectGroup)
                {
                    string[] parentProjectGroupId = projectGroup.ProjectGroupAncestralGuid.Split(",", StringSplitOptions.RemoveEmptyEntries);
                    return parentProjectGroupId.Contains(parm.ProjectGroupGuid.ToString());
                });
                string[] projectGroupArr = newProjectGroups.Select(s => s.ProjectGroupGuid.ToString()).ToArray();
                predicate = predicate.AndIF(parm.ProjectGroupGuid != 0, s => s.ProjectGroupGuid == parm.ProjectGroupGuid || projectGroupArr.Contains(s.ProjectGroupGuid.ToString()));
            }

            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.ProjectName), s => s.ProjectName.Contains(parm.ProjectName));
            //predicate = predicate.AndIF(parm.ProjectGroupGuid != 0, s => s.ProjectGroupGuid == parm.ProjectGroupGuid);
            var query = _ProjectesRepository
                .Queryable()
                 .LeftJoin<ProjectGroup>((s, c) => s.ProjectGroupGuid == c.ProjectGroupGuid)
                .LeftJoin<ToolCustomer>((s, c, d) => s.ProjectCustomerGuid == d.ToolCustomerGuid)
                .Where(s => s.ProjectCustomerGuid == parm.ToolCustomerGuid)
                .Where(predicate.ToExpression())
                .OrderBy(s => s.ProjectSort, OrderByType.Asc)
                .Select((s, c, d) => new ProjectesApiVo
                {
                    ProjectId = s.ProjectId,
                    ProjectGuid = s.ProjectGuid,
                    ProjectCustomerGuid = s.ProjectCustomerGuid,
                    ProjectGroupGuid = s.ProjectGroupGuid,
                    ProjectGroupName = c.ProjectGroupName,
                    CustomerName = d.ToolCustomerName,
                    ProjectName = s.ProjectName,
                    ProjectImg = s.ProjectImg,
                    ProjectIntro = s.ProjectIntro,
                    ProjectSort = s.ProjectSort,
                });


            return await query.ToListAsync();
        }

        /// <summary>
        /// 查询项目详情(Api)
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public Task<string> GetProjectesDetails(ProjectesApiDto parm)
        {

            var query = _ProjectesRepository
                .Queryable()
                .Where(s => s.ProjectGuid == parm.ProjectesGuid)
                .Select(s => new ProjectesApiDetailsVo
                {
                    ProjectId = s.ProjectId,
                    ProjectGuid = s.ProjectGuid,
                    ProjectCustomerGuid = s.ProjectCustomerGuid,
                    ProjectGroupGuid = s.ProjectGroupGuid,
                    ProjectName = s.ProjectName,
                    ProjectImg = s.ProjectImg,
                    ProjectIntro = s.ProjectIntro,
                    ProjectSort = s.ProjectSort,
                }).Take(1);


            return query.ToJsonAsync();
        }


        #endregion

    }
}
