using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARW.Model;
using ARW.Model.Dto.Api.CreateTable.BaseFiledTemplates;
using ARW.Model.Models.Business.CreateTable.BaseFiledTemplates;
using ARW.Model.Vo.Api.CreateTable.BaseFiledTemplates;

namespace ARW.Service.Api.IBusinessService.CreateTable.BaseFiledTemplates
{
    public interface IBaseFiledTemplateServiceApi : IBaseService<BaseFiledTemplate>
    {
		/// <summary>
        /// 获取基础字段模板分页列表(Api)
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<List<BaseFiledTemplateApiVo>> GetBaseFiledTemplateListApi(BaseFiledTemplateQueryApiDto parm);

		/// <summary>
        /// 获取基础字段模板详情(Api)
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<string> GetBaseFiledTemplateDetails(BaseFiledTemplateApiDto parm);

    }
}
