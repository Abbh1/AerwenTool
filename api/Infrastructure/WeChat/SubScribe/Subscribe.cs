using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin.Entities.TemplateMessage;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin;
using Senparc.Weixin.MP;
using Senparc.Weixin.WxOpen.AdvancedAPIs;
using Infrastructure;
using Senparc.Weixin.WxOpen.AdvancedAPIs.Template;
using System.Threading.Tasks;
using System;

namespace Infrastructure.WeChat.SubScribe
{
    public static class Subscribe
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public static async Task<string> SubscribeMessage(string templateId,string openId, TemplateMessageData templateMessageData)
        {
            await Task.Delay(1000);//停1秒钟，实际开发过程中可以将权限存入数据库，任意时间发送。

            var page = "pages/index/index";
            var wxOpenAppId = AppSettings.GetConfig("SenparcWeixinSetting:TenPayV3_AppId");
            //templateId也可以由后端指定

            try
            {
                var result = await MessageApi.SendSubscribeAsync(wxOpenAppId, openId, templateId, templateMessageData, page);
                if (result.errcode == ReturnCode.请求成功)
                {
                    return "消息已发送，请注意查收" ;
                }
                else
                {
                    return "发送失败：" + result.errmsg;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex,"推送失败");
                throw new CustomException("推送失败:"+ex);
            }

        }

        /// <summary>
        /// 下发小程序和公众号统一的服务消息
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static async Task<string> UniformSend(string mpTemplateId, string openId,string pagePath, object templateData)
        {

            await Task.Delay(1000);//停1秒钟，实际开发过程中可以将权限存入数据库，任意时间发送。

            try
            {
                var mpAppId = Config.SenparcWeixinSetting.MpSetting.WeixinAppId;//公众号ID


                //{"touser":"oeaTy0DgoGq-lyqvTauWVjbIVuP0","weapp_template_msg":null,"mp_template_msg":{"appid":"wx669ef95216eef885","template_id":null,"url":"https://dev.senparc.com","miniprogram":{"appid":"wx12b4f63276b14d4c","pagepath":"websocket/websocket"},"data":{"first":{"value":"小程序和公众号统一的服务消息","color":"#173177"},"keyword1":{"value":"2022/1/20 23:22:12","color":"#173177"},"keyword2":{"value":"dev.senparc.com","color":"#173177"},"keyword3":{"value":"小程序接口测试","color":"#173177"},"keyword4":{"value":"正常","color":"#173177"},"keyword5":{"value":"测试“小程序和公众号统一的服务消息”接口，服务正常","color":"#173177"},"remark":{"value":"您的 OpenId：oeaTy0DgoGq-lyqvTauWVjbIVuP0","color":"#173177"},"TemplateId":"ur6TqESOo-32FEUk4qJxeWZZVt4KEOPjqbAFDGWw6gg","Url":"https://dev.senparc.com","TemplateName":"系统异常告警通知"}}}



                var wxOpenAppId = AppSettings.GetConfig("SenparcWeixinSetting:TenPayV3_AppId");
                var miniprogram = new Miniprogram_PagePath(wxOpenAppId, pagePath);//使用 pagepath 参数
                //var miniprogram = new Miniprogram_Page(WxOpenAppId, pagePath);// 使用 page 参数
                //https://weixin.senparc.com/QA-17333

                UniformSendData msgData = new(
                    openId,
                    new Mp_Template_Msg(mpAppId,
                                        mpTemplateId,
                                        "https://dev.senparc.com",
                                        miniprogram,
                                        templateData)
                    );

                var result = await Senparc.Weixin.WxOpen.AdvancedAPIs.Template.TemplateApi.UniformSendAsync(mpAppId, msgData);

                if (result.errcode == ReturnCode.请求成功)
                {
                    return "服务消息已发送，请注意查收";
                }
                else
                {
                    string msg;

                    if (result.errmsg.Contains("require subscribe"))
                    {
                        msg = "您需要关注公众号【盛派网络小助手】才能收到公众号内的模板消息！";
                    }
                    else
                    {
                        msg = result.errmsg;
                    }

                    return "出错啦！";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("推送报错:"+ex);
            }
        }


    }
}
