using System;
using Infrastructure.Attribute;
using ARW.Repository.System;
using ARW.Model.Models.Business.Download.DownloadCategorys;

namespace ARW.Repository.Business.Download.DownloadCategorys
{
    /// <summary>
    /// 下载分类仓储
    ///
    /// @author admin
    /// @date 2023-05-25
    /// </summary>
    [AppService(ServiceLifetime = LifeTime.Transient)]
    public class DownloadCategoryRepository : BaseRepository<DownloadCategory>
    {
        #region 业务逻辑代码
        #endregion
    }
}