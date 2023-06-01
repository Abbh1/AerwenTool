using CyanDesignDemo.Model.Business.Login;
using CyanDesignDemo.Model.Business.Open;
using CyanDesignDemo.ViewModel.Business.Open;
using DMSkin.Core;
using DMSkinDemo.ViewModel.Business.Open;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace CyanDesignDemo.View.Business
{
    /// <summary>
    /// PageOpenFiles.xaml 的交互逻辑
    /// </summary>
    public partial class PageOpenFiles : Page
    {
        private Frame _OpenFrame;
        private Project _Project;
        private LoginModel UserInfo;

        public PageOpenFiles(Project productData, Frame OpenFrame)
        {
            InitializeComponent();

            _OpenFrame = OpenFrame;
            _Project = productData;
            UserInfo = Storage.GetData<LoginModel>("userInfo");
            var pageOpenFiles = new OpenFilesViewModel(productData, OpenFrame);
            this.DataContext = pageOpenFiles;
        }



        // 编辑页面跳转
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //当前行选中数据
            var selectedData = ProjectFilesDataGrid.SelectedItem as ProjectFiles;
            selectedData.Title = "编辑项目配置";
            var pageInstance = new PageOpenFilesDialog(_Project, selectedData, _OpenFrame);
            _OpenFrame.Navigate(pageInstance);
        }

        // 添加
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var selectedData = new ProjectFiles();
            selectedData.Title = "添加项目配置";
            var pageInstance = new PageOpenFilesDialog(_Project, selectedData, _OpenFrame);
            _OpenFrame.Navigate(pageInstance);
        }

        /// <summary>
        /// 删除项目配置
        /// </summary>
        private async void DeleteProjectFiles(object sender, RoutedEventArgs e)
        {
            var selectedData = ProjectFilesDataGrid.SelectedItem as ProjectFiles;
            var viewModel = new ViewModelBase();

            MessageBoxResult result = MessageBox.Show("确定要删除该项目吗？", "确认删除", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // 用户点击了确认按钮
                var ids = selectedData.ProjectFilesId.ToString();
                var res = await viewModel.DeleteData("/ProjectFilesApi", ids);
                MessageBox.Show(res);
            }

        }


        //是否需要打开
        private async void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            var selectedData = ProjectFilesDataGrid.SelectedItem as ProjectFiles;
            if (selectedData != null)
            {
                selectedData.ProjectFilesIsOpen = 1;
                await AddOrUpdateProjecteFiles(selectedData);
            }
        }

        private async void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            var selectedData = ProjectFilesDataGrid.SelectedItem as ProjectFiles;
            if (selectedData != null)
            {
                selectedData.ProjectFilesIsOpen = 2;
                await AddOrUpdateProjecteFiles(selectedData);
            }
        }


        //是否启动git

        private async void ToggleButton_Checked_1(object sender, RoutedEventArgs e)
        {
            var selectedData = ProjectFilesDataGrid.SelectedItem as ProjectFiles;
            if (selectedData != null)
            {
                selectedData.ProjectFilesIsGit = 1;
                await AddOrUpdateProjecteFiles(selectedData);
            }
        }

        private async void ToggleButton_Unchecked_1(object sender, RoutedEventArgs e)
        {
            var selectedData = ProjectFilesDataGrid.SelectedItem as ProjectFiles;
            if (selectedData != null)
            {
                selectedData.ProjectFilesIsGit = 2;
                await AddOrUpdateProjecteFiles(selectedData);
            }
        }

        /// <summary>
        /// 添加或修改项目配置
        /// </summary>
        private async Task AddOrUpdateProjecteFiles(ProjectFiles projectFiles)
        {
            var viewModel = new ViewModelBase();
            projectFiles.CustomerGuid = UserInfo.ToolCustomerGuid;
            projectFiles.ProjectGuid = _Project.ProjectGuid;
            var res = await viewModel.SendPostRequest("/ProjectFilesApi/addOrUpdateProjectFiles", projectFiles);
            if (res.code != 200)
            {
                MessageBox.Show(res.msg);
            }
        }

    }
}
