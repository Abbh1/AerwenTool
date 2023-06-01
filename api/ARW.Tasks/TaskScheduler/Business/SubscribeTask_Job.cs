using ARW.Common;
using ARW.Model.System;
using ARW.Service.System.IService;
using Infrastructure;
using Infrastructure.Attribute;
using Infrastructure.WeChat.SubScribe;
using Quartz;
using Senparc.CO2NET.Extensions;
using Senparc.Weixin.Entities.TemplateMessage;
using Senparc.Weixin.WxOpen.AdvancedAPIs.WxApp.WxAppJson;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemTime = System.SystemTime;

namespace ARW.Tasks.TaskScheduler.Business
{
    /// <summary>
    /// 订阅推送任务
    /// </summary>
    [AppService(ServiceType = typeof(SubscribeTask_Job), ServiceLifetime = LifeTime.Scoped)]
    public class SubscribeTask_Job : JobBase, IJob
    {
        //private readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        //private readonly ISysTasksLogService SysTasksLogService;
        //private readonly ISysTasksQzService SysTasksQzService;

        //private readonly ISubscribeTaskService _SubscribeTaskService;


        //public SubscribeTask_Job(ISysTasksLogService sysTasksLogService, ISysTasksQzService sysTasksQzService, ISubscribeTaskService subscribeTaskService)
        //{
        //    SysTasksLogService = sysTasksLogService;
        //    SysTasksQzService = sysTasksQzService;
        //    _SubscribeTaskService = subscribeTaskService;
        //}

        //public async Task Execute(IJobExecutionContext context)
        //{
        //    var message = await Run();
        //    var logModel = await ExecuteJob(context, async () => await Run(), message);
        //    await RecordTaskLog(context, logModel);
        //}


        //public async Task<string> Run()
        //{
        //    await Task.Delay(1);
        //    //TODO 业务逻辑

        //    var waitPushList = _SubscribeTaskService.GetSubscribeTaskList();
        //    if (waitPushList.Count > 0)
        //    {
        //        var pagePath = "pages/myMessage/myMessage";
        //        var templateMessageData = new TemplateMessageData();
        //        templateMessageData["thing1"] = new TemplateMessageDataValue("测试门店");
        //        templateMessageData["thing2"] = new TemplateMessageDataValue("佛山市顺德区xxx健身");
        //        templateMessageData["time3"] = new TemplateMessageDataValue(SystemTime.Now.ToString("yyyy年MM月dd日 HH:mm"));
        //        templateMessageData["short_thing4"] = new TemplateMessageDataValue("1小时");
        //        templateMessageData["thing5"] = new TemplateMessageDataValue("还有1个小时开始健身，请熟知！");

        //        var taskList = new List<string>();
        //        foreach (var item in waitPushList)
        //        {
        //            try
        //            {
        //                if (item.EndTime < DateTime.Now)
        //                {
        //                    logger.Info("执行到了这里" + item.OpenId);
        //                    logger.Info("一共有" + waitPushList.Count + "个");
        //                    var res = await Subscribe.SubscribeMessage(item.TemplateId, item.OpenId, templateMessageData);
        //                    logger.Info("推送结果：" + res);

        //                    if (res == "消息已发送，请注意查收")
        //                    {

        //                        // 修改推送状态
        //                        await _SubscribeTaskService.UpdateAsync(f => new SubscribeTask
        //                        {
        //                            SubscribeTaskStatus = 1
        //                        }, s => s.SubscribeTaskGuid == item.SubscribeTaskGuid);
        //                        taskList.Add(item.OpenId + "用户的订阅已推送");
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                return ex.ToString();
        //            }
        //        }
        //        return taskList.ToJson().ToString();
        //    }
        //    else
        //    {
        //        logger.Info("空的");
        //        return "没有需要执行的任务";
        //    }
        //    //return "执行成功！";

        //    #region 公众号模板消息信息       -- DPBMARK MP

        //    //可选参数（需要和公众号模板消息匹配）：
        //    //var templateData = new
        //    //{
        //    //    first = new Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage.TemplateDataItem("预约健身开始提醒"),
        //    //    keyword1 = new Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage.TemplateDataItem("测试门店"),
        //    //    keyword2 = new Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage.TemplateDataItem("佛山市顺德区xxx健身"),
        //    //    keyword3 = new Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage.TemplateDataItem(SystemTime.Now.ToString("yyyy年MM月dd日 HH:mm")),
        //    //    keyword4 = new Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage.TemplateDataItem("1小时"),
        //    //    keyword5 = new Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage.TemplateDataItem("还有1个小时开始健身，请熟知！"),
        //    //    remark = new Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage.TemplateDataItem("您的 OpenId：" + openId),
        //    //};
        //    #endregion                      DPBMARK_END
        //    //System.Console.WriteLine("job test");
        //}


        ///// <summary>
        ///// 记录到日志
        ///// </summary>
        ///// <param name="context"></param>
        ///// <param name="logModel"></param>
        //public async Task RecordTaskLog(IJobExecutionContext context, SysTasksLog logModel)
        //{

        //    //可以直接获取 JobDetail 的值
        //    IJobDetail job = context.JobDetail;

        //    logModel.InvokeTarget = job.JobType.FullName;
        //    logModel = await SysTasksLogService.AddTaskLog(job.Key.Name, logModel);
        //    //成功后执行次数+1
        //    if (logModel.Status == "0")
        //    {
        //        await SysTasksQzService.UpdateAsync(f => new SysTasksQz()
        //        {
        //            RunTimes = f.RunTimes + 1,
        //            LastRunTime = DateTime.Now
        //        }, f => f.ID.ToString() == job.Key.Name);
        //    }
        //    logger.Info($"执行任务【{job.Key.Name}|{logModel.JobName}】结果={logModel.JobMessage}");
        //}
        public Task Execute(IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
