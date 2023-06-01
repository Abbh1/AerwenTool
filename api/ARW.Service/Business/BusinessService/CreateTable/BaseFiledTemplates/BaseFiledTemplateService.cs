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
using ARW.Repository.Business.CreateTable.BaseFiledTemplates;
using ARW.Service.Business.IBusinessService.CreateTable.BaseFiledTemplates;
using ARW.Model.Dto.Business.CreateTable.BaseFiledTemplates;
using ARW.Model.Models.Business.CreateTable.BaseFiledTemplates;
using ARW.Model.Vo.Business.CreateTable.BaseFiledTemplates;
using ARW.Model.Models.Business.ToolCustomers;

namespace ARW.Service.Business.BusinessService.CreateTable.BaseFiledTemplates
{
    /// <summary>
    /// 基础字段模板接口实现类
    /// </summary>
    [AppService(ServiceType = typeof(IBaseFiledTemplateService), ServiceLifetime = LifeTime.Transient)]
    public class BaseFiledTemplateServiceImpl : BaseService<BaseFiledTemplate>, IBaseFiledTemplateService
    {
        private readonly BaseFiledTemplateRepository _BaseFiledTemplateRepository;

        public BaseFiledTemplateServiceImpl(BaseFiledTemplateRepository BaseFiledTemplateRepository)
        {
            this._BaseFiledTemplateRepository = BaseFiledTemplateRepository;
        }

        #region 业务逻辑代码


        /// <summary>
        /// 查询基础字段模板分页列表
        /// </summary>
        public Task<PagedInfo<BaseFiledTemplateVo>> GetBaseFiledTemplateList(BaseFiledTemplateQueryDto parm)
        {
            //开始拼装查询条件d
            var predicate = Expressionable.Create<BaseFiledTemplate>();

            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.BaseFiledTemplateName), it => it.BaseFiledTemplateName.Contains(parm.BaseFiledTemplateName));
            var query = _BaseFiledTemplateRepository
                .Queryable()
                .LeftJoin<ToolCustomer>((s, c) => s.BaseFiledTemplateCustomerGuid == c.ToolCustomerGuid)
                .Where(predicate.ToExpression())
                .OrderBy(s => s.Update_time, OrderByType.Desc)
                .Select((s, c) => new BaseFiledTemplateVo
                {
                    BaseFiledTemplateId = s.BaseFiledTemplateId,
                    BaseFiledTemplateGuid = s.BaseFiledTemplateGuid,
                    BaseFiledTemplateCustomerGuid = s.BaseFiledTemplateCustomerGuid,
                    BaseFiledTemplateCustomerName = c.ToolCustomerName,
                    BaseFiledTemplateName = s.BaseFiledTemplateName,
                    BaseFiledTemplateContent = s.BaseFiledTemplateContent,
                });


            return query.ToPageAsync(parm);
        }

        /// <summary>
        ///  添加或修改基础字段模板
        /// </summary>
        public async Task<string> AddOrUpdateBaseFiledTemplate(BaseFiledTemplate model)
        {
            if (model.BaseFiledTemplateId != 0)
            {
                var response = await _BaseFiledTemplateRepository.UpdateAsync(model);
                return "修改成功！";
            }
            else
            {

                var response = await _BaseFiledTemplateRepository.InsertReturnSnowflakeIdAsync(model);
                return "添加成功！";
            }
        }

        #region Excel处理


        /// <summary>
        /// Excel数据导出处理
        /// </summary>
        public async Task<List<BaseFiledTemplateVo>> HandleExportData(List<BaseFiledTemplateVo> data)
        {
            return data;
        }

        #endregion



        #endregion

    }
}
