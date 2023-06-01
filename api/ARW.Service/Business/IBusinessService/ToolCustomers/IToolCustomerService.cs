using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARW.Model;
using ARW.Model.Dto.Business.ToolCustomers;
using ARW.Model.Models.Business.ToolCustomers;
using ARW.Model.Vo.Business.ToolCustomers;

namespace ARW.Service.Business.IBusinessService.ToolCustomers
{
    public interface IToolCustomerService : IBaseService<ToolCustomer>
    {
		/// <summary>
        /// 获取Tool客户分页列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<PagedInfo<ToolCustomerVo>> GetToolCustomerList(ToolCustomerQueryDto parm);

		
		/// <summary>
        /// 添加或修改Tool客户
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<string> AddOrUpdateToolCustomer(ToolCustomer parm);



        /// <summary>
        /// Excel导出
        /// </summary>
        Task<List<ToolCustomerVo>> HandleExportData(List<ToolCustomerVo> data);


    }
}
