using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARW.Model;
using ARW.Model.Dto.Api.Project.Projects;
using ARW.Model.Models.Business.Project.Projects;
using ARW.Model.Vo.Api.Project.Projects;

namespace ARW.Service.Api.IBusinessService.Project.Projects
{
    public interface IProjectesServiceApi : IBaseService<Projectes>
    {
		/// <summary>
        /// 获取项目分页列表(Api)
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<List<ProjectesApiVo>> GetProjectesListApi(ProjectesQueryApiDto parm);

		/// <summary>
        /// 获取项目详情(Api)
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<string> GetProjectesDetails(ProjectesApiDto parm);

    }
}
