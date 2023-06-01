using Infrastructure.Attribute;
using Microsoft.AspNetCore.Http;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARW.Model;
using ARW.Repository;
using ARW.Repository.Business.CreateTable.BaseFiledTemplates;
using ARW.Service.Api.IBusinessService.CreateTable.BaseFiledTemplates;
using ARW.Model.Dto.Api.CreateTable.BaseFiledTemplates;
using ARW.Model.Models.Business.CreateTable.BaseFiledTemplates;
using ARW.Model.Vo.Api.CreateTable.BaseFiledTemplates;
using ARW.Model.Models.Business.ToolCustomers;

namespace ARW.Service.Api.BusinessService.CreateTable.BaseFiledTemplates
{
    /// <summary>
    /// 基础字段模板接口实现类
    /// </summary>
    [AppService(ServiceType = typeof(IBaseFiledTemplateServiceApi), ServiceLifetime = LifeTime.Transient)]
    public class BaseFiledTemplateServiceImplApi : BaseService<BaseFiledTemplate>, IBaseFiledTemplateServiceApi
    {
        private readonly BaseFiledTemplateRepository _BaseFiledTemplateRepository;

        public BaseFiledTemplateServiceImplApi(BaseFiledTemplateRepository BaseFiledTemplateRepository)
        {
            this._BaseFiledTemplateRepository = BaseFiledTemplateRepository;
        }

        #region Api接口代码


        /// <summary>
        /// 查询基础字段模板列表(Api)
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public Task<List<BaseFiledTemplateApiVo>> GetBaseFiledTemplateListApi(BaseFiledTemplateQueryApiDto parm)
        {
            //开始拼装查询条件d
            var predicate = Expressionable.Create<BaseFiledTemplate>();

            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.BaseFiledTemplateName), it => it.BaseFiledTemplateName.Contains(parm.BaseFiledTemplateName));
            var query = _BaseFiledTemplateRepository
                .Queryable()
                .Where(predicate.ToExpression())
                .Where(s => s.BaseFiledTemplateCustomerGuid == parm.ToolCustomerGuid)
                .OrderBy(s => s.Update_time, OrderByType.Desc)
                .Select(s => new BaseFiledTemplateApiVo
                {
                    BaseFiledTemplateId = s.BaseFiledTemplateId,
                    BaseFiledTemplateGuid = s.BaseFiledTemplateGuid,
                    BaseFiledTemplateCustomerGuid = s.BaseFiledTemplateCustomerGuid,
                    BaseFiledTemplateName = s.BaseFiledTemplateName,
                    BaseFiledTemplateContent = s.BaseFiledTemplateContent,
                });


            return query.ToListAsync();
        }

        /// <summary>
        /// 查询基础字段模板详情(Api)
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public Task<string> GetBaseFiledTemplateDetails(BaseFiledTemplateApiDto parm)
        {

            var query = _BaseFiledTemplateRepository
                .Queryable()
                .Where(s => s.BaseFiledTemplateGuid == parm.BaseFiledTemplateGuid)
                .Select(s => new BaseFiledTemplateApiDetailsVo
                {
                    BaseFiledTemplateId = s.BaseFiledTemplateId,
                    BaseFiledTemplateGuid = s.BaseFiledTemplateGuid,
                    BaseFiledTemplateCustomerGuid = s.BaseFiledTemplateCustomerGuid,
                    BaseFiledTemplateName = s.BaseFiledTemplateName,
                    BaseFiledTemplateContent = s.BaseFiledTemplateContent,
                }).Take(1);


            return query.ToJsonAsync();
        }


        #endregion

    }
}
