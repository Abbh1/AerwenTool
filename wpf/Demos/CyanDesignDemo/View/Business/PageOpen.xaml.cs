using CyanDesignDemo.Model.Business.Open;
using CyanDesignDemo.ViewModel.Business.Open;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ookii.Dialogs.Wpf;
using DMSkin.Core;
using CyanDesignDemo.Model.Business.Login;
using System.Windows.Markup;

namespace CyanDesignDemo.View.Business
{
    /// <summary>
    /// PageOpen.xaml 的交互逻辑
    /// </summary>
    public partial class PageOpen : Page
    {
        private LoginModel UserInfo;

        public PageOpen()
        {
            InitializeComponent();

            UserInfo = Storage.GetData<LoginModel>("userInfo");

            ProjectDataGrid.CellEditEnding += DataGrid_CellEditEnding;

            var openViewModel = new OpenViewModel(OpenFrame);
            openViewModel.ProjectFormValue.ProjectGroupGuid = 1;
            this.DataContext = openViewModel;
        }


        // 编辑页面跳转
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //当前行选中数据
            var selectedData = ProjectDataGrid.SelectedItem as Project;
            var pageInstance = new PageOpenEdit(selectedData, OpenFrame);
            OpenFrame.Navigate(pageInstance);
        }

        /// <summary>
        /// 打开配置页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //当前行选中数据
            var selectedData = ProjectDataGrid.SelectedItem as Project;
            var pageInstance = new PageOpenFiles(selectedData, OpenFrame);
            OpenFrame.Navigate(pageInstance);
        }

        /// <summary>
        /// 删除确认框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("确定要删除该项目吗？", "确认删除", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var viewModelBase = new ViewModelBase();

                //当前行选中数据
                var selectedData = ProjectDataGrid.SelectedItem as Project;
                var msg = await viewModelBase.DeleteData("/ProjectesApi", selectedData.ProjectId.ToString());
                MessageBox.Show(msg);
                var openViewModel = new OpenViewModel(OpenFrame);
                this.DataContext = openViewModel;
            }
            else
            {
                // 用户点击了取消按钮或关闭了对话框
                // 取消删除操作
            }

        }

        /// <summary>
        /// 当前选中项目分组数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            // 获取选中的数据项
            var selectedDataItem = e.NewValue as ProjectGroup;

            // 将选中的数据项赋值给 ViewModel 的 SelectedTreeViewItem 属性
            var viewModel = DataContext as OpenViewModel;
            viewModel.SelectedTreeViewItem = selectedDataItem;

            // 搜索
            var viewModelBase = new ViewModelBase();
            var url = $"/ProjectesApi/getProjectesList?ToolCustomerGuid={UserInfo.ToolCustomerGuid}&ProjectGroupGuid={selectedDataItem.ProjectGroupGuid}";
            var list = viewModelBase.GetListData<Project>(url);

            // 更新 ProjectList 属性的值，触发界面刷新
            viewModel.ProjectList = list;
        }




        /// <summary>
        /// 打开项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var viewModelBase = new ViewModelBase();

            var selectedData = ProjectDataGrid.SelectedItem as Project;

            var url = $"/ProjectesApi/getProjectesOfFilesList?ToolCustomerGuid={UserInfo.ToolCustomerGuid}&ProjectGuid={selectedData.ProjectGuid}";
            var filesList = viewModelBase.GetListData<ProjectFiles>(url);

            if (filesList.Count == 0)
            {
                MessageBox.Show("请先配置项目配置");
                return;
            }

            try
            {
                foreach (var item in filesList)
                {
                    // 打开文件
                    if (item.IsProjectFilesOpen)
                    {
                        // 选择打开方式
                        if (item.IsChooseOpen)
                        {
                            //打开方式路径
                            string openPath = item.ProjectFilesOpenMethodPath;
                            //文件路径
                            string path = item.ProjectFilesFileOpenPath;

                            //打开文件
                            Process.Start(openPath, path);
                        }
                        else
                        {
                            //直接打开
                            string path = item.ProjectFilesFileOpenPath;
                            Process.Start(path);
                        }
                    }

                    // Git拉取
                    if (item.IsProjectFilesIsGit)
                    {
                        string gitPath = "git"; // Git 可执行文件的路径，如果已将其添加到系统环境变量中，则可以直接使用 "git"
                        string repositoryPath = item.ProjectFilesGitPath; // 项目仓库的路径

                        // 执行 git fetch 命令
                        RunGitCommand(gitPath, repositoryPath, "fetch");

                        // 执行 git pull 命令
                        RunGitCommand(gitPath, repositoryPath, "pull");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("错误：" + ex);
                throw;
            }

        }


        private void RunGitCommand(string gitPath, string workingDirectory, string command)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = gitPath,
                    Arguments = command,
                    WorkingDirectory = workingDirectory,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = new Process { StartInfo = startInfo })
                {
                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (process.ExitCode == 0)
                    {
                        // Git 命令执行成功
                        //MessageBox.Show(command + "成功");
                    }
                    else
                    {
                        // Git 命令执行失败
                        MessageBox.Show(command + "报错: " + error + "请自行去sourtree解决冲突");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(command + "报错: " + ex.Message + "请自行去sourtree解决冲突");
            }
        }


        // 修改项目排序
        private async void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            // 获取正在编辑的单元格
            var cell = e.EditingElement;
            if (cell != null)
            {
                // 获取绑定的数据对象
                var item = e.Row.Item as Project;
                if (item != null)
                {
                    // 获取新值
                    var textBox = e.EditingElement as TextBox;
                    var newValue = textBox.Text;
                    var viewModelBase = new ViewModelBase();

                    if (item.ProjectSort == Convert.ToInt32(newValue)) return;

                    item.ProjectSort = Convert.ToInt32(newValue);
                    var res = await viewModelBase.SendPostRequest("/ProjectesApi/addOrUpdateProjectes", item);
                    MessageBox.Show(res.data);
                    var projectGroupViewModel = new OpenViewModel(OpenFrame);
                    this.DataContext = projectGroupViewModel;
                }
            }
        }


    }
}
