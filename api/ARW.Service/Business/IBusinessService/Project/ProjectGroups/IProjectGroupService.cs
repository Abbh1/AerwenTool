using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARW.Model;
using ARW.Model.Dto.Business.Project.ProjectGroups;
using ARW.Model.Models.Business.Project.ProjectGroups;
using ARW.Model.Vo.Business.Project.ProjectGroups;

namespace ARW.Service.Business.IBusinessService.Project.ProjectGroups
{
    public interface IProjectGroupService : IBaseService<ProjectGroup>
    {
        /// <summary>
        /// 获取项目分组树形列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<List<ProjectGroupVo>> GetProjectGroupTreeList(ProjectGroupQueryDto parm);

        /// <summary>
        /// 获取项目分组列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<List<ProjectGroupVo>> GetProjectGroupList(ProjectGroupQueryDto parm);

		
		/// <summary>
        /// 添加或修改项目分组
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<string> AddOrUpdateProjectGroup(ProjectGroup parm);


        /// <summary>
        /// 数据导入处理
        /// </summary>
        /// <param name="shopVo"></param>
        /// <returns></returns>
        Task<ProjectGroupVo> HandleImportData(ProjectGroupVo ProjectGroupVo);


        /// <summary>
        /// Excel导入
        /// </summary>
        /// <param name="shopVo"></param>
        /// <returns></returns>
        Task<string> ImportExcel(ProjectGroup ProjectGroup,int index,bool isUpdateSupport,string user);

        /// <summary>
        /// Excel导出
        /// </summary>
        Task<List<ProjectGroupVo>> HandleExportData(List<ProjectGroupVo> data);


    }
}
