using Infrastructure;
using NLog;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ARW.Model.System;
using ARW.Service.System.IService;
using Microsoft.Extensions.DependencyInjection;
using Google.Protobuf.Reflection;
using Microsoft.AspNetCore.Builder;

namespace ARW.Tasks
{
    public class JobBase
    {
        /// <summary>
        /// 日志接口
        /// </summary>
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();


        /// <summary>
        /// 执行指定任务
        /// </summary>
        /// <param name="context">作业上下文</param>
        /// <param name="job">业务逻辑方法</param>
        public async Task<SysTasksLog> ExecuteJob(IJobExecutionContext context, Func<Task> job,string msg)
        {
            double elapsed = 0;
            int status = 0;
            string logMsg;
            try
            {
                //var s = context.Trigger.Key.Name;
                //记录Job时间
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                //执行任务
                await job();
                stopwatch.Stop();
                elapsed = stopwatch.Elapsed.TotalMilliseconds;
                logMsg = msg;
            }
            catch (Exception ex)
            {
                JobExecutionException e2 = new(ex)
                {
                    //true  是立即重新执行任务 
                    RefireImmediately = true
                };
                status = 1;
                logMsg = $"Fail，Exception：{ex.Message}";
            }

            var logModel = new SysTasksLog()
            {
                Elapsed = elapsed,
                Status = status.ToString(),
                JobMessage = logMsg
            };

            //await RecordTaskLog(context, logModel);
            return logModel;
        }

    }
}
