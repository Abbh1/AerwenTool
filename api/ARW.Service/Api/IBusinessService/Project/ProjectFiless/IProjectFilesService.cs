using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARW.Model;
using ARW.Model.Dto.Api.Project.ProjectFiless;
using ARW.Model.Models.Business.Project.ProjectFiless;
using ARW.Model.Vo.Api.Project.ProjectFiless;

namespace ARW.Service.Api.IBusinessService.Project.ProjectFiless
{
    public interface IProjectFilesServiceApi : IBaseService<ProjectFiles>
    {
		/// <summary>
        /// 获取项目配置分页列表(Api)
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<List<ProjectFilesApiVo>> GetProjectFilesListApi(ProjectFilesQueryApiDto parm);

		/// <summary>
        /// 获取项目配置详情(Api)
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<string> GetProjectFilesDetails(ProjectFilesApiDto parm);

    }
}
