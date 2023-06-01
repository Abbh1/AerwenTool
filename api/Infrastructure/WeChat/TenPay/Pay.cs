using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Senparc.CO2NET.Extensions;
using Senparc.CO2NET.HttpUtility;
using Senparc.CO2NET.Utilities;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.Helpers;
using Senparc.Weixin.MP.AdvancedAPIs.MerChant;
using Senparc.Weixin.TenPayV3;
using Senparc.Weixin.TenPayV3.Apis;
using Senparc.Weixin.TenPayV3.Apis.BasePay;
using Senparc.Weixin.TenPayV3.Apis.Entities;
using Senparc.Weixin.TenPayV3.Entities;
using Senparc.Weixin.TenPayV3.Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Senparc.Weixin.TenPayV3.Apis.BasePay.Entities;

namespace Infrastructure.WeChat.TenPay
{
   
    /// <summary>
    /// 微信支付基础类
    /// </summary>
    public class Pay
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 用于初始化BasePayApis
        /// </summary>
        private readonly ISenparcWeixinSettingForTenpayV3 _tenpayV3Setting;
        public static HttpContext HttpContext => HttpContextLocal.Current();
        private readonly BasePayApis _basePayApis;
        private readonly SenparcHttpClient _httpClient;

        /// <summary>
        /// trade_no 和 transaction_id 对照表
        /// TODO：可以放入缓存，设置有效时间
        /// </summary>
        //public static ConcurrentDictionary<string, string> TradeNumberToTransactionId = new ConcurrentDictionary<string, string>();

        public TenPayV3Info TenPayV3Info;


        public Pay(SenparcHttpClient httpClient)
        {
            var TenPayV3_AppId = AppSettings.GetConfig("SenparcWeixinSetting:TenPayV3_AppId");
            var TenPayV3_AppSecret = AppSettings.GetConfig("SenparcWeixinSetting:TenPayV3_AppSecret");
            var TenPayV3_MchId = AppSettings.GetConfig("SenparcWeixinSetting:TenPayV3_MchId");
            var TenPayV3_Key = AppSettings.GetConfig("SenparcWeixinSetting:TenPayV3_Key");
            var TenPayV3_CertPath = AppSettings.GetConfig("SenparcWeixinSetting:TenPayV3_CertPath");
            var TenPayV3_CertSecret = AppSettings.GetConfig("SenparcWeixinSetting:TenPayV3_CertPath");
            var TenPayV3_TenpayNotify = AppSettings.GetConfig("SenparcWeixinSetting:TenPayV3_TenpayNotify");
            var TenPayV3_WxOpenNotify = AppSettings.GetConfig("SenparcWeixinSetting:TenPayV3_WxOpenNotify");
            var TenPayV3_PrivateKey = AppSettings.GetConfig("SenparcWeixinSetting:TenPayV3_PrivateKey");
            var TenPayV3_SerialNumber = AppSettings.GetConfig("SenparcWeixinSetting:TenPayV3_SerialNumber");
            var TenPayV3_ApiV3Key = AppSettings.GetConfig("SenparcWeixinSetting:TenPayV3_ApiV3Key");


            var tenPayV3Info = new TenPayV3Info(TenPayV3_AppId, TenPayV3_AppSecret, TenPayV3_MchId, TenPayV3_Key, TenPayV3_CertPath, TenPayV3_CertSecret,TenPayV3_TenpayNotify, TenPayV3_WxOpenNotify,TenPayV3_PrivateKey, TenPayV3_SerialNumber, TenPayV3_ApiV3Key);
            TenPayV3InfoCollection.Register(tenPayV3Info, "测试");
            this.TenPayV3Info = tenPayV3Info;

            _tenpayV3Setting = Senparc.Weixin.Config.SenparcWeixinSetting.TenpayV3Setting;
            _basePayApis = new BasePayApis(_tenpayV3Setting);
            this._httpClient = httpClient;
        }


        public async Task<JsApiUiPackage> PrePay(long productGuid, string openId,string orderNo,int type,int price)
        {
            string sp_billno = orderNo;//out_trade_no

            //调用下单接口下单
            var name = "测试";

            try
            {
                var appId = TenPayV3Info.AppId;

                var notifyUrl = TenPayV3Info.TenPayV3_WxOpenNotify;

                //请求信息
                TransactionsRequestData jsApiRequestData = new(
                        appId,
                        TenPayV3Info.MchId,
                        name + " - 微信支付 V3",
                        sp_billno,
                        new TenpayDateTime(DateTime.Now.AddMinutes(1), false),
                        null,
                        notifyUrl,
                        null,
                        new() { currency = "CNY", total = price },
                        new(openId),
                        null,
                        null,
                        null
                    );

                logger.Info("支付参数：{0}", jsApiRequestData.ToJson());


                //请求接口
                var basePayApis2 = new Senparc.Weixin.TenPayV3.TenPayHttpClient.BasePayApis2(_httpClient, _tenpayV3Setting);
                var result = await basePayApis2.JsApiAsync(jsApiRequestData);

                logger.Info("支付结果：{@UnifiedorderResult}", result);


                if (result.VerifySignSuccess != true)
                {
                    throw new WeixinException("获取 prepay_id 结果校验出错！");
                }

                //获取 UI 信息包
                var jsApiUiPackage = TenPaySignHelper.GetJsApiUiPackage(appId, result.prepay_id);


                return jsApiUiPackage;
            }
            catch (Exception)
            {

                throw;
            }


        }

        /// <summary>
        /// JS-SDK支付回调地址（在下单接口中设置的 notify_url）
        /// </summary>
        /// <returns></returns>
        public async Task<OrderReturnJson> PayNotifyUrl()
        {
            try
            {
                //获取微信服务器异步发送的支付通知信息
                var resHandler = new TenPayNotifyHandler(HttpContext);

                var orderReturnJson = await resHandler.AesGcmDecryptGetObjectAsync<OrderReturnJson>();

                //演示记录 transaction_id，实际开发中需要记录到数据库，以便退款和后续跟踪
                //TradeNumberToTransactionId[orderReturnJson.out_trade_no] = orderReturnJson.transaction_id;

                //获取支付状态
                string trade_state = orderReturnJson.trade_state;

                //验证请求是否从微信发过来（安全）
                NotifyReturnData returnData = new();

                //验证可靠的支付状态
                if (orderReturnJson.VerifySignSuccess == true && trade_state == "SUCCESS")
                {
                    returnData.code = "SUCCESS";//正确的订单处理
                    logger.Info("回调成功啦！", returnData.code.ToString());
                    /* 提示：
                        * 1、直到这里，才能认为交易真正成功了，可以进行数据库操作，但是别忘了返回规定格式的消息！
                        * 2、上述判断已经具有比较高的安全性以外，还可以对访问 IP 进行判断进一步加强安全性。
                        * 3、下面演示的是发送支付成功的模板消息提示，非必须。
                        */
                }
                else
                {
                    returnData.code = "FAILD";//错误的订单处理
                    returnData.message = "验证失败";
                    //此处可以给用户发送支付失败提示等
                }

                #region 记录日志（也可以记录到数据库审计日志中）
                var notifyJson = orderReturnJson.ToJson().ToString();

                await payLog("WechatPay", "支付通知信息：" + notifyJson + "");

                #endregion

                //https://pay.weixin.qq.com/wiki/doc/apiv3/apis/chapter3_1_5.shtml
                return orderReturnJson;
            }
            catch (Exception ex)
            {
                WeixinTrace.WeixinExceptionLog(new WeixinException(ex.Message, ex));
                throw;
            }
        }


        /// <summary>
        /// 订单查询
        /// </summary>
        /// <returns></returns>
        public async Task<OrderReturnJson> OrderQuery(string out_trade_no = null, string transaction_id = null)
        {
            //out_trade_no transaction_id 两个参数不能都为空
            if (out_trade_no is null && transaction_id is null)
            {
                throw new ArgumentNullException(nameof(out_trade_no) + " or " + nameof(transaction_id));
            }

            OrderReturnJson result = null;

            //选择方式查询订单
            if (out_trade_no is not null)
            {
                result = await _basePayApis.OrderQueryByOutTradeNoAsync(out_trade_no, TenPayV3Info.MchId);
            }
            if (transaction_id is not null)
            {
                result = await _basePayApis.OrderQueryByTransactionIdAsync(transaction_id, TenPayV3Info.MchId);
            }

            return result;
        }


        /// <summary>
        /// 关闭订单
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnJsonBase> CloseOrder(string out_trade_no)
        {

            //out_trade_no transaction_id 两个参数不能都为空
            if (out_trade_no is null)
            {
                throw new ArgumentNullException(nameof(out_trade_no));
            }

            ReturnJsonBase result = null;
            result = await _basePayApis.CloseOrderAsync(out_trade_no, TenPayV3Info.MchId);

            return result;
        }



        /// <summary>
        /// 退款申请接口
        /// </summary>
        /// <returns></returns>
        public async Task<RefundReturnJson> Refund(string transactionId, decimal totalFee,string? paymentRefundNumber)
        {
            try
            {
                string outRefundNo;
                await payLog("WechatRefund", "1");

                string nonceStr = TenPayV3Util.GetNoncestr();

                await payLog("WechatRefund", "2 退款微信单号transactionId：" + transactionId);

                if (!string.IsNullOrEmpty(paymentRefundNumber))
                {
                    outRefundNo = paymentRefundNumber;
                }
                else
                {
                    outRefundNo = CreateNo_Recharge();
                }
                int refundFee = Convert.ToInt32(totalFee);
                string opUserId = TenPayV3Info.MchId;
                var notifyUrl = "http://aerwen.net/prod-api/api/WxPay/refundNotifyUrl";
                //var dataInfo = new TenPayV3RefundRequestData(TenPayV3Info.AppId, TenPayV3Info.MchId, TenPayV3Info.Key,
                //    null, nonceStr, null, outTradeNo, outRefundNo, totalFee, refundFee, opUserId, null, notifyUrl: notifyUrl);
                //TODO:该接口参数二选一传入
                var dataInfo = new RefundRequsetData(transactionId, null, outRefundNo, "Senparc TenPayV3 demo退款测试", notifyUrl, null, new RefundRequsetData.Amount(refundFee, null, refundFee, "CNY"), null);


                //#region 新方法（Senparc.Weixin v6.4.4+）
                //var result = TenPayOldV3.Refund(_serviceProvider, dataInfo);//证书地址、密码，在配置文件中设置，并在注册微信支付信息时自动记录
                //#endregion
                var result = await _basePayApis.RefundAsync(dataInfo);

                await payLog("WechatRefund", "3 退款结果Result：" + result.ToJson());
                return result;
                //return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WeixinTrace.WeixinExceptionLog(new WeixinException(ex.Message, ex));

                throw;
            }
        }

        /// <summary>
        /// 退款通知地址
        /// </summary>
        /// <returns></returns>
        public async Task<RefundNotifyJson> RefundNotifyUrl()
        {
            await payLog("WechatRefund", "允许被访问IP" + HttpContext.UserHostAddress()?.ToString());

            NotifyReturnData returnData = new();
            var resHandler = new TenPayNotifyHandler(HttpContext);
            var refundNotifyJson = await resHandler.AesGcmDecryptGetObjectAsync<RefundNotifyJson>();
            try
            {
                await payLog("WechatRefund", "退款支付结果：" + refundNotifyJson.ToJson());

                string refund_status = refundNotifyJson.refund_status;
                if (/*refundNotifyJson.VerifySignSuccess == true &*/ refund_status == "SUCCESS")
                {
                    returnData.code = "SUCCESS";
                    returnData.message = "OK";

                    //填写逻辑
                    await payLog("WechatRefund", "验证通过");
                }
                else
                {
                    returnData.code = "FAILD";
                    returnData.message = "验证失败";
                    await payLog("WechatRefund", "验证失败");

                }
                return refundNotifyJson;

                //进行后续业务处理
            }
            catch (Exception ex)
            {
                returnData.code = "FAILD";
                returnData.message = ex.Message;
                WeixinTrace.WeixinExceptionLog(new WeixinException(ex.Message, ex));
            }

            return refundNotifyJson;

            //https://pay.weixin.qq.com/wiki/doc/apiv3/wechatpay/wechatpay3_3.shtml
        }


        #region 参数模型
        /// <summary>
        /// 小程序支付返回的参数
        /// </summary>
        public class PayParams
        {
            /// <summary>
            /// 产品Guid
            /// </summary>
            public JsApiUiPackage jsApiUiPackage { get; set; }

            /// <summary>
            /// 系统订单号
            /// </summary>
            public string outTradeNo { get; set; }

            /// <summary>
            ///订单下单时间
            /// </summary>
            public DateTime CreateTime { get; set; }

            /// <summary>
            ///订单结束时间
            /// </summary>
            public DateTime OverTime { get; set; }
        }

        /// <summary>
        /// 小程序支付接口的参数
        /// </summary>
        public class PayDto
        {
            /// <summary>
            /// 产品Guid
            /// </summary>
            public long ProductGuid { get; set; }

            /// <summary>
            /// 支付类型
            /// </summary>
            public int type { get; set; }

            /// <summary>
            /// 小程序用户OpenId
            /// </summary>
            public string openId { get; set; }
        }

        /// <summary>
        /// 小程序支付订单查询的参数
        /// </summary>
        public class OrderQueryDto
        {
            /// <summary>
            /// 系统订单号
            /// </summary>
            public string? outTradeNo { get; set; }

            /// <summary>
            /// 微信支付订单号
            /// </summary>
            public string? transactionId { get; set; }

        }


        /// <summary>
        /// 小程序支付需要的参数
        /// </summary>
        public class PayRequesEntity
        {
            /// <summary>
            /// 时间戳从1970年1月1日00:00:00至今的秒数,即当前的时间
            /// </summary>
            public string timeStamp { get; set; }

            /// <summary>
            /// 随机字符串，长度为32个字符以下。
            /// </summary>
            public string nonceStr { get; set; }

            /// <summary>
            /// 统一下单接口返回的 prepay_id 参数值
            /// </summary>
            public string package { get; set; }

            /// <summary>
            /// 签名算法
            /// </summary>
            public string signType { get; set; }

            /// <summary>
            /// 签名
            /// </summary>
            public string paySign { get; set; }
        }
        #endregion


        #region 微信支付日志操作
        /// <summary>
        /// 微信支付日志
        /// </summary>
        /// <param name="dirName"></param>
        public async Task payLog(string dirName,string data)
        {
            var logDir = ServerUtility.ContentRootMapPath(string.Format("~/App_Data/{0}/{1}", dirName , SystemTime.Now.ToString("yyyyMMdd")));
            if (!Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
            }

            var logPath = Path.Combine(logDir, string.Format("{0}-{1}-{2}.txt", SystemTime.Now.ToString("yyyyMMdd"), SystemTime.Now.ToString("HHmmss"), Guid.NewGuid().ToString("n").Substring(0, 8)));

            using (var fileStream = System.IO.File.OpenWrite(logPath))
            {
                await fileStream.WriteAsync(Encoding.Default.GetBytes(data), 0, Encoding.Default.GetByteCount(data));
                fileStream.Close();
            }
        }
        #endregion


        /// <summary>
        /// 申请资金账单接口
        /// </summary>
        /// <param name="date">日期，格式如：2021-08-27</param>
        /// <returns></returns>
        public async Task<string> FundflowBill(string date)
        {
            var filePath = $"{date}-FundflowBill.csv";
            Console.WriteLine("FilePath:" + filePath);
            using (var fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                BasePayApis basePayApis = new BasePayApis();

                var result = await _basePayApis.FundflowBillQueryAsync(date, fs);

                fs.Flush();
            }
            return "已经下载倒指定目录，文件名：" + filePath;
        }


        public static object _lock = new object();

        public static int count = 1;
        /// <summary>
        /// 退款订单号生成
        /// </summary>
        /// <returns></returns>
        public static string CreateNo_Recharge()
        {
            lock (_lock)
            {
                if (count >= 10000)
                {
                    count = 1;
                }

                var number = "R" + DateTime.Now.ToString("yyMMddHHmmssfff") + count.ToString("0000");

                count++;

                return number;
            }
        }


    }
}
