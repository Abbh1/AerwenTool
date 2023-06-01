using CyanDesignDemo;
using CyanDesignDemo.Model;
using CyanDesignDemo.Model.Business.DownLoad;
using CyanDesignDemo.Model.Business.Login;
using CyanDesignDemo.Model.Business.Open;
using CyanDesignDemo.View.Business;
using DMSkin.Core;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace DMSkinDemo.ViewModel.Business
{
    public class DownLoadViewModel : ViewModelBase
    {
        // 实例化模型层
        private DownloadCategory _DownloadCategory = new DownloadCategory();
        private DownloadFiles _DownloadFiles = new DownloadFiles();

        private List<DownloadCategory> _DownloadCategoryTreeList;
        private List<DownloadCategory> _DownloadCategoryList;
        private List<DownloadFiles> _DownloadFilesList;
        private LoginModel UserInfo;


        public DownLoadViewModel()
        {
            UserInfo = Storage.GetData<LoginModel>("userInfo");
            DownloadCategoryTreeList = GetDownloadCategoryTreeList();
            DownloadCategoryList = GetDownloadCategoryList();
            DownloadFilesList = GetDownloadFilesList();
        }

        #region 字段映射

        private DownloadCategory selectedTreeViewItem;

        public DownloadCategory SelectedTreeViewItem
        {
            get { return selectedTreeViewItem; }
            set
            {
                selectedTreeViewItem = value;
                OnPropertyChanged(nameof(SelectedTreeViewItem));
            }
        }

        /// <summary>
        /// 分类树形列表
        /// </summary>
        public List<DownloadCategory> DownloadCategoryTreeList
        {
            get
            {
                return _DownloadCategoryTreeList;
            }
            set
            {
                _DownloadCategoryTreeList = value;
                OnPropertyChanged(nameof(DownloadCategoryTreeList));
            }
        }

        /// <summary>
        /// 分类列表
        /// </summary>
        public List<DownloadCategory> DownloadCategoryList
        {
            get
            {
                return _DownloadCategoryList;
            }
            set
            {
                _DownloadCategoryList = value;
                OnPropertyChanged(nameof(DownloadCategoryList));
            }
        }

        /// <summary>
        /// 分类表单
        /// </summary>
        public DownloadCategory DownloadCategoryForm
        {
            get
            {
                return _DownloadCategory;
            }
            set
            {
                _DownloadCategory = value;
                OnPropertyChanged(nameof(DownloadCategoryForm));
            }
        }

        /// <summary>
        /// 文件列表
        /// </summary>
        public List<DownloadFiles> DownloadFilesList
        {
            get
            {
                return _DownloadFilesList;
            }
            set
            {
                _DownloadFilesList = value;
                OnPropertyChanged(nameof(DownloadFilesList));
            }
        }

        /// <summary>
        /// 文件表单
        /// </summary>
        public DownloadFiles DownloadFilesForm
        {
            get
            {
                return _DownloadFiles;
            }
            set
            {
                _DownloadFiles = value;
                OnPropertyChanged(nameof(DownloadFilesForm));
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
        /// 提交下载分类
        /// </summary>    
        public ICommand SaveDownloadCategoryCommand => new DelegateCommand(async obj =>
        {

            if (string.IsNullOrEmpty(DownloadCategoryForm.DownloadCategoryName))
            {
                MessageBox.Show("分类名称不能为空");
                return;
            }

            var msg = await AddDownloadCategory();
            MessageBox.Show(msg);
            CloseDownloadCategoryDialog();
            Refresh();
        });

        /// <summary>
        /// 提交下载文件
        /// </summary>    
        public ICommand AddDownloadFilesCommand => new DelegateCommand(async obj =>
        {
            var msg = await AddDownloadFiles();
            MessageBox.Show(msg);
            CloseDownloadFilesDialog();
            Refresh();
        });

        /// <summary>
        /// 刷新
        /// </summary>    
        public ICommand RefreshCommand => new DelegateCommand(obj =>
        {
            Refresh();
        });

        /// <summary>
        /// 搜索
        /// </summary>    
        public ICommand SearchCommand => new DelegateCommand(obj =>
        {
            DownloadFilesList = GetDownloadFilesList();
        });

        /// <summary>
        /// 下载
        /// </summary>    
        public ICommand DownLoadLinkCommand => new DelegateCommand(async obj =>
        {
           
        });


        /// <summary>
        /// 下载附件
        /// </summary>    
        public ICommand DownLoadAttachmentCommand => new DelegateCommand(obj =>
        {

        });

        #endregion

        #region 请求

        /// <summary>
        /// 获取下载分类列表
        /// </summary>
        /// <returns></returns>
        public List<DownloadCategory> GetDownloadCategoryTreeList()
        {
            var url = "/DownloadCategoryApi/getDownloadCategoryTreeList";
            var list = GetListData<DownloadCategory>(url);
            return list;
        }

        /// <summary>
        /// 获取下载分类列表
        /// </summary>
        /// <returns></returns>
        public List<DownloadCategory> GetDownloadCategoryList()
        {
            var url = "/DownloadCategoryApi/getDownloadCategoryList";
            var list = GetListData<DownloadCategory>(url);
            return list;
        }

        /// <summary>
        /// 获取下载文件列表
        /// </summary>
        /// <returns></returns>
        public List<DownloadFiles> GetDownloadFilesList()
        {
            var url = "";
            if(SelectedTreeViewItem == null)
            {
                url = $"/DownloadFilesApi/getDownloadFilesList?DownloadFilesName={searchValue}";
            }
            else
            {
                url = $"/DownloadFilesApi/getDownloadFilesList?DownloadFilesName={searchValue}&DownloadCategoryGuid={SelectedTreeViewItem?.DownloadCategoryGuid}";
            }
            var list = GetListData<DownloadFiles>(url);
            return list;
        }


        /// <summary>
        /// 添加下载分类
        /// </summary>
        public async Task<string> AddDownloadCategory()
        {
            DownloadCategoryForm.DownloadCategoryAuditStatus = 1;
            DownloadCategoryForm.DownloadCategorySort = 100;
            var res = await SendPostRequest("/DownloadCategoryApi/addOrUpdateDownloadCategory", DownloadCategoryForm);
            return res.data;
        }

        /// <summary>
        /// 添加下载文件
        /// </summary>
        public async Task<string> AddDownloadFiles()
        {
            DownloadFilesForm.DownloadFilesAuditStatus = 1;
            var res = await SendPostRequest("/DownloadFilesApi/addOrUpdateDownloadFiles", DownloadFilesForm);
            return res.data;
        }

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
                    DownloadFilesForm.DownloadFilesIcon = url;
                    DownloadFilesForm = DownloadFilesForm;
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
            DownloadFilesForm.DownloadFilesIcon = "";
            DownloadFilesForm = DownloadFilesForm;
        });

        #endregion

        #region 方法
        private void CloseDownloadCategoryDialog()
        {
            DownloadCategoryForm = new DownloadCategory();
            DownloadCategoryForm = DownloadCategoryForm;
        }

        private void CloseDownloadFilesDialog()
        {
            DownloadFilesForm = new DownloadFiles();
            DownloadFilesForm = DownloadFilesForm;
        }

        public void Refresh()
        {
            DownloadCategoryList = GetDownloadCategoryList();
            DownloadFilesList = GetDownloadFilesList();
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




        #endregion

    }
}
