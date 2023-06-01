using System;
using Infrastructure.Attribute;
using ARW.Repository.System;
using ARW.Model.Models.Business.ToolCustomers;

namespace ARW.Repository.Business.ToolCustomers
{
    /// <summary>
    /// Tool客户仓储
    ///
    /// @author admin
    /// @date 2023-05-22
    /// </summary>
    [AppService(ServiceLifetime = LifeTime.Transient)]
    public class ToolCustomerRepository : BaseRepository<ToolCustomer>
    {
        #region 业务逻辑代码
        #endregion
    }
}