using CyanDesignDemo;
using CyanDesignDemo.Model;
using CyanDesignDemo.Model.Business.Login;
using CyanDesignDemo.Model.Business.Open;
using CyanDesignDemo.View.Business;
using DMSkin.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace DMSkinDemo.ViewModel.Business.Open
{
    public class OpenFilesViewModel : ViewModelBase
    {
        // 实例化模型层
        private Project projectModel = new Project();
        private ProjectFiles projectFilesModel = new ProjectFiles();

        // 跳转对象
        private Frame OpenFrame;
        private LoginModel UserInfo;

        public OpenFilesViewModel(Project project, Frame openFrame)
        {
            projectModel = project;
            OpenFrame = openFrame;
            UserInfo = Storage.GetData<LoginModel>("userInfo");
        }

        #region 字段映射

        /// <summary>
        /// 项目数据
        /// </summary>
        public Project Project
        {
            get
            {
                return projectModel;
            }
            set
            {
                projectModel = value;
                OnPropertyChanged(nameof(Project));
            }
        }

        /// <summary>
        /// 项目配置列表
        /// </summary>
        public List<ProjectFiles> ProjectFilesList
        {
            get
            {
                projectFilesModel.ProjectFilesList = GetProjectFilesList();
                return projectFilesModel.ProjectFilesList;
            }
            set
            {
                projectFilesModel.ProjectFilesList = value;
                OnPropertyChanged(nameof(ProjectFilesList));
            }
        }

        #endregion

        #region 命令



        /// <summary>
        /// 关闭弹窗
        /// </summary>
        public ICommand CloseCommand => new DelegateCommand(obj =>
        {
            CloseDialog();
        });


        /// <summary>
        /// 刷新
        /// </summary>    
        public ICommand RefreshCommand => new DelegateCommand(obj =>
        {
            ProjectFilesList = GetProjectFilesList();
        });

        #endregion


        #region 请求
        //请求               
        public List<ProjectFiles> GetProjectFilesList()
        {
            var url = $"/ProjectFilesApi/getProjectFilesList?CustomerGuid={UserInfo.ToolCustomerGuid}&ProjectGuid={projectModel.ProjectGuid}";
            var list = GetListData<ProjectFiles>(url);
            return list;
        }

        #endregion


        #region 方法

        private void CloseDialog()
        {
            var pageInstance = new PageOpen();
            OpenFrame.Navigate(pageInstance);
        }

        public void Refresh()
        {
            ProjectFilesList = GetProjectFilesList();
        }


        #endregion

    }
}
