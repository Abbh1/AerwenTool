using CyanDesignDemo.Model;
using CyanDesignDemo.Model.Business.Login;
using CyanDesignDemo.Model.Business.Open;
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
using System.Xml.Linq;

namespace CyanDesignDemo.ViewModel.Business.Open
{
    public class OpenViewModel : ViewModelBase
    {

        // 实例化模型层
        private ProjectGroup projectGroupModel = new ProjectGroup();
        private Project projectModel = new Project();
        private ProjectForm projectFormModel = new ProjectForm();
        private ProjectGroupForm projectGroupForm = new ProjectGroupForm();

        // 跳转对象
        private Frame OpenFrame;
        private LoginModel UserInfo;

        public OpenViewModel(Frame obj)
        {
            OpenFrame = obj;
            UserInfo = Storage.GetData<LoginModel>("userInfo");
            ProjectList = GetProjectList();
        }

        #region 字段映射

        /// <summary>
        /// 项目分组树形列表
        /// </summary>
        public List<ProjectGroup> ProjectGroupTreeList
        {
            get
            {
                projectGroupModel.ProjectGroupTreeList = GetProjectGroupTreeList();
                return projectGroupModel.ProjectGroupTreeList;
            }
            set
            {
                projectGroupModel.ProjectGroupTreeList = value;
                OnPropertyChanged(nameof(ProjectGroupTreeList));
            }
        }

        /// <summary>
        /// 项目分组列表
        /// </summary>
        public List<ProjectGroup> ProjectGroupList
        {
            get
            {
                projectGroupModel.ProjectGroupList = GetProjectGroupList();
                if (projectGroupModel.ProjectGroupList != null)
                {
                    var project = projectGroupModel.ProjectGroupList.First();
                    if (project != null) ProjectFormValue.ProjectGroupGuid = projectGroupModel.ProjectGroupList.First().ProjectGroupGuid;
                }
                return projectGroupModel.ProjectGroupList;
            }
            set
            {
                projectGroupModel.ProjectGroupList = value;
                OnPropertyChanged(nameof(ProjectGroupList));
            }
        }

        /// <summary>
        /// 项目列表
        /// </summary>
        public List<Project> ProjectList
        {
            get
            {
                return projectModel.ProjectList;
            }
            set
            {
                projectModel.ProjectList = value;
                OnPropertyChanged(nameof(ProjectList));
            }
        }


        /// <summary>
        /// 项目表单值
        /// </summary>
        public ProjectForm ProjectFormValue
        {
            get
            {
                return projectFormModel;
            }
            set
            {
                projectFormModel = value;
                OnPropertyChanged(nameof(ProjectFormValue));
            }
        }

        /// <summary>
        /// 项目分组表单值
        /// </summary>
        public ProjectGroupForm ProjectGroupFormValue
        {
            get
            {
                return projectGroupForm;
            }
            set
            {
                projectGroupForm = value;
                OnPropertyChanged(nameof(ProjectGroupFormValue));
            }
        }

        private ProjectGroup selectedTreeViewItem;

        public ProjectGroup SelectedTreeViewItem
        {
            get { return selectedTreeViewItem; }
            set
            {
                selectedTreeViewItem = value;
                OnPropertyChanged(nameof(SelectedTreeViewItem));
            }
        }


        private string searchValue;

        public string SearchValue
        {
            get { return searchValue; }
            set
            {
                searchValue = value;
                OnPropertyChanged(nameof(SearchValue));
            }
        }

        #endregion


        #region 命令

        /// <summary>
        /// 打开添加项目分组弹窗
        /// </summary>
        public ICommand OpenProjectGroupDiglosCommand => new DelegateCommand(obj =>
        {
            var item = selectedTreeViewItem;

            ProjectGroupFormValue.IsShow = true;
            ProjectGroupFormValue.Title = obj.ToString();

            var IsEdit = ProjectGroupFormValue.Title == "编辑项目分组";
            if (IsEdit)
            {
                if (item == null)
                {
                    MessageBox.Show("请选选择要修改的分组");
                    return;
                }

                ProjectGroupFormValue = new ProjectGroupForm
                {
                    ProjectGroupId = item.ProjectGroupId,
                    ProjectGroupGuid = item.ProjectGroupGuid,
                    ProjectGroupParentGuid = item.ProjectGroupParentGuid,
                    ProjectGroupAncestralGuid = item.ProjectGroupAncestralGuid,
                    ProjectGroupCustomerGuid = item.ProjectGroupCustomerGuid,
                    ProjectGroupName = item.ProjectGroupName,
                    ProjectGroupSort = item.ProjectGroupSort,
                    Children = item.Children,
                    IsShow = true,
                    Title = obj.ToString()
                };
            }

            ProjectGroupFormValue = ProjectGroupFormValue;
        });

        /// <summary>
        /// TreeView取消选中
        /// </summary>
        public ICommand CancelSelectionCommand => new DelegateCommand(obj =>
        {
            SelectedTreeViewItem = null;
        });

        /// <summary>
        /// 打开添加项目弹窗
        /// </summary>
        public ICommand OpenDialogCommand => new DelegateCommand(obj =>
        {
            ProjectFormValue.IsShow = true;
            ProjectFormValue = ProjectFormValue;
        });

        /// <summary>
        /// 搜索
        /// </summary>
        public ICommand SearchCommand => new DelegateCommand(obj =>
        {
            var search = searchValue;
            Refresh();
        });

        /// <summary>
        /// 刷新页面
        /// </summary>
        public ICommand RefreshCommand => new DelegateCommand(obj =>
        {
            SearchValue = "";
            Refresh();
        });

        /// <summary>
        /// 保存项目分组
        /// </summary>
        public ICommand SaveProjectGroupCommand => new DelegateCommand(async obj =>
        {
            var IsAdd = ProjectGroupFormValue.Title == "添加项目分组";
            var item = selectedTreeViewItem;
            var msg = "";

            if (string.IsNullOrEmpty(ProjectGroupFormValue.ProjectGroupName))
            {
                MessageBox.Show("项目分组名称不能为空");
                return;
            }

            if (IsAdd)
            {
                if (item != null)
                {
                    var guid = item.ProjectGroupGuid;
                    ProjectGroupFormValue.ProjectGroupParentGuid = guid;
                }
                else
                {
                    ProjectGroupFormValue.ProjectGroupParentGuid = 0;
                }

            }
            else
            {
            }

            msg = await AddOrUpdateProjecteGroup();
            MessageBox.Show(msg);
            CloseGroupDialog();
            Refresh();
        });

        /// <summary>
        /// 保存项目
        /// </summary>
        public ICommand SaveCommand => new DelegateCommand(async obj =>
        {
            var projectFormValue = ProjectFormValue;

            if (string.IsNullOrEmpty(projectFormValue.ProjectName))
            {
                MessageBox.Show("项目名称不能为空");
                return;
            }

            var msg = await AddOrUpdateProjecte();
            MessageBox.Show(msg);
            CloseDialog();
            Refresh();
        });

        /// <summary>
        /// 删除项目分组
        /// </summary>
        public ICommand RemoveSelectedItemCommand => new DelegateCommand(async obj =>
        {
            var item = selectedTreeViewItem;
            if (item == null)
            {
                MessageBox.Show("请选择要删除的项目分组！");
                return;
            }

            MessageBoxResult result = MessageBox.Show("确定要删除该项目分组吗？（同时删除子级）", "确认删除", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                // 用户点击了确认按钮
                var msg = await DeleteProjectGroup(item.ProjectGroupId.ToString());
                MessageBox.Show(msg);
                Refresh();
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
                    ProjectFormValue.ProjectImg = url;
                    ProjectFormValue = ProjectFormValue;
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
            ProjectFormValue.ProjectImg = "";
            ProjectFormValue = ProjectFormValue;
        });

        #endregion

        #region 请求

        /// <summary>
        /// 获取项目分组树形列表
        /// </summary>
        /// <returns></returns>
        public List<ProjectGroup> GetProjectGroupTreeList()
        {
            var url = "/ProjectGroupApi/getProjectGroupTreeList?ToolCustomerGuid=" + UserInfo.ToolCustomerGuid;
            var list = GetListData<ProjectGroup>(url);
            return list;
        }

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
        /// 获取项目列表
        /// </summary>
        /// <returns></returns>
        public List<Project> GetProjectList()
        {
            var url = "/ProjectesApi/getProjectesList?ProjectName=" + searchValue + "&ToolCustomerGuid=" + UserInfo.ToolCustomerGuid;
            var list = GetListData<Project>(url);
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
        public async Task<string> AddOrUpdateProjecte()
        {
            ProjectFormValue.ProjectCustomerGuid = UserInfo.ToolCustomerGuid;
            var res = await SendPostRequest("/ProjectesApi/addOrUpdateProjectes", ProjectFormValue);
            return res.data;
        }

        /// <summary>
        /// 添加或修改项目分组
        /// </summary>
        private async Task<string> AddOrUpdateProjecteGroup()
        {
            ProjectGroupFormValue.ProjectGroupCustomerGuid = UserInfo.ToolCustomerGuid;
            var res = await SendPostRequest("/ProjectGroupApi/addOrUpdateProjectGroup", ProjectGroupFormValue);
            return res.data;
        }

        /// <summary>
        /// 删除项目分组
        /// </summary>
        private async Task<string> DeleteProjectGroup(string ids)
        {
            var res = await DeleteData("/ProjectGroupApi", ids);
            return res;
        }

        #endregion

        #region 方法

        private void CloseDialog()
        {
            ProjectFormValue = new ProjectForm();
            ProjectFormValue = ProjectFormValue;
        }

        public void Refresh()
        {
            ProjectList = GetProjectList();
            ProjectGroupTreeList = GetProjectGroupTreeList();
            ProjectGroupList = GetProjectGroupList();
        }

        private void CloseGroupDialog()
        {
            ProjectGroupFormValue = new ProjectGroupForm();
            ProjectGroupFormValue = ProjectGroupFormValue;
        }


        #endregion

    }
}
