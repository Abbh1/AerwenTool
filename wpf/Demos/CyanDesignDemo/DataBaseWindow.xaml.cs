using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// DataBaseWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DataBaseWindow : Window
    {
        public DataBaseWindow()
        {
            InitializeComponent();

            GetValue();

        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var connectionStringTemplate = "server=IP地址;Database=数据库名称;Uid=用户名;Pwd=密码;";
                var ip = IPTextBox.Text;
                var dataBase = DataBaseTextBox.Text;
                var userName = UserNameTextBox.Text;
                var passWord = PassWordTextBox.Text;

                // 替换占位符
                var connectionString = connectionStringTemplate
                    .Replace("IP地址", ip)
                    .Replace("数据库名称", dataBase)
                    .Replace("用户名", userName)
                    .Replace("密码", passWord);

                // 写入文件
                var filePath = "dataBase.txt";

                if (File.Exists(filePath))
                {
                    // 文件存在，执行打开文件的逻辑操作
                    File.WriteAllText(filePath, connectionString);
                }
                else
                {
                    AddFile(filePath, "server=IP地址;Database=数据库名称;Uid=用户名;Pwd=密码;");
                    File.WriteAllText(filePath, connectionString);
                }

                string connectionStringText = File.ReadAllText("dataBase.txt");
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionStringText))
                    {
                        connection.Open();
                        MessageBox.Show("连接成功！");
                        this.Hide();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"连接失败，请检查连接字符串: {ex.Message}");
                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }



        // 打开文件的方法
        private void OpenFile(string filePath)
        {
            try
            {
                Process.Start(filePath);
            }
            catch (Exception ex)
            {
                // 处理异常情况
                MessageBox.Show($"无法打开文件: {ex.Message}");
            }
        }


        // 添加文件的方法
        private void AddFile(string filePath, string content)
        {
            try
            {
                // 检查文件是否已存在
                if (File.Exists(filePath))
                {
                    MessageBox.Show("文件已存在。");
                    return;
                }

                // 写入文件内容
                File.WriteAllText(filePath, content);

                // 文件添加成功后，执行打开文件操作
                OpenFile(filePath);
            }
            catch (Exception ex)
            {
                // 处理异常情况
                MessageBox.Show($"无法添加文件: {ex.Message}");
            }
        }


        public void GetValue()
        {
            var filePath = "dataBase.txt";

            if (File.Exists(filePath))
            {
                string connectionStringText = File.ReadAllText(filePath);
                var connectionValues = connectionStringText.Split(';');

                foreach (var value in connectionValues)
                {
                    var keyValue = value.Split('=');
                    if (keyValue.Length == 2)
                    {
                        var key = keyValue[0].Trim().ToLower();
                        var val = keyValue[1].Trim();

                        switch (key)
                        {
                            case "server":
                            case "data source":
                            case "ip":
                                IPTextBox.Text = val;
                                break;
                            case "database":
                                DataBaseTextBox.Text = val;
                                break;
                            case "uid":
                            case "user":
                            case "username":
                                UserNameTextBox.Text = val;
                                break;
                            case "pwd":
                            case "password":
                                PassWordTextBox.Text = val;
                                break;
                            default:
                                // Handle unrecognized key or value
                                break;
                        }
                    }
                }
            }

        }

    }
}
