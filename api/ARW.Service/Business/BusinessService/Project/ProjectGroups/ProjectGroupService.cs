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
using ARW.Repository.Business.Project.ProjectGroups;
using ARW.Service.Business.IBusinessService.Project.ProjectGroups;
using ARW.Model.Dto.Business.Project.ProjectGroups;
using ARW.Model.Models.Business.Project.ProjectGroups;
using ARW.Model.Vo.Business.Project.ProjectGroups;

namespace ARW.Service.Business.BusinessService.Project.ProjectGroups
{
    /// <summary>
    /// 项目分组接口实现类
    /// </summary>
    [AppService(ServiceType = typeof(IProjectGroupService), ServiceLifetime = LifeTime.Transient)]
    public class ProjectGroupServiceImpl : BaseService<ProjectGroup>, IProjectGroupService
    {
        private readonly ProjectGroupRepository _ProjectGroupRepository;

        public ProjectGroupServiceImpl(ProjectGroupRepository ProjectGroupRepository)
        {
            this._ProjectGroupRepository = ProjectGroupRepository;
        }

        #region 业务逻辑代码


        /// <summary>
        /// 查询项目分组树形列表
        /// </summary>
        public async Task<List<ProjectGroupVo>> GetProjectGroupTreeList(ProjectGroupQueryDto parm)
        {
            //开始拼装查询条件
            var predicate = Expressionable.Create<ProjectGroup>();

            var query = _ProjectGroupRepository
                .Queryable()
                .LeftJoin<ProjectGroup>((s, c) => s.ProjectGroupParentGuid == c.ProjectGroupGuid)
                .Where(predicate.ToExpression())
                .OrderBy(s => s.ProjectGroupSort, OrderByType.Asc)
                .Select((s, c) => new ProjectGroupVo
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

            return await query.ToTreeAsync(it => it.Children, it => it.ProjectGroupParentGuid, 0);
        }


        /// <summary>
        /// 查询项目分组列表
        /// </summary>
		public Task<List<ProjectGroupVo>> GetProjectGroupList(ProjectGroupQueryDto parm)
        {
            //开始拼装查询条件d
            var predicate = Expressionable.Create<ProjectGroup>();

            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.ProjectGroupName), it => it.ProjectGroupName == parm.ProjectGroupName);
            var query = _ProjectGroupRepository
                .Queryable()
                .Where(predicate.ToExpression())
                .OrderBy(s => s.ProjectGroupSort, OrderByType.Asc)
                .Select(s => new ProjectGroupVo
                {
                    ProjectGroupId = s.ProjectGroupId,
                    ProjectGroupGuid = s.ProjectGroupGuid,
                    ProjectGroupParentGuid = s.ProjectGroupParentGuid,
                    ProjectGroupAncestralGuid = s.ProjectGroupAncestralGuid,
                    ProjectGroupCustomerGuid = s.ProjectGroupCustomerGuid,
                    ProjectGroupName = s.ProjectGroupName,
                    ProjectGroupSort = s.ProjectGroupSort,
                });


            return query.ToListAsync();
        }

        /// <summary>
        ///  添加或修改项目分组
        /// </summary>
        public async Task<string> AddOrUpdateProjectGroup(ProjectGroup model)
        {
            if (model.ProjectGroupId != 0)
            {
                var type = await _ProjectGroupRepository.GetListAsync(s => s.ProjectGroupParentGuid == model.ProjectGroupGuid);
                if (type != null)
                {
                    foreach (var item in type)
                    {
                        if (model.ProjectGroupParentGuid == item.ProjectGroupGuid) throw new CustomException("上级菜单不能选择自己的子级！");
                    }
                }
                if (model.ProjectGroupParentGuid == model.ProjectGroupGuid) throw new CustomException("上级菜单不能选择与当前菜单一样的！");
                var response = await _ProjectGroupRepository.UpdateAsync(model);
                return "修改成功！";
            }
            else
            {
                var info = _ProjectGroupRepository.GetFirst(it => it.ProjectGroupGuid == model.ProjectGroupParentGuid);
                model.ProjectGroupAncestralGuid = "0";
                if (info != null) model.ProjectGroupAncestralGuid = info.ProjectGroupAncestralGuid + "," + model.ProjectGroupParentGuid;

                var response = await _ProjectGroupRepository.InsertReturnSnowflakeIdAsync(model);
                return "添加成功！";
            }
        }

        #region Excel处理
        /// <summary>
        /// 数据导入处理
        /// </summary>
        public async Task<ProjectGroupVo> HandleImportData(ProjectGroupVo ProjectGroup)
        {
            return ProjectGroup;
        }


        /// <summary>
        /// Excel导入
        /// </summary>
        public async Task<string> ImportExcel(ProjectGroup ProjectGroup, int index, bool isUpdateSupport, string user)
        {
            try
            {
                // 空值判断
                // if (ProjectGroup.ProjectGroupId == null) throw new CustomException("项目分组不能为空");

                if (isUpdateSupport)
                {
                    // 判断唯一值
                    var model = await GetFirstAsync(s => s.ProjectGroupId == ProjectGroup.ProjectGroupId);

                    // 如果为空就新增数据
                    if (model == null)
                    {
                        // 开启事务
                        var res = await UseTranAsync(async () =>
                        {
                            var addRes = await AddOrUpdateProjectGroup(ProjectGroup);
                        });
                        var addStr = $"第 {index} 行 => 项目分组：【{ProjectGroup.ProjectGroupId}】<span style='color:#27af49'>新增成功!</span><br>";
                        return addStr;
                    }
                    else
                    {
                        // 如果有数据就进行修改
                        // 开启事务
                        await UseTranAsync(async () =>
                        {
                            ProjectGroup.ProjectGroupId = model.ProjectGroupId;
                            ProjectGroup.ProjectGroupGuid = model.ProjectGroupGuid;
                            ProjectGroup.Update_by = user;
                            ProjectGroup.Update_time = DateTime.Now;
                            var editRes = await AddOrUpdateProjectGroup(ProjectGroup);
                        });
                        var editStr = $"第 {index} 行 => 项目分组：【{ProjectGroup.ProjectGroupId}】<span style='color:#e6a23c'>更新成功!</span><br>";
                        return editStr;
                    }
                }
                else
                {
                    // 开启事务
                    var res = await UseTranAsync(async () =>
                    {
                        var addRes = await AddOrUpdateProjectGroup(ProjectGroup);
                    });
                    //Console.WriteLine(res.IsSuccess);
                    var addStr = $"第 {index} 行 => 项目分组：【{ProjectGroup.ProjectGroupId}】<span style='color:#27af49'>新增成功!</span><br>";
                    return addStr;
                }
            }
            catch (Exception ex)
            {
                var errorRes = $"第 {index} 行 => 项目分组：【{ProjectGroup.ProjectGroupId}】<span style='color:red'>导入失败！{ex.Message}</span><br>";
                return errorRes;
                throw;
            }
        }



        /// <summary>
        /// Excel数据导出处理
        /// </summary>
        public async Task<List<ProjectGroupVo>> HandleExportData(List<ProjectGroupVo> data)
        {
            return data;
        }


        #endregion



        #endregion

    }
}
