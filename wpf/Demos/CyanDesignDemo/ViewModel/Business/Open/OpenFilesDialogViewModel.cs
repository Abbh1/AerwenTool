using CyanDesignDemo;
using CyanDesignDemo.Model;
using CyanDesignDemo.Model.Business.Login;
using CyanDesignDemo.Model.Business.Open;
using CyanDesignDemo.View.Business;
using DMSkin.Core;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace DMSkinDemo.ViewModel.Business.Open
{
    public class OpenFilesDialogViewModel : ViewModelBase
    {
        // 实例化模型层
        private ProjectFiles projectFilesModel = new ProjectFiles();

        // 跳转对象
        private Frame OpenFrame;
        private LoginModel UserInfo;
        private Project _Project;

        public OpenFilesDialogViewModel(Project project, ProjectFiles projectFile, Frame openFrame)
        {
            _Project = project;
            projectFilesModel = projectFile;
            OpenFrame = openFrame;
            UserInfo = Storage.GetData<LoginModel>("userInfo");
        }

        #region 字段映射

        /// <summary>
        /// 项目配置
        /// </summary>
        public ProjectFiles ProjectFiles
        {
            get
            {
                return projectFilesModel;
            }
            set
            {
                projectFilesModel = value;
                OnPropertyChanged(nameof(ProjectFiles));
            }
        }

        #endregion


        #region 命令


        /// <summary>
        /// 保存项目配置
        /// </summary>
        public ICommand SaveCommand => new DelegateCommand(async obj =>
        {
            var projectFiles = ProjectFiles;

            if (string.IsNullOrEmpty(ProjectFiles.Title))
            {
                MessageBox.Show("项目配置标题不能为空");
                return;
            }

            var res = await AddOrUpdateProjecteFiles();
            if (res.code == 200)
            {
                MessageBox.Show(res.msg);
                CloseDialog();
            }
            else
            {
                MessageBox.Show(res.msg);
            }
        });


        /// <summary>
        /// 关闭弹窗
        /// </summary>
        public ICommand CloseCommand => new DelegateCommand(obj =>
        {
            CloseDialog();
        });

        /// <summary>
        /// 打开方式路径
        /// </summary>    
        public ICommand SelectOpenMethodPathCommand => new DelegateCommand(obj =>
        {
            var files = ChoseFiles();
            if (!string.IsNullOrEmpty(files))
            {
                ProjectFiles.ProjectFilesOpenMethodPath = files;
                ProjectFiles = ProjectFiles;
            }
        });

        /// <summary>
        /// 文件路径
        /// </summary>    
        public ICommand SelectFileOpenPathCommand => new DelegateCommand(obj =>
        {
            if (ProjectFiles.IsFolderSelected)
            {
                var folder = ChoseFolder();
                if (!string.IsNullOrEmpty(folder))
                {
                    ProjectFiles.ProjectFilesFileOpenPath = folder;
                    ProjectFiles = ProjectFiles;
                }
            }
            else
            {
                var files = ChoseFiles();
                if (!string.IsNullOrEmpty(files))
                {
                    ProjectFiles.ProjectFilesFileOpenPath = files;
                    ProjectFiles = ProjectFiles;
                }
            }
        });

        /// <summary>
        /// Git路径
        /// </summary>    
        public ICommand SelectGitPathCommand => new DelegateCommand(obj =>
        {
            var folder = ChoseFolder();
            if (!string.IsNullOrEmpty(folder))
            {
                ProjectFiles.ProjectFilesGitPath = folder;
                ProjectFiles = ProjectFiles;
            }
        });


        /// <summary>
        /// 查找本地VsCode路径
        /// </summary>
        public ICommand SelectVsCode => new DelegateCommand(obj =>
        {
            // 获取 Visual Studio Code 可执行文件位置
            string vscodePath = GetVsCodePath();
            if (string.IsNullOrEmpty(vscodePath))
            {
                MessageBox.Show("找不到 VsCode 的路径，请手动添加");
                return;
            }
            ProjectFiles.ProjectFilesOpenMethodPath = vscodePath;
            ProjectFiles = ProjectFiles;
        });

        /// <summary>
        /// 查找本地IDEA路径
        /// </summary>
        public ICommand SelectIDEA => new DelegateCommand(obj =>
        {
            
            // 获取 IDEA 可执行文件位置
            //string IDEAPath = GetIntelliJIDEAPath();
            //if (string.IsNullOrEmpty(IDEAPath))
            //{
            //    MessageBox.Show("找不到 IDEA 的路径，请手动添加");
            //    return;
            //}
            //ProjectFiles.ProjectFilesOpenMethodPath = IDEAPath;
            //ProjectFiles = ProjectFiles;
        });


        #endregion


        #region 请求
        //请求               
        public List<InfoArticle> GetNewsList()
        {
            var list = GetListData<InfoArticle>("/news/getinfoArticleList?page=1&idx=0&limit=6");
            return list;
        }

        /// <summary>
        /// 添加或修改项目配置
        /// </summary>
        private async Task<ApiResponseDataNoType> AddOrUpdateProjecteFiles()
        {
            ProjectFiles.CustomerGuid = UserInfo.ToolCustomerGuid;
            ProjectFiles.ProjectGuid = _Project.ProjectGuid;
            var res = await SendPostRequest("/ProjectFilesApi/addOrUpdateProjectFiles", ProjectFiles);
            return res;
        }


        #endregion


        #region 方法

        private void CloseDialog()
        {
            var pageInstance = new PageOpenFiles(_Project, OpenFrame);
            OpenFrame.Navigate(pageInstance);
        }


        private string ChoseFiles()
        {
            // 创建文件选择对话框实例
            var openFileDialog = new VistaOpenFileDialog();
            var selectedFilePath = "";

            // 设置对话框的属性
            openFileDialog.Title = "选择文件";
            openFileDialog.Multiselect = false; // 是否允许选择多个文件

            // 显示对话框并检查对话框的返回结果
            if (openFileDialog.ShowDialog() == true)
            {
                // 获取所选文件的路径
                selectedFilePath = openFileDialog.FileName;
            }
            return selectedFilePath;

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


        // 获取应用程序的文件位置方法
        private static string GetVsCodePath()
        {
            string installPath = GetVsCodeInstallPath();
            if (!string.IsNullOrEmpty(installPath))
            {
                return installPath;
            }

            string[] commonPaths = new string[]
            {
                @"D:\Microsoft VS Code",
                @"E:\Microsoft VS Code",
                @"F:\Microsoft VS Code",
                @"G:\Microsoft VS Code",
                // 添加其他可能的安装路径
            };

            foreach (string path in commonPaths)
            {
                if (Directory.Exists(path))
                {
                    return path + "\\Code.exe";
                }
            }

            return string.Empty;
        }

        private static string GetVsCodeInstallPath()
        {
            string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string vsCodePath = Path.Combine(localAppDataPath, "Programs", "Microsoft VS Code");

            if (Directory.Exists(vsCodePath))
            {
                return Path.Combine(vsCodePath, "Code.exe");
            }

            return string.Empty;
        }


        private static string GetIntelliJIDEAPath()
        {
            string[] commonPaths = new string[]
            {
                @"C:\",
                @"D:\",
                @"E:\",
                @"F:\",
                @"G:\",
                // 添加其他可能的安装路径
            };

            foreach (string path in commonPaths)
            {
                    string[] subDirectories = Directory.GetDirectories(path, "IntelliJ IDEA*", SearchOption.AllDirectories);

                    foreach (string directory in subDirectories)
                    {
                        string binPath = Path.Combine(directory, "bin", "idea64.exe");
                        if (File.Exists(binPath))
                        {
                            return binPath;
                        }
                    }
            }

            return string.Empty;
        }




        #endregion

    }
}
