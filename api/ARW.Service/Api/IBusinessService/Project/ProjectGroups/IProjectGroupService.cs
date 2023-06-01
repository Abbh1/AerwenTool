using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARW.Model;
using ARW.Model.Dto.Api.Project.ProjectGroups;
using ARW.Model.Models.Business.Project.ProjectGroups;
using ARW.Model.Vo.Api.Project.ProjectGroups;

namespace ARW.Service.Api.IBusinessService.Project.ProjectGroups
{
    public interface IProjectGroupServiceApi : IBaseService<ProjectGroup>
    {
        /// <summary>
        /// 获取项目分组树形列表(Api)
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        List<ProjectGroupApiVo> GetProjectGroupTreeListApi(ProjectGroupQueryApiDto parm);

        /// <summary>
        /// 获取项目分组列表(Api)
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        List<ProjectGroupApiVo> GetProjectGroupListApi(ProjectGroupQueryApiDto parm);

        /// <summary>
        /// 获取项目分组详情(Api)
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<string> GetProjectGroupDetails(ProjectGroupApiDto parm);

    }
}
