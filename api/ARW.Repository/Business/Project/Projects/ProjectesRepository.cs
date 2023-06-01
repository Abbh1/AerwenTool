using System;
using Infrastructure.Attribute;
using ARW.Repository.System;
using ARW.Model.Models.Business.Project.Projects;

namespace ARW.Repository.Business.Project.Projects
{
    /// <summary>
    /// 项目仓储
    ///
    /// @author admin
    /// @date 2023-05-19
    /// </summary>
    [AppService(ServiceLifetime = LifeTime.Transient)]
    public class ProjectesRepository : BaseRepository<Projectes>
    {
        #region 业务逻辑代码
        #endregion
    }
}