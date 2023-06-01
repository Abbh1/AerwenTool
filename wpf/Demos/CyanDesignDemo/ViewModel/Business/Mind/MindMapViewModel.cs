using CyanDesignDemo;
using CyanDesignDemo.Model;
using CyanDesignDemo.Model.Business.Gen;
using CyanDesignDemo.Model.Business.Mind;
using CyanDesignDemo.View.Business;
using DMSkin.Core;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace DMSkinDemo.ViewModel.Business
{
    public class MindMapViewModel : ViewModelBase
    {
        // 实例化模型层
        private MindTable _MindTable = new MindTable();
        private MindTableField _MindTableField = new MindTableField();
        private ObservableCollection<MindTable> _MindTableList = new ObservableCollection<MindTable>();


        private ObservableCollection<MindTable> _SelectedTables = new ObservableCollection<MindTable>();
        private bool _IsShow { get; set; }
        private string _MindRes { get; set; }

        public MindMapViewModel()
        {
        }

        #region 字段映射

        /// <summary>
        /// 数据表列表
        /// </summary>
        public ObservableCollection<MindTable> MindTableList
        {
            get
            {
                return _MindTableList;
            }
            set
            {
                _MindTableList = value;
                OnPropertyChanged(nameof(MindTableList));
            }
        }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow
        {
            get
            {
                return _IsShow;
            }
            set
            {
                _IsShow = value;
                OnPropertyChanged(nameof(IsShow));
            }
        }

        /// <summary>
        /// 转换结果
        /// </summary>
        public string MindRes
        {
            get
            {
                return _MindRes;
            }
            set
            {
                _MindRes = value;
                OnPropertyChanged(nameof(MindRes));
            }
        }

        /// <summary>
        /// 选中的表格
        /// </summary>
        public ObservableCollection<MindTable> SelectedTables
        {
            get
            {
                return _SelectedTables;
            }
            set
            {
                _SelectedTables = value;
                OnPropertyChanged(nameof(SelectedTables));
            }
        }

        /// <summary>
        /// 搜索
        /// </summary>
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
        /// 搜索
        /// </summary>    
        public ICommand SearchCommand => new DelegateCommand(obj =>
        {
            string searchValue = SearchValue; // 获取搜索框的值，假设为字符串类型
            ObservableCollection<MindTable> mindTableList = MindTableList; // 获取完整的数据源

            // 进行模糊查询
            var searchResult = mindTableList.Where(table =>
                table.MindTableName.Contains(searchValue) || table.MindTableAnnotation.Contains(searchValue));

            // 创建临时集合并将查询结果赋值给临时集合
            ObservableCollection<MindTable> tempMindTableList = new ObservableCollection<MindTable>(searchResult);

            // 将临时集合赋值给 MindTableList，以更新界面上的数据
            MindTableList = tempMindTableList;
        });


        /// <summary>
        /// 刷新
        /// </summary>    
        public ICommand RefreshCommand => new DelegateCommand(obj =>
        {
            SearchValue = "";
            MindTableList = new ObservableCollection<MindTable>(); // 获取完整的数据源
            GetDataBaeTableList();
        });


        /// <summary>
        /// 转换
        /// </summary>    
        public ICommand ChangeCommand => new DelegateCommand(obj =>
        {
            if (SelectedTables.Count <= 0)
            {
                MessageBox.Show("请选择要转换的表！");
                return;
            }

            var res_str = "";
            int index = 0;

            // SelectedTables选中的表信息
            foreach (var item in SelectedTables)
            {
                if (!string.IsNullOrEmpty(item.MindTableAnnotation))
                {
                    res_str += $"\n{item.MindTableAnnotation}\n\t{item.MindTableName}";
                    if (item.MindTableFieldList.Count > 0)
                    {
                        foreach (var field in item.MindTableFieldList)
                        {
                            if (string.IsNullOrEmpty(field.MindTableFieldAnnotation))
                            {
                                res_str += $"\n\t\t{field.MindTableFieldName}";
                            }
                            else
                            {
                                field.MindTableFieldAnnotation = field.MindTableFieldAnnotation.Replace("，", ",");
                                field.MindTableFieldAnnotation = field.MindTableFieldAnnotation.Replace("]", ")");
                                field.MindTableFieldAnnotation = field.MindTableFieldAnnotation.Replace("[", "(");
                                res_str += $"\n\t\t{field.MindTableFieldName}\n\t\t\t{field.MindTableFieldType}({field.MindTableFieldSize},{field.MindTableFieldDecimal})\n\t\t\t\t{field.IsNull}\n\t\t\t\t\t{field.MindTableFieldAnnotation}";
                            }
                        }
                    }
                }
                else
                {
                    res_str += $"\n{item.MindTableName}";
                    if (item.MindTableFieldList.Count > 0)
                    {
                        foreach (var field in item.MindTableFieldList)
                        {
                            if (string.IsNullOrEmpty(field.MindTableFieldAnnotation))
                            {
                                res_str += $"\n\t{field.MindTableFieldName}";
                            }
                            else
                            {
                                field.MindTableFieldAnnotation = field.MindTableFieldAnnotation.Replace("，", ",");
                                field.MindTableFieldAnnotation = field.MindTableFieldAnnotation.Replace("]", ")");
                                field.MindTableFieldAnnotation = field.MindTableFieldAnnotation.Replace("[", "(");
                                res_str += $"\n\t{field.MindTableFieldName}\n\t\t{field.MindTableFieldType}({field.MindTableFieldSize},{field.MindTableFieldDecimal})\n\t\t\t{field.IsNull}\n\t\t\t\t{field.MindTableFieldAnnotation}";
                            }
                        }
                    }
                }

                // 增加索引下标
                index++;
            }

            MindRes = res_str;
        });


        /// <summary>
        /// 获取需要转换的表
        /// </summary>    
        public ICommand GetChangeTableCommand => new DelegateCommand(obj =>
        {
            SelectedTables = new ObservableCollection<MindTable>();

            // 遍历 MindTableList，获取选中的表
            foreach (MindTable table in MindTableList)
            {
                if (table.IsSelected)
                {
                    SelectedTables.Add(table);
                }
            }
            if (SelectedTables.Count <= 0)
            {
                MessageBox.Show("请选择要转换的表！");
                return;
            }

            // 创建一个列表，用于存储选中的表的表名
            List<string> selectedTableNames = SelectedTables.Select(table => $"【{table.MindTableName}】").ToList();

            // 使用逗号分隔列表中的表名
            string concatenatedTableNames = string.Join(Environment.NewLine, selectedTableNames);

            MessageBox.Show($"已选择{Environment.NewLine}{concatenatedTableNames}");
            IsShow = false;
        });

        /// <summary>
        /// 选择全部数据库
        /// </summary>    
        public ICommand GetAllTableCommand => new DelegateCommand(obj =>
        {
            SelectedTables = new ObservableCollection<MindTable>();

            GetDataBaeTableList();
            SelectedTables = MindTableList;
            if (SelectedTables.Count <= 0)
            {
                MessageBox.Show("请选择要转换的表！");
                return;
            }

            MessageBox.Show($"已选择所有表");
            IsShow = false;
        });


        /// <summary>
        /// 连接数据库测试
        /// </summary>    
        public ICommand ConnectToDatabaseCommand => new DelegateCommand(obj =>
        {
            string connectionString = File.ReadAllText("dataBase.txt");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MessageBox.Show("连接成功！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"连接失败，请检查连接字符串: {ex.Message}");
            }
        });





        /// <summary>
        /// 打开连接数据库文件
        /// </summary>    
        public ICommand OpenConnectFilesCommand => new DelegateCommand(obj =>
        {
            DataBaseWindow dataBaseWindow = new DataBaseWindow();
            dataBaseWindow.Show();
        });


        /// <summary>
        /// 打开选择数据库弹窗
        /// </summary>    
        public ICommand OpenSelectTableCommand => new DelegateCommand(obj =>
        {
            IsShow = true;
            GetDataBaeTableList();
        });

        #endregion

        #region 请求
        #endregion

        #region 方法

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

        private void GetDataBaeTableList()
        {
            MindTableList.Clear();
            try
            {
                string connectionString = File.ReadAllText("dataBase.txt");
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // 获取指定数据库中的所有表名
                    string databaseName = connection.Database; // 获取当前连接的数据库名称
                    DataTable tablesSchema = connection.GetSchema("Tables", new[] { null, databaseName });
                    List<MindTable> tables = new List<MindTable>();
                    foreach (DataRow row in tablesSchema.Rows)
                    {
                        string tableName = row["TABLE_NAME"].ToString();
                        string tableComment = row["TABLE_COMMENT"].ToString();
                        MindTable mindTable = new MindTable
                        {
                            MindTableName = tableName,
                            MindTableAnnotation = tableComment,
                            MindTableFieldList = new ObservableCollection<MindTableField>()
                        };
                        tables.Add(mindTable);
                    }

                    // 遍历每个表，获取表的字段信息
                    foreach (var table in tables)
                    {
                        // 获取表的字段信息
                        DataTable columnsSchema = connection.GetSchema("Columns", new[] { null, databaseName, table.MindTableName });
                        DataView columnsView = new DataView(columnsSchema);
                        columnsView.Sort = "ORDINAL_POSITION ASC";
                        foreach (DataRowView rowView in columnsView)
                        {
                            DataRow row = rowView.Row;
                            string columnName = row["COLUMN_NAME"].ToString();
                            // 判断当前字段名是否已存在于列表中，如果存在则跳过该次循环
                            if (table.MindTableFieldList.Any(field => field.MindTableFieldName == columnName))
                            {
                                continue;
                            }
                            string columnType = row["DATA_TYPE"].ToString();
                            string columnSize = (row["CHARACTER_MAXIMUM_LENGTH"] ?? 0).ToString();
                            string columnDecimalDigits = (row["NUMERIC_SCALE"] ?? 0).ToString();
                            string columnIsNullAble = row["IS_NULLABLE"].ToString();
                            string columnComment = row["COLUMN_COMMENT"].ToString();
                            // 其他字段信息，根据需要添加到 MindTableField 对象中


                            if (string.IsNullOrEmpty(columnSize))
                            {
                                columnSize = "0";
                            }

                            if (string.IsNullOrEmpty(columnDecimalDigits))
                            {
                                columnDecimalDigits = "0";
                            }
                            columnIsNullAble = (columnIsNullAble == "YES") ? "可以为空" : "不能为空";


                            // 创建 MindTableField 对象
                            MindTableField mindTableField = new MindTableField
                            {
                                MindTableFieldName = columnName,
                                MindTableFieldType = columnType,
                                MindTableFieldAnnotation = columnComment,
                                MindTableFieldSize = columnSize,
                                MindTableFieldDecimal= columnDecimalDigits,
                                IsNull = columnIsNullAble
                                // 其他字段信息赋值
                            };


                            // 将 MindTableField 添加到 MindTable 的字段列表中
                            table.MindTableFieldList.Add(mindTableField);
                        }

                        // 将 MindTable 添加到 _MindTableList 中
                        MindTableList.Add(table);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"连接失败，请检查连接字符串: {ex.Message}");
            }

        }

        #endregion

    }
}
