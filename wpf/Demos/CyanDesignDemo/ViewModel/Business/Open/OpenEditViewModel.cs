using CyanDesignDemo.Model;
using CyanDesignDemo.Model.Business.Login;
using CyanDesignDemo.Model.Business.Open;
using CyanDesignDemo.View.Business;
using DMSkin.Core;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Xml.Linq;

namespace CyanDesignDemo.ViewModel.Business.Open
{
    public class OpenEditViewModel : ViewModelBase
    {

        // 实例化模型层
        private ProjectGroup projectGroupModel = new ProjectGroup();
        private Project projectModel = new Project();

        private Frame OpenFrame;
        private LoginModel UserInfo;

        public OpenEditViewModel(Project project, Frame openFrame)
        {
            projectModel = project;
            OpenFrame = openFrame;
            UserInfo = Storage.GetData<LoginModel>("userInfo");
        }

        #region 字段映射

        /// <summary>
        /// 项目分组列表
        /// </summary>
        public List<ProjectGroup> ProjectGroupList
        {
            get
            {
                projectGroupModel.ProjectGroupList = GetProjectGroupList();
                return projectGroupModel.ProjectGroupList;
            }
            set
            {
                projectGroupModel.ProjectGroupList = value;
                OnPropertyChanged(nameof(ProjectGroupList));
            }
        }

        /// <summary>
        /// 项目表单值
        /// </summary>
        public Project ProjectModelValue
        {
            get
            {
                return projectModel;
            }
            set
            {
                projectModel = value;
                OnPropertyChanged(nameof(ProjectModelValue));
            }
        }


        #endregion


        #region 命令

        /// <summary>
        /// 保存
        /// </summary>
        public ICommand SaveCommand => new DelegateCommand(async obj =>
        {
            var projectFormValue = ProjectModelValue;

            if (string.IsNullOrEmpty(projectFormValue.ProjectName))
            {
                MessageBox.Show("项目名称不能为空");
                return;
            }

            var msg = await AddOrUpdateProjecte();
            MessageBox.Show(msg);
            CloseDialog();
        });


        /// <summary>
        /// 关闭弹窗
        /// </summary>
        public ICommand CloseCommand => new DelegateCommand(obj =>
        {
            CloseDialog();
        });


        /// <summary>
        /// 上传图片
        /// </summary>
        public ICommand UploadImageCommand => new DelegateCommand(async obj =>
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png; *.jpg; *.jpeg; *.gif; *.bmp)|*.png; *.jpg; *.jpeg; *.gif; *.bmp";
            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;

                try
                {
                    // 将图片上传到服务器
                    var url = await GetUrl(imagePath);
                    ProjectModelValue.ProjectImg = url;
                    ProjectModelValue = ProjectModelValue;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("图片上传失败：" + ex.Message);
                }
            }
        });

        /// <summary>
        /// 清空图片
        /// </summary>
        public ICommand ClearImageCommand => new DelegateCommand(obj =>
        {
            ProjectModelValue.ProjectImg = "";
            ProjectModelValue= ProjectModelValue;
        });

        #endregion

        #region 请求

        /// <summary>
        /// 获取项目分组列表
        /// </summary>
        /// <returns></returns>
        public List<ProjectGroup> GetProjectGroupList()
        {
            var url = "/ProjectGroupApi/getProjectGroupList?ToolCustomerGuid=" + UserInfo.ToolCustomerGuid;
            var list = GetListData<ProjectGroup>(url);
            return list;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="imageUrl">文件路径</param>
        /// <returns></returns>
        private async Task<string> GetUrl(string imageUrl)
        {
            var res = await UploadFile<FileModel>(imageUrl, "Open");
            var url = res.url;
            return url;
        }

        /// <summary>
        /// 添加或修改项目
        /// </summary>
        private async Task<string> AddOrUpdateProjecte()
        {
            var res = await SendPostRequest("/ProjectesApi/addOrUpdateProjectes", ProjectModelValue);
            return res.msg;
        }


        #endregion

        #region 方法

        private void CloseDialog()
        {
            ProjectModelValue= new Project();
            ProjectModelValue= ProjectModelValue;
            var pageInstance = new PageOpen();
            OpenFrame.Navigate(pageInstance);
        }

        #endregion

    }
}
