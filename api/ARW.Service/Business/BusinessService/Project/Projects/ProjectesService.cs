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
using ARW.Repository.Business.Project.Projects;
using ARW.Service.Business.IBusinessService.Project.Projects;
using ARW.Model.Dto.Business.Project.Projects;
using ARW.Model.Models.Business.Project.Projects;
using ARW.Model.Vo.Business.Project.Projects;
using ARW.Model.Models.Business.Project.ProjectGroups;
using ARW.Model.Models.Business.ToolCustomers;

namespace ARW.Service.Business.BusinessService.Project.Projects
{
    /// <summary>
    /// 项目接口实现类
    /// </summary>
    [AppService(ServiceType = typeof(IProjectesService), ServiceLifetime = LifeTime.Transient)]
    public class ProjectesServiceImpl : BaseService<Projectes>, IProjectesService
    {
        private readonly ProjectesRepository _ProjectesRepository;

        public ProjectesServiceImpl(ProjectesRepository ProjectesRepository)
        {
            this._ProjectesRepository = ProjectesRepository;
        }

        #region 业务逻辑代码


        /// <summary>
        /// 查询项目分页列表
        /// </summary>
        public Task<PagedInfo<ProjectesVo>> GetProjectesList(ProjectesQueryDto parm)
        {
            //开始拼装查询条件d
            var predicate = Expressionable.Create<Projectes>();

            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.ProjectName), it => it.ProjectName == parm.ProjectName);
            var query = _ProjectesRepository
                .Queryable()
                .Where(predicate.ToExpression())
                .LeftJoin<ProjectGroup>((s, c) => s.ProjectGroupGuid == c.ProjectGroupGuid)
                .LeftJoin<ToolCustomer>((s, c, d) => s.ProjectCustomerGuid == d.ToolCustomerGuid)
                .OrderBy(s => s.ProjectSort, OrderByType.Desc)
                .Select((s,c,d) => new ProjectesVo
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


            return query.ToPageAsync(parm);
        }

        /// <summary>
        ///  添加或修改项目
        /// </summary>
        public async Task<string> AddOrUpdateProjectes(Projectes model)
        {
            if (model.ProjectId != 0)
            {
                var response = await _ProjectesRepository.UpdateAsync(model);
                return "修改成功！";
            }
            else
            {

                var response = await _ProjectesRepository.InsertReturnSnowflakeIdAsync(model);
                return "添加成功！";
            }
        }

        #region Excel处理
        /// <summary>
        /// 数据导入处理
        /// </summary>
        public async Task<ProjectesVo> HandleImportData(ProjectesVo Projectes)
        {
            return Projectes;
        }


        /// <summary>
        /// Excel导入
        /// </summary>
        public async Task<string> ImportExcel(Projectes Projectes, int index, bool isUpdateSupport, string user)
        {
            try
            {
                // 空值判断
                // if (Projectes.ProjectId == null) throw new CustomException("项目不能为空");

                if (isUpdateSupport)
                {
                    // 判断唯一值
                    var model = await GetFirstAsync(s => s.ProjectId == Projectes.ProjectId);

                    // 如果为空就新增数据
                    if (model == null)
                    {
                        // 开启事务
                        var res = await UseTranAsync(async () =>
                        {
                            var addRes = await AddOrUpdateProjectes(Projectes);
                        });
                        var addStr = $"第 {index} 行 => 项目：【{Projectes.ProjectId}】<span style='color:#27af49'>新增成功!</span><br>";
                        return addStr;
                    }
                    else
                    {
                        // 如果有数据就进行修改
                        // 开启事务
                        await UseTranAsync(async () =>
                        {
                            Projectes.ProjectId = model.ProjectId;
                            Projectes.ProjectGuid = model.ProjectGuid;
                            Projectes.Update_by = user;
                            Projectes.Update_time = DateTime.Now;
                            var editRes = await AddOrUpdateProjectes(Projectes);
                        });
                        var editStr = $"第 {index} 行 => 项目：【{Projectes.ProjectId}】<span style='color:#e6a23c'>更新成功!</span><br>";
                        return editStr;
                    }
                }
                else
                {
                    // 开启事务
                    var res = await UseTranAsync(async () =>
                    {
                        var addRes = await AddOrUpdateProjectes(Projectes);
                    });
                    //Console.WriteLine(res.IsSuccess);
                    var addStr = $"第 {index} 行 => 项目：【{Projectes.ProjectId}】<span style='color:#27af49'>新增成功!</span><br>";
                    return addStr;
                }
            }
            catch (Exception ex)
            {
                var errorRes = $"第 {index} 行 => 项目：【{Projectes.ProjectId}】<span style='color:red'>导入失败！{ex.Message}</span><br>";
                return errorRes;
                throw;
            }
        }



        /// <summary>
        /// Excel数据导出处理
        /// </summary>
        public async Task<List<ProjectesVo>> HandleExportData(List<ProjectesVo> data)
        {
            return data;
        }

        #endregion



        #endregion

    }
}
