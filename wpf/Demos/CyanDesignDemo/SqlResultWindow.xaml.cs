using CyanDesignDemo.Model.Business.Gen;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// SqlResultWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SqlResultWindow : Window
    {
        private GenVersion _GenVersion;
        public SqlResultWindow(string res, GenVersion version)
        {
            InitializeComponent();
            ResTextBox.Text = res;
            _GenVersion= version;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = File.ReadAllText("dataBase.txt");
            if (string.IsNullOrEmpty(connectionString))
            {
                MessageBox.Show("请先编辑数据库连接语句");
                return;
            }

            if (_GenVersion.IsMysql)
            {
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();
                        // 创建建表语句
                        string createTableSql = ResTextBox.Text;

                        // 创建命令对象
                        using (MySqlCommand command = new MySqlCommand(createTableSql, connection))
                        {
                            // 执行建表语句
                            command.ExecuteNonQuery();
                            MessageBox.Show("表创建成功！");
                            this.Hide();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"建表失败: {ex.Message}");
                }
            }
            else if (_GenVersion.IsSqlserver)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        MessageBox.Show("连接成功！");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"连接失败，请检查连接字符串: {ex.Message}");
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DataBaseWindow dataBaseWindow = new DataBaseWindow();
            dataBaseWindow.Show();
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string text = ResTextBox.Text;

            if (!string.IsNullOrEmpty(text))
            {
                Clipboard.SetText(text);
                MessageBox.Show("文本已复制到剪贴板！");
            }
            else
            {
                MessageBox.Show("文本为空！");
            }
        }
    }
}
