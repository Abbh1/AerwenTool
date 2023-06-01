using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARW.Model;
using ARW.Model.Dto.Business.CreateTable.BaseFiledTemplates;
using ARW.Model.Models.Business.CreateTable.BaseFiledTemplates;
using ARW.Model.Vo.Business.CreateTable.BaseFiledTemplates;

namespace ARW.Service.Business.IBusinessService.CreateTable.BaseFiledTemplates
{
    public interface IBaseFiledTemplateService : IBaseService<BaseFiledTemplate>
    {
		/// <summary>
        /// 获取基础字段模板分页列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<PagedInfo<BaseFiledTemplateVo>> GetBaseFiledTemplateList(BaseFiledTemplateQueryDto parm);

		
		/// <summary>
        /// 添加或修改基础字段模板
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<string> AddOrUpdateBaseFiledTemplate(BaseFiledTemplate parm);



        /// <summary>
        /// Excel导出
        /// </summary>
        Task<List<BaseFiledTemplateVo>> HandleExportData(List<BaseFiledTemplateVo> data);


    }
}
