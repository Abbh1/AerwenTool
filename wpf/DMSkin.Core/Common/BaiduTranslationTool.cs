using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DMSkin.Core.Common
{
    /// <summary>
    /// 百度翻译AIP，你需要从<see href ="https://fanyi-api.baidu.com/api/trans/product/desktop?req=developer"/>获取密钥
    /// <br/>这是免费且无需审核的。
    /// </summary>
    public class BaiduTranslation
    {
        public const string 自动检测 = "auto";
        public const string 中文 = "zh";
        public const string 英语 = "en";
        public const string 粤语 = "yue";
        public const string 文言文 = "wyw";
        public const string 日语 = "jp";
        public const string 韩语 = "kor";
        public const string 法语 = "fra";
        public const string 西班牙语 = "spa";
        public const string 泰语 = "th";
        public const string 阿拉伯语 = "ara";
        public const string 俄语 = "ru";
        public const string 葡萄牙语 = "pt";
        public const string 德语 = "de";
        public const string 意大利语 = "it";
        public const string 希腊语 = "el";
        public const string 荷兰语 = "nl";
        public const string 波兰语 = "pl";
        public const string 保加利亚语 = "bul";
        public const string 爱沙尼亚语 = "est";
        public const string 丹麦语 = "dan";
        public const string 芬兰语 = "fin";
        public const string 捷克语 = "cs";
        public const string 罗马尼亚语 = "rom";
        public const string 斯洛文尼亚语 = "slo";
        public const string 瑞典语 = "swe";
        public const string 匈牙利语 = "hu";
        public const string 繁体中文 = "cht";
        public const string 越南语 = "vie";

        private readonly string ID;
        private readonly string Key;
        private readonly int delay;
        private DateTime last;
        private const string API_URL = "http://api.fanyi.baidu.com/api/trans/vip/translate";

        /// <summary>
        /// 实例化一个对接百度翻译的对象。你需要从这里获取使用权限：<see href ="https://fanyi-api.baidu.com/api/trans/product/desktop?req=developer"/>
        /// </summary>
        /// <param name="APPID">百度提供的ID</param>
        /// <param name="密钥">百度提供的密钥</param>
        /// <param name="delay">每次请求前的延迟。默认为最低档的1000毫秒</param>
        public BaiduTranslation(string APPID, string 密钥, int delay = 1000)
        {
            this.delay = delay;
            ID = APPID;
            Key = 密钥;
            Key = 密钥;
        }



        /// <summary>
        /// 发送一个翻译请求
        /// </summary>
        /// <param name="source">源字符串，上限5000字符</param>
        /// <param name="from">源语种</param>
        /// <param name="to">目标语种</param>
        /// <returns>序列化后的输出</returns>
        /// TransJson_Result
        public async Task<TransJson_Result> GetTranslation(string source, string from = 中文, string to = 英语)
        {
            try
            {
                MD5 md5 = MD5.Create();
                byte[] md5byte = md5.ComputeHash(Encoding.UTF8.GetBytes(ID + source + last.Millisecond + Key));
                string md5string = new string(md5byte.SelectMany(s => $"{s:x2}").ToArray());
                //string utf8 = System.Net.Http.HttpUtility.UrlEncode(source, Encoding.UTF8);
                string utf8 = System.Web.HttpUtility.UrlEncode(source, Encoding.UTF8);
                string url = $"{API_URL}?q={utf8}&from={from}&to={to}&appid={ID}&salt={last.Millisecond}&sign={md5string}";

                var time = DateTime.Now - last;
                last = DateTime.Now;
                if (time.TotalMilliseconds < delay)
                    await Task.Delay(delay - (int)time.TotalMilliseconds);

                HttpClient http = new HttpClient();

                var responseJson = http.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
                //return responseJson;
                //var result = responseJson.FromJson<TransJson_Result>(url);

                var res = JsonConvert.DeserializeObject<TransJson_Result>(responseJson);

                return res;
                //JsonSerializer.Deserialize<TransJson_Result>(responseJson);
                //return await http.GetFromJsonAsync<TransJson_Result>(url);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }

    public class TransJson_Result
    {
        /// <summary>
        /// 错误码 
        /// <list type="table">
        ///		<item>
        ///			<term>52000</term>
        ///			<description>成功。</description>
        ///		</item>
        ///		<item>
        ///			<term>52001</term>
        ///			<description>请求超时。请重试</description>
        ///		</item>
        ///		<item>
        ///			<term>52002</term>
        ///			<description>系统错误。请重试</description>
        ///		</item>
        ///		<item>
        ///			<term>52003</term>
        ///			<description>未授权用户。请检查appid是否正确或者服务是否开通</description>
        ///		</item>
        ///		<item>
        ///			<term>54000</term>
        ///			<description>必填参数为空。请检查是否少传参数</description>
        ///		</item>
        ///		<item>
        ///			<term>54001</term>
        ///			<description>签名错误。请检查您的签名生成方法</description>
        ///		</item>
        ///		<item>
        ///			<term>54003</term>
        ///			<description>访问频率受限。请降低您的调用频率，或进行身份认证后切换为高级版/尊享版</description>
        ///		</item>
        ///		<item>
        ///			<term>54004</term>
        ///			<description>账户余额不足。请前往<see href ="https://api.fanyi.baidu.com/api/trans/product/desktop">管理控制台</see>为账户充值</description>
        ///		</item>
        ///		<item>
        ///			<term>54005</term>
        ///			<description>长query请求频繁。请降低长query的发送频率，3s后再试</description>
        ///		</item>
        ///		<item>
        ///			<term>58000</term>
        ///			<description>客户端IP非法。检查个人资料里填写的IP地址是否正确，可前往<see href ="https://api.fanyi.baidu.com/access/0/3">开发者信息-基本信息</see> 修改 </description>
        ///		</item>
        ///		<item>
        ///			<term>58001</term>
        ///			<description>译文语言方向不支持。检查译文语言是否在语言列表里</description>
        ///		</item>
        ///		<item>
        ///			<term>58002</term>
        ///			<description>服务当前已关闭。请前往<see href ="https://api.fanyi.baidu.com/choose">管理控制台</see>开启服务</description>
        ///		</item>
        ///		<item>
        ///			<term>90107</term>
        ///			<description>认证未通过或未生效。请前往<see href ="https://api.fanyi.baidu.com/myIdentify">我的认证</see>查看认证进度</description>
        ///		</item>
        ///</list>
        ///</summary>
        public string error_code { get; set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        public string error_msg { get; set; }
        /// <summary>
        /// 源语种
        /// </summary>
        public string from { get; set; }
        /// <summary>
        /// 目标语种
        /// </summary> 
        public string to { get; set; }
        public Trans_Result[] trans_result { get; set; }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            foreach (var item in trans_result)
            {
                yield return new KeyValuePair<string, string>(item.src, item.dst);
            }
        }
    }

    public class Trans_Result
    {
        /// <summary>
        /// 源字符串
        /// </summary>
        public string src { get; set; }
        /// <summary>
        /// 翻译后的字符串
        /// </summary>
        public string dst { get; set; }
    }


}
