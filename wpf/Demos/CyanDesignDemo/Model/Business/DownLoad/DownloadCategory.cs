using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyanDesignDemo.Model.Business.DownLoad
{
    public class DownloadCategory : BaseModel
    {
        public int DownloadCategoryId { get; set; }


        /// <summary>
        /// 描述 : 
        /// 空值 : false  
        /// </summary>
        public long DownloadCategoryGuid { get; set; }


        /// <summary>
        /// 描述 :父级guid 
        /// 空值 : false  
        /// </summary>
        public long DownloadCategoryParentGuid { get; set; }


        /// <summary>
        /// 描述 :祖级guid 
        /// 空值 : true  
        /// </summary>
        public string DownloadCategoryAncestralGuid { get; set; }


        /// <summary>
        /// 描述 :名称 
        /// 空值 : false  
        /// </summary>
        public string DownloadCategoryName { get; set; }


        /// <summary>
        /// 描述 :排序 
        /// 空值 : false  
        /// </summary>
        public int DownloadCategorySort { get; set; }


        /// <summary>
        /// 描述 :审核状态 
        /// 空值 : true  
        /// </summary>
        public int? DownloadCategoryAuditStatus { get; set; }


        /// <summary>
        /// 描述 :审核人guid 
        /// 空值 : true  
        /// </summary>
        public long? DownloadCategoryAuditUserGuid { get; set; }



        public List<DownloadCategory> Children { get; set; }

        public bool IsShow { get; set; }
    }
}
