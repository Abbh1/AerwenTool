using System;
using Infrastructure.Attribute;
using ARW.Repository.System;
using ARW.Model.Models.Business.Download.DownloadFiless;

namespace ARW.Repository.Business.Download.DownloadFiless
{
    /// <summary>
    /// 下载文件仓储
    ///
    /// @author admin
    /// @date 2023-05-28
    /// </summary>
    [AppService(ServiceLifetime = LifeTime.Transient)]
    public class DownloadFilesRepository : BaseRepository<DownloadFiles>
    {
        #region 业务逻辑代码
        #endregion
    }
}