using System;
using Infrastructure.Attribute;
using ARW.Repository.System;
using ARW.Model.Models.Business.Project.ProjectGroups;

namespace ARW.Repository.Business.Project.ProjectGroups
{
    /// <summary>
    /// 项目分组仓储
    ///
    /// @author admin
    /// @date 2023-05-19
    /// </summary>
    [AppService(ServiceLifetime = LifeTime.Transient)]
    public class ProjectGroupRepository : BaseRepository<ProjectGroup>
    {
        #region 业务逻辑代码
        #endregion
    }
}