using Infrastructure.Attribute;
using Microsoft.AspNetCore.Http;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using ARW.Model;
using ARW.Repository;
using ARW.Repository.Business.ToolCustomers;
using ARW.Service.Business.IBusinessService.ToolCustomers;
using ARW.Model.Dto.Business.ToolCustomers;
using ARW.Model.Models.Business.ToolCustomers;
using ARW.Model.Vo.Business.ToolCustomers;

namespace ARW.Service.Business.BusinessService.ToolCustomers
{
    /// <summary>
    /// Tool客户接口实现类
    /// </summary>
    [AppService(ServiceType = typeof(IToolCustomerService), ServiceLifetime = LifeTime.Transient)]
    public class ToolCustomerServiceImpl : BaseService<ToolCustomer>, IToolCustomerService
    {
        private readonly ToolCustomerRepository _ToolCustomerRepository;

        public ToolCustomerServiceImpl(ToolCustomerRepository ToolCustomerRepository)
        {
            this._ToolCustomerRepository = ToolCustomerRepository;
        }

	#region 业务逻辑代码
	
		
		/// <summary>
        /// 查询Tool客户分页列表
        /// </summary>
		public Task<PagedInfo<ToolCustomerVo>> GetToolCustomerList(ToolCustomerQueryDto parm)
        {
            //开始拼装查询条件d
            var predicate = Expressionable.Create<ToolCustomer>();

            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.ToolCustomerName), it => it.ToolCustomerName.Contains(parm.ToolCustomerName));
            var query = _ToolCustomerRepository
                .Queryable()
                .Where(predicate.ToExpression())
                .OrderBy(s => s.Update_time,OrderByType.Desc)
                .Select(s => new ToolCustomerVo
                {
            ToolCustomerId = s.ToolCustomerId,
                  ToolCustomerGuid = s.ToolCustomerGuid,
                  ToolCustomerName = s.ToolCustomerName,
                              ToolCustomerPhoneNumber = s.ToolCustomerPhoneNumber,
                  ToolCustomerEmail = s.ToolCustomerEmail,
                                       });
                

		return query.ToPageAsync(parm);
        }

		/// <summary>
        ///  添加或修改Tool客户
        /// </summary>
        public async Task<string> AddOrUpdateToolCustomer(ToolCustomer model)
        {
            if (model.ToolCustomerId != 0)
            {
                var response = await _ToolCustomerRepository.UpdateAsync(model);
                return "修改成功！";
            }
            else
            {

                var response = await _ToolCustomerRepository.InsertReturnSnowflakeIdAsync(model);
                return "添加成功！";
            }
        }

        #region Excel处理

    
        /// <summary>
        /// Excel数据导出处理
        /// </summary>
        public async Task<List<ToolCustomerVo>> HandleExportData(List<ToolCustomerVo> data)
        {
            return data;
        }

        #endregion
	


#endregion

    }
}
