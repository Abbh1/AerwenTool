using ARW.Common;
using ARW.Model.System;
using ARW.Service.System.IService;
using Infrastructure;
using Infrastructure.Attribute;
using Quartz;
using System;
using System.Threading.Tasks;

namespace ARW.Tasks.TaskScheduler
{
    /// <summary>
    /// 定时任务测试
    /// 使用如下注册后TaskExtensions里面不用再注册了
    /// </summary>
    [AppService(ServiceType = typeof(Job_SyncTest), ServiceLifetime = LifeTime.Scoped)]
    public class Job_SyncTest : JobBase, IJob
    {
        private readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly ISysTasksLogService SysTasksLogService;
        private readonly ISysTasksQzService SysTasksQzService;


        public Job_SyncTest(ISysTasksLogService sysTasksLogService, ISysTasksQzService sysTasksQzService)
        {
            SysTasksLogService = sysTasksLogService;
            SysTasksQzService = sysTasksQzService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var logModel = await ExecuteJob(context, async () => await Run(),"发送成功！");
            await RecordTaskLog(context, logModel);
        }

        
        public async Task Run()
        {
            await Task.Delay(1);
            //TODO 业务逻辑

            MailHelper mailHelper = new MailHelper();
            string[] strings = {"2679599887@qq.com", "2423579486@qq.com" };
            mailHelper.SendMail(strings, "炒币强","喜欢吗？");

            //System.Console.WriteLine("job test");
        }


        /// <summary>
        /// 记录到日志
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logModel"></param>
        public async Task RecordTaskLog(IJobExecutionContext context, SysTasksLog logModel)
        {

            //可以直接获取 JobDetail 的值
            IJobDetail job = context.JobDetail;

            logModel.InvokeTarget = job.JobType.FullName;
            logModel = await SysTasksLogService.AddTaskLog(job.Key.Name, logModel);
            //成功后执行次数+1
            if (logModel.Status == "0")
            {
                await SysTasksQzService.UpdateAsync(f => new SysTasksQz()
                {
                    RunTimes = f.RunTimes + 1,
                    LastRunTime = DateTime.Now
                }, f => f.ID.ToString() == job.Key.Name);
            }
            logger.Info($"执行任务【{job.Key.Name}|{logModel.JobName}】结果={logModel.JobMessage}");
        }
    }
}
