using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARW.Model;
using ARW.Model.Dto.Business.Project.Projects;
using ARW.Model.Models.Business.Project.Projects;
using ARW.Model.Vo.Business.Project.Projects;

namespace ARW.Service.Business.IBusinessService.Project.Projects
{
    public interface IProjectesService : IBaseService<Projectes>
    {
		/// <summary>
        /// 获取项目分页列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<PagedInfo<ProjectesVo>> GetProjectesList(ProjectesQueryDto parm);

		
		/// <summary>
        /// 添加或修改项目
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<string> AddOrUpdateProjectes(Projectes parm);


        /// <summary>
        /// 数据导入处理
        /// </summary>
        /// <param name="shopVo"></param>
        /// <returns></returns>
        Task<ProjectesVo> HandleImportData(ProjectesVo ProjectesVo);


        /// <summary>
        /// Excel导入
        /// </summary>
        /// <param name="shopVo"></param>
        /// <returns></returns>
        Task<string> ImportExcel(Projectes Projectes,int index,bool isUpdateSupport,string user);

        /// <summary>
        /// Excel导出
        /// </summary>
        Task<List<ProjectesVo>> HandleExportData(List<ProjectesVo> data);


    }
}
