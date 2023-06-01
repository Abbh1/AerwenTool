using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using CyanDesignDemo.Model.Business.DownLoad;
using CyanDesignDemo.Model.Business.Login;
using CyanDesignDemo.Model.Business.Open;
using CyanDesignDemo.ViewModel.Business.Open;
using DMSkin.Core;
using DMSkinDemo.ViewModel.Business;
using Ookii.Dialogs.Wpf;

namespace CyanDesignDemo.View.Business
{
    /// <summary>
    /// PageDownLoad.xaml 的交互逻辑
    /// </summary>
    public partial class PageDownLoad : Page
    {
        private LoginModel UserInfo;

        public PageDownLoad()
        {
            UserInfo = Storage.GetData<LoginModel>("userInfo");
            InitializeComponent();

            var downLoadViewModel = new DownLoadViewModel();
            this.DataContext = downLoadViewModel;

        }


        /// <summary>
        /// 当前选中分类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            // 获取选中的数据项
            var selectedDataItem = e.NewValue as DownloadCategory;

            // 将选中的数据项赋值给 ViewModel 的 SelectedTreeViewItem 属性
            var viewModel = DataContext as DownLoadViewModel;
            viewModel.SelectedTreeViewItem = selectedDataItem;

            // 搜索
            var viewModelBase = new ViewModelBase();
            var url = $"/DownloadFilesApi/getDownloadFilesList?DownloadCategoryGuid={selectedDataItem.DownloadCategoryGuid}";
            var list = viewModelBase.GetListData<DownloadFiles>(url);

            // 更新 DownloadFilesList 属性的值，触发界面刷新
            viewModel.DownloadFilesList = list;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var selectedData = DownLoadDataGrid.SelectedItem as DownloadFiles;
            var url = selectedData.DownloadFilesLink;
            selectedData.DownloadFilesVolume += 1;

            var viewModel = new ViewModelBase();
            await viewModel.SendPostRequest("/DownloadFilesApi/addOrUpdateDownloadFiles", selectedData);

            try
            {
                Process.Start(new ProcessStartInfo(url));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"无法打开浏览器：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var selectedData = DownLoadDataGrid.SelectedItem as DownloadFiles;
            var url = selectedData.DownloadFilesAttachment;
            try
            {
                Process.Start(new ProcessStartInfo(url));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"无法打开浏览器：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url">下载路径</param>
        /// <param name="savePath">保存地址</param>
        /// <returns></returns>
        public async Task DownloadFile(string url, string savePath)
        {
            try
            {

                using (HttpClient client = new HttpClient())
                {
                    // 发送 GET 请求并获取响应
                    HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

                    // 确保请求成功
                    response.EnsureSuccessStatusCode();

                    // 获取响应内容的总长度
                    long? totalBytes = response.Content.Headers.ContentLength;

                    // 创建保存文件的目录
                    //string directory = System.IO.Path.GetDirectoryName(savePath);
                    //Directory.CreateDirectory(directory);

                    // 读取响应的内容流
                    using (Stream contentStream = await response.Content.ReadAsStreamAsync())
                    {
                        // 创建文件流来保存文件
                        using (FileStream fileStream = File.Create(savePath))
                        {
                            // 创建缓冲区
                            byte[] buffer = new byte[8192];
                            int bytesRead;
                            long bytesDownloaded = 0;

                            // 从响应内容流读取并写入到文件流，同时更新下载进度
                            while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                            {
                                await fileStream.WriteAsync(buffer, 0, bytesRead);
                                bytesDownloaded += bytesRead;

                                // 计算下载进度（百分比）
                                double percentComplete = (double)bytesDownloaded / totalBytes.Value * 100;

                                // 更新下载进度的显示
                                MessageBox.Show($"Download progress: {percentComplete:F2}%");
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }

        }

        private string ChoseFolder()
        {
            var dialog = new VistaFolderBrowserDialog();
            var selectedFolder = "";

            if (dialog.ShowDialog() == true)
            {
                selectedFolder = dialog.SelectedPath;
                // 在这里处理选中的文件夹
            }

            return selectedFolder;
        }

    }
}
