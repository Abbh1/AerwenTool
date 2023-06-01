using CyanDesignDemo.Model.Business.CreateTable;
using CyanDesignDemo.Model.Business.Login;
using CyanDesignDemo.Model.Business.Open;
using CyanDesignDemo.ViewModel.Business.Login;
using DMSkin.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using System.Windows.Shapes;

namespace CyanDesignDemo
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();

            string filePath = "userdata.json";

            if (File.Exists(filePath))
            {
                // 文件存在
                string readJson = File.ReadAllText("userdata.json");
                var radUserData = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginModel>(readJson);
                var isExpirated = DateTime.Today > radUserData.ExpirationDate;

                if (!isExpirated)
                {
                    Storage.SaveData("userInfo", radUserData);
                    Storage.SaveData("jwt", radUserData.jwt);
                    MainWindow mainWindow = new MainWindow();
                    this.Hide();
                    mainWindow.Show();
                    return;
                }

            }

            var loginViewModel = new LoginViewModel();
            this.DataContext = loginViewModel;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var viewModelBase = new ViewModelBase();
            var loginModel = new LoginModel();

            var userNme = UserName.Text;
            var password = PasswordBox1.Password;
            if (string.IsNullOrEmpty(userNme))
            {
                MessageBox.Show("用户名不能为空");
                return;
            }
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("密码不能为空");
                return;
            }

            loginModel.ToolCustomerName = userNme;
            loginModel.ToolCustomerPassword = password;
            var jwt = "";

            if (SaveBtn.Content.ToString() == "登录")
            {
                var res = await viewModelBase.SendPostRequest("/Login/login", loginModel);
                try
                {
                    if (res.code == 200)
                    {
                        jwt = res.data;
                        if (!string.IsNullOrEmpty(jwt))
                        {
                            await WriteFiles(loginModel, jwt);
                        }
                        MessageBox.Show(res.msg);
                    }
                    else
                    {
                        MessageBox.Show(res.msg);
                    }

                }
                catch (Exception)
                {

                    throw;
                }
            }
            else
            {

                try
                {
                    var res = await viewModelBase.SendPostRequest("/Login/register", loginModel);

                    if (res.code == 200)
                    {
                        jwt = res.data;
                        if (!string.IsNullOrEmpty(jwt))
                        {
                            await WriteFiles(loginModel, jwt);
                            var viewModel = new ViewModelBase();
                            var resData = await viewModelBase.SendPostRequest<LoginModel>("/Login/getUserInfo", loginModel);
                            var resUserData = resData.data;

                            var ProjectGroupFormValue = new ProjectGroup
                            {
                                ProjectGroupName = "默认分组",
                                ProjectGroupCustomerGuid = resUserData.ToolCustomerGuid,
                                ProjectGroupParentGuid = 0
                            };
                            await viewModel.SendPostRequest("/ProjectGroupApi/addOrUpdateProjectGroup", ProjectGroupFormValue);

                            var baseFiledTemplate = new BaseFiledTemplate
                            {
                                BaseFiledTemplateCustomerGuid = resUserData.ToolCustomerGuid,
                                BaseFiledTemplateName = "Mysql",
                                BaseFiledTemplateContent = "CREATE TABLE `${tableNamePrefix}${tableName}`  (\r\n  `${tableNameSub}id` int(11) NOT NULL AUTO_INCREMENT,\r\n  `${tableNameSub}guid` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,\r\n\r\n  ${tablBusinessFieldList}\r\n\r\n  `${tableNameSub}create_time` datetime(0) NULL DEFAULT NULL,\r\n  `${tableNameSub}create_user_guid` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,\r\n  `${tableNameSub}update_time` datetime(0) NULL DEFAULT NULL,\r\n  `${tableNameSub}update_user_guid` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,\r\n  `${tableNameSub}delete_time` datetime(0) NULL DEFAULT NULL,\r\n  `${tableNameSub}delete_user_guid` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,\r\n  PRIMARY KEY (`${tableNameSub}id`)\r\n) COMMENT = '${tableAnnotate}'",
                            };
                            await viewModelBase.SendPostRequest("/BaseFiledTemplateApi/addOrUpdateBaseFiledTemplate", baseFiledTemplate);

                        }

                        MessageBox.Show(res.msg);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                
            }
            if (jwt == null)
            {
                return;
            }

            MainWindow mainWindow = new MainWindow();
            this.Hide();
            mainWindow.Show();
        }

        /// <summary>
        /// 注册跳转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Title.Content = "注册";
            RegisterBox.Visibility = Visibility.Hidden;
            Back.Visibility = Visibility.Visible;
            SaveBtn.Content = "注册";
        }

        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Back_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Title.Content = "登录";
            RegisterBox.Visibility = Visibility.Visible;
            Back.Visibility = Visibility.Hidden;
            SaveBtn.Content = "登录";
        }

        private async Task WriteFiles(LoginModel loginModel, string jwt)
        {
            var viewModel = new ViewModelBase();
            var user = await viewModel.SendPostRequest<LoginModel>("/Login/getUserInfo", loginModel);
            var radUserData = user.data;
            radUserData.ExpirationDate = DateTime.Now.AddDays(3);
            radUserData.jwt = jwt;
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(radUserData);
            File.WriteAllText("userdata.json", json);
            Storage.SaveData("userInfo", radUserData);
            Storage.SaveData("jwt", jwt);
        }

    }
}
