using CyanDesignDemo;
using CyanDesignDemo.Model;
using CyanDesignDemo.View.Business;
using DMSkin.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace DMSkinDemo.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        // 实例化模型层
        private MainModel mainModel = new MainModel();

        // 跳转对象
        private Frame MainFrame;
        private MainWindow MainWindow;

        public MainViewModel(Frame obj,MainWindow mainWindow)
        {
            MainFrame = obj;
            MainWindow = mainWindow;
        }

        #region 字段映射
        /// <summary>
        /// 保存的文字
        /// </summary>
        public string Name
        {
            get
            {
                return mainModel.Name;
            }
            set
            {
                mainModel.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        #endregion

        #region 命令

        /// <summary>
        /// 登出
        /// </summary>    
        public ICommand LogOutCommand => new DelegateCommand(async obj =>
        {
            string filePath = "userdata.json";
            File.Delete(filePath);


            MainWindow.Hide();
            Login login = new Login();
            login.Show();
        });

        /// <summary>
        /// 保存
        /// </summary>    
        public ICommand SaveCommand => new DelegateCommand(obj =>
        {
            Name = "你好";
        });

        /// <summary>
        /// 菜单跳转
        /// </summary>
        public ICommand NavCommand => new DelegateCommand(obj =>
        {
            if (obj == null) throw new Exception("未找到对象");

            // 隐藏返回按钮
            MainFrame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;

            var menuName = obj.ToString();
            Name = menuName;

            Type pageType = Assembly.GetExecutingAssembly().GetType("CyanDesignDemo.View.Business." + menuName);
            if (pageType != null)
            {
                object pageInstance = Activator.CreateInstance(pageType);
                MainFrame.Navigate(pageInstance);
            }
        });

        #endregion

        #region 请求
        //请求               
        public List<InfoArticle> GetNewsList()
        {
            var list = GetListData<InfoArticle>("/news/getinfoArticleList?page=1&idx=0&limit=6");
            return list;
        }
        #endregion

        #region 方法
        #endregion

    }
}
