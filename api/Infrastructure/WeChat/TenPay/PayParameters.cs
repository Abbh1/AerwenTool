using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.WeChat.TenPay
{
    public class PayParameters
    {
        /// <summary>
        /// 小程序ID
        /// </summary>
        public string appid { get { return "wx5e6ee16752b633c4"; } }
        /// <summary>
        /// 附加数据，在查询API和支付通知中原样返回，可作为自定义参数使用
        /// </summary>
        public string attach { get { return "支付测试"; } }
        /// <summary>
        /// 商户号
        /// </summary>
        public string mchid { get { return "1632347502"; } }
        /// <summary>
        /// 随机字符串，长度要求在32位以内。推荐随机数生成算法
        /// </summary>
        public string nonce { get { return Senparc.Weixin.MP.Helpers.JSSDKHelper.GetNoncestr(); } }
        /// <summary>
        /// 异步接收微信支付结果通知的回调地址，通知url必须为外网可访问的url，不能携带参数。
        /// </summary>
        public string notify_url { get { return "https://192.168.1.102/api/weiXinPay/Notify"; } }
        /// <summary>
        /// 商品简单描述，该字段请按照规范传递，具体请见参数规定
        /// </summary>
        public string body { get { return "JSAPI支付测试"; } }
        /// <summary>
        /// 商户系统内部订单号，要求32个字符内，只能是数字、大小写字母_-|*且在同一个商户号下唯一。详见商户订单号
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 支持IPV4和IPV6两种格式的IP地址。调用微信支付API的机器IP
        /// </summary>
        public string spbill_create_ip { get { return "192.168.1.102"; } }
        /// <summary>
        /// 订单总金额，单位为分，详见支付金额
        /// </summary>
        public int total_fee { get { return 10; } }
        /// <summary>
        /// 小程序取值如下：JSAPI，详细说明见参数规定
        /// </summary>
        public string trade_type { get { return "JSAPI"; } }
        /// <summary>
        /// 交易过程生成签名的密钥，仅保留在商户系统和微信支付后台，
        /// 不会在网络中传播。商户妥善保管该Key，切勿在网络中传输，
        /// 不能在其他客户端中存储，保证key不会被泄漏。商户可根据邮件
        /// 提示登录微信商户平台进行设置。也可按以下路径设置：
        /// 微信商户平台(pay.weixin.qq.com)-->账户设置-->API安全-->密钥设置
        /// </summary>
        public string key { get { return "oDjIVWvPHBmjo5fH6p1q5ZkWEcJfKWfT"; } }
        /// <summary>
        /// AppSecret是APPID对应的接口密码，用于获取接口调用凭证时使用
        /// </summary>
        public string secret { get { return "0666e1b9071ce6baacca2adf5945319d"; } }
        /// <summary>
        /// 是否需要分账 Y-是，需要分账  N-否，不分账 字母要求大写，不传默认不分账
        /// </summary>
        public string profit_sharing { get; set; }

    }
}
