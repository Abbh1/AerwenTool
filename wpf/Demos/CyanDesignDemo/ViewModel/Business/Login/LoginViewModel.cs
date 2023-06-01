using CyanDesignDemo.Model;
using CyanDesignDemo.Model.Business.Login;
using DMSkin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace CyanDesignDemo.ViewModel.Business.Login
{
    public class LoginViewModel: ViewModelBase
    {
        // 实例化模型层
        private LoginModel loginModel = new LoginModel();


        public LoginViewModel()
        {
        }

        #region 字段映射
        public string title { get; set; } = "登录";

        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public LoginModel LoginForm
        {
            get
            {
                return loginModel;
            }
            set
            {
                loginModel = value;
                OnPropertyChanged(nameof(LoginForm));
            }
        }

        #endregion

        #region 命令
        /// <summary>
        /// 保存
        /// </summary>    
        public ICommand SaveCommand => new DelegateCommand(obj =>
        {
            var userName = LoginForm.ToolCustomerName;

        });

        /// <summary>
        /// 菜单跳转
        /// </summary>
        public ICommand NavCommand => new DelegateCommand(obj =>
        {

        });

        #endregion

        #region 请求
        //请求               
        //public List<InfoArticle> GetNewsList()
        //{
        //    var list = GetListData<InfoArticle>("/news/getinfoArticleList?page=1&idx=0&limit=6");
        //    return list;
        //}
        #endregion

        #region 方法
        #endregion


    }
}
