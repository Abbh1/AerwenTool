using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARW.Model;
using ARW.Model.Dto.Business.Project.ProjectFiless;
using ARW.Model.Models.Business.Project.ProjectFiless;
using ARW.Model.Vo.Business.Project.ProjectFiless;

namespace ARW.Service.Business.IBusinessService.Project.ProjectFiless
{
    public interface IProjectFilesService : IBaseService<ProjectFiles>
    {
		/// <summary>
        /// 获取项目配置分页列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<PagedInfo<ProjectFilesVo>> GetProjectFilesList(ProjectFilesQueryDto parm);

		
		/// <summary>
        /// 添加或修改项目配置
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<string> AddOrUpdateProjectFiles(ProjectFiles parm);



        /// <summary>
        /// Excel导出
        /// </summary>
        Task<List<ProjectFilesVo>> HandleExportData(List<ProjectFilesVo> data);


    }
}
