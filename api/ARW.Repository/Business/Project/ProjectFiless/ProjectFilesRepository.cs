using System;
using Infrastructure.Attribute;
using ARW.Repository.System;
using ARW.Model.Models.Business.Project.ProjectFiless;

namespace ARW.Repository.Business.Project.ProjectFiless
{
    /// <summary>
    /// 项目配置仓储
    ///
    /// @author admin
    /// @date 2023-05-23
    /// </summary>
    [AppService(ServiceLifetime = LifeTime.Transient)]
    public class ProjectFilesRepository : BaseRepository<ProjectFiles>
    {
        #region 业务逻辑代码
        #endregion
    }
}