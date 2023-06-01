using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyanDesignDemo.Model.Business.DownLoad
{
    public class DownloadFiles : BaseModel
    {
        public int DownloadFilesId { get; set; }


        /// <summary>
        /// 描述 : 
        /// 空值 : false  
        /// </summary>
        public long DownloadFilesGuid { get; set; }


        /// <summary>
        /// 描述 :下载分类guid 
        /// 空值 : false  
        /// </summary>
        public long DownloadCategoryGuid { get; set; }


        /// <summary>
        /// 描述 :图标 
        /// 空值 : false  
        /// </summary>
        public string DownloadFilesIcon { get; set; }


        /// <summary>
        /// 描述 :名称 
        /// 空值 : false  
        public string DownloadFilesName { get; set; }


        /// <summary>
        /// 描述 :简介 
        /// 空值 : true  
        /// </summary>
        public string DownloadFilesIntro { get; set; }


        /// <summary>
        /// 描述 :下载链接 
        /// 空值 : false  
        /// </summary>
        public string DownloadFilesLink { get; set; }


        /// <summary>
        /// 描述 :附件 
        /// 空值 : true  
        /// </summary>
        public string DownloadFilesAttachment { get; set; }


        /// <summary>
        /// 描述 :大小 
        /// 空值 : false  
        /// </summary>
        public decimal DownloadFilesSize { get; set; }


        /// <summary>
        /// 描述 :下载量 
        /// 空值 : false  
        /// </summary>
        public int DownloadFilesVolume { get; set; }


        /// <summary>
        /// 描述 :审核状态 
        /// 空值 : false  
        /// </summary>
        public int DownloadFilesAuditStatus { get; set; }


        /// <summary>
        /// 描述 :审核人guid 
        /// 空值 : false  
        /// </summary>
        public long DownloadFilesAuditUserGuid { get; set; }

        public bool IsShow { get; set; }

    }
}
