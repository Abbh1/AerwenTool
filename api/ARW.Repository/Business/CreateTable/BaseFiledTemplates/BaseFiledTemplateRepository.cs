using System;
using Infrastructure.Attribute;
using ARW.Repository.System;
using ARW.Model.Models.Business.CreateTable.BaseFiledTemplates;

namespace ARW.Repository.Business.CreateTable.BaseFiledTemplates
{
    /// <summary>
    /// 基础字段模板仓储
    ///
    /// @author admin
    /// @date 2023-05-26
    /// </summary>
    [AppService(ServiceLifetime = LifeTime.Transient)]
    public class BaseFiledTemplateRepository : BaseRepository<BaseFiledTemplate>
    {
        #region 业务逻辑代码
        #endregion
    }
}