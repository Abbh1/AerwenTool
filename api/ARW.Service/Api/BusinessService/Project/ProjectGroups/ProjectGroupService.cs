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
using ARW.Repository.Business.Project.ProjectGroups;
using ARW.Service.Api.IBusinessService.Project.ProjectGroups;
using ARW.Model.Dto.Api.Project.ProjectGroups;
using ARW.Model.Models.Business.Project.ProjectGroups;
using ARW.Model.Vo.Api.Project.ProjectGroups;

namespace ARW.Service.Api.BusinessService.Project.ProjectGroups
{
    /// <summary>
    /// 项目分组接口实现类
    /// </summary>
    [AppService(ServiceType = typeof(IProjectGroupServiceApi), ServiceLifetime = LifeTime.Transient)]
    public class ProjectGroupServiceImplApi : BaseService<ProjectGroup>, IProjectGroupServiceApi
    {
        private readonly ProjectGroupRepository _ProjectGroupRepository;

        public ProjectGroupServiceImplApi(ProjectGroupRepository ProjectGroupRepository)
        {
            this._ProjectGroupRepository = ProjectGroupRepository;
        }

        #region Api接口代码


        /// <summary>
        /// 查询项目分组树形列表(Api)
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public List<ProjectGroupApiVo> GetProjectGroupTreeListApi(ProjectGroupQueryApiDto parm)
        {
            //开始拼装查询条件d
            var predicate = Expressionable.Create<ProjectGroup>();

            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.ProjectGroupName), it => it.ProjectGroupName == parm.ProjectGroupName);
            var query = _ProjectGroupRepository
                .Queryable()
                .LeftJoin<ProjectGroup>((s, c) => s.ProjectGroupParentGuid == c.ProjectGroupGuid)
                .Where(predicate.ToExpression())
                .Where(s => s.ProjectGroupCustomerGuid == parm.ToolCustomerGuid)
                .OrderBy(s => s.ProjectGroupSort, OrderByType.Asc)
                .Select((s, c) => new ProjectGroupApiVo
                {
                    ProjectGroupId = s.ProjectGroupId,
                    ProjectGroupGuid = s.ProjectGroupGuid,
                    ProjectGroupParentGuid = s.ProjectGroupParentGuid,
                    ProjectGroupAncestralGuid = s.ProjectGroupAncestralGuid,
                    ProjectGroupCustomerGuid = s.ProjectGroupCustomerGuid,
                    ProjectGroupName = s.ProjectGroupName,
                    ProjectGroupSort = s.ProjectGroupSort,
                    ParentName = c.ProjectGroupName,
                });

            return query.ToTree(it => it.Children, it => it.ProjectGroupParentGuid, 0);
        }


        /// <summary>
        /// 查询项目分组树形列表(Api)
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public List<ProjectGroupApiVo> GetProjectGroupListApi(ProjectGroupQueryApiDto parm)
        {
            //开始拼装查询条件d
            var predicate = Expressionable.Create<ProjectGroup>();

            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.ProjectGroupName), it => it.ProjectGroupName == parm.ProjectGroupName);
            var query = _ProjectGroupRepository
                .Queryable()
                .LeftJoin<ProjectGroup>((s, c) => s.ProjectGroupParentGuid == c.ProjectGroupGuid)
                .Where(predicate.ToExpression())
                .Where(s => s.ProjectGroupCustomerGuid == parm.ToolCustomerGuid)
                .OrderBy(s => s.ProjectGroupSort, OrderByType.Asc)
                .Select((s, c) => new ProjectGroupApiVo
                {
                    ProjectGroupId = s.ProjectGroupId,
                    ProjectGroupGuid = s.ProjectGroupGuid,
                    ProjectGroupParentGuid = s.ProjectGroupParentGuid,
                    ProjectGroupAncestralGuid = s.ProjectGroupAncestralGuid,
                    ProjectGroupCustomerGuid = s.ProjectGroupCustomerGuid,
                    ProjectGroupName = s.ProjectGroupName,
                    ProjectGroupSort = s.ProjectGroupSort,
                    ParentName = c.ProjectGroupName,
                });

            return query.ToList();
        }


        /// <summary>
        /// 查询项目分组详情(Api)
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public Task<string> GetProjectGroupDetails(ProjectGroupApiDto parm)
        {

            var query = _ProjectGroupRepository
                .Queryable()
                .Where(s => s.ProjectGroupGuid == parm.ProjectGroupGuid)
                .Select(s => new ProjectGroupApiDetailsVo
                {
                    ProjectGroupId = s.ProjectGroupId,
                    ProjectGroupGuid = s.ProjectGroupGuid,
                    ProjectGroupParentGuid = s.ProjectGroupParentGuid,
                    ProjectGroupAncestralGuid = s.ProjectGroupAncestralGuid,
                    ProjectGroupCustomerGuid = s.ProjectGroupCustomerGuid,
                    ProjectGroupName = s.ProjectGroupName,
                    ProjectGroupSort = s.ProjectGroupSort,
                }).Take(1);


            return query.ToJsonAsync();
        }


        #endregion

    }
}
