using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows;

namespace CyanDesignDemo
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 检查当前用户是否为管理员
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            bool isElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);

            // 如果当前用户不是管理员，则重新以管理员权限运行应用程序
            if (!isElevated)
            {
                try
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.UseShellExecute = true;
                    startInfo.WorkingDirectory = Environment.CurrentDirectory;
                    startInfo.FileName = Process.GetCurrentProcess().MainModule.FileName;
                    startInfo.Verb = "runas"; // 以管理员权限运行

                    Process.Start(startInfo);

                    // 关闭当前实例
                    Environment.Exit(0);

                }
                catch (Exception ex)
                {
                    // 处理启动以管理员权限运行时的异常
                    MessageBox.Show("无法以管理员权限运行应用程序：" + ex.Message);
                    Environment.Exit(1);
                }
            }
            else
            {
                // 正常启动应用程序
                //Login mainWindow = new Login();
                //mainWindow.Show();
            }
        }
    }
}
