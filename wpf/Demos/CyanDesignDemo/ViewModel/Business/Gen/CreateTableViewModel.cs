using CyanDesignDemo.Model;
using CyanDesignDemo.Model.Business.CreateTable;
using CyanDesignDemo.Model.Business.Gen;
using CyanDesignDemo.Model.Business.Login;
using DMSkin.Core;
using DMSkin.Core.Common;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using MessageBox = System.Windows.MessageBox;

namespace CyanDesignDemo.ViewModel.Business.Gen
{
    public class CreateTableViewModel : ViewModelBase
    {

        // 实例化模型层
        private GenTable genTable = new GenTable();
        private Translate translate = new Translate();
        private GenField genField = new GenField();
        private ObservableCollection<GenFieldItem> genFieldsList = new ObservableCollection<GenFieldItem>();
        private List<BaseFiledTemplate> _BaseFiledTemplateList = new List<BaseFiledTemplate>();
        private BaseFiledTemplate baseFiledTemplate = new BaseFiledTemplate();
        private BaseFiledTemplate baseFiledTemplateFrom = new BaseFiledTemplate();
        private GenVersion _GenVersion = new GenVersion();
        public GenFieldItem SelectedGenField { get; set; }
        public string _selectedFileType { get; set; }
        public BaseFiledTemplate _selectedBaseFiledTemplate { get; set; }


        // 跳转对象
        private Frame CreateTableFrame;
        private LoginModel UserInfo;

        public CreateTableViewModel(Frame obj)
        {
            UserInfo = Storage.GetData<LoginModel>("userInfo");

            CreateTableFrame = obj;
            _BaseFiledTemplateList = GetBaseFiledTemplateList();
            if (_BaseFiledTemplateList != null)
            {
                _selectedBaseFiledTemplate = BaseFiledTemplateList.First();
                baseFiledTemplate = BaseFiledTemplateList.First();
            }
            //baseFiledTemplate.BaseFiledTemplateGuid = baseFiledTemplate.BaseFiledTemplateList.First().BaseFiledTemplateGuid;
            //baseFiledTemplate.BaseFiledTemplateContent = baseFiledTemplate.BaseFiledTemplateList.First().BaseFiledTemplateContent;
            _selectedFileType = "varchar";
        }

        #region 字段映射

        /// <summary>
        /// 数据库版本
        /// </summary>
        public GenVersion GenVersion
        {
            get
            {
                return _GenVersion;
            }
            set
            {
                _GenVersion = value;
                OnPropertyChanged(nameof(GenVersion));
            }
        }

        /// <summary>
        /// 表信息
        /// </summary>
        public GenTable GenTable
        {
            get
            {
                return genTable;
            }
            set
            {
                genTable = value;
                OnPropertyChanged(nameof(GenTable));
            }
        }

        /// <summary>
        /// 翻译
        /// </summary>
        public Translate Translate
        {
            get
            {
                return translate;
            }
            set
            {
                translate = value;
                OnPropertyChanged(nameof(Translate));
            }
        }

        /// <summary>
        /// 字段信息
        /// </summary>
        public GenField GenField
        {
            get
            {
                return genField;
            }
            set
            {
                genField = value;
                OnPropertyChanged(nameof(GenField));
            }
        }

        /// <summary>
        /// 字段列表
        /// </summary>
        public ObservableCollection<GenFieldItem> GenFieldsList
        {
            get
            {
                return genFieldsList;
            }
            set
            {
                genFieldsList = value;
                OnPropertyChanged(nameof(GenFieldsList));
            }
        }

        /// <summary>
        /// 基础字段模板列表
        /// </summary>
        public List<BaseFiledTemplate> BaseFiledTemplateList
        {
            get
            {
                return _BaseFiledTemplateList;
            }
            set
            {
                _BaseFiledTemplateList = value;
                OnPropertyChanged(nameof(BaseFiledTemplateList));
            }
        }

        /// <summary>
        /// 基础字段模板
        /// </summary>
        public BaseFiledTemplate BaseFiledTemplateValue
        {
            get
            {
                return baseFiledTemplate;
            }
            set
            {
                baseFiledTemplate = value;
                OnPropertyChanged(nameof(BaseFiledTemplateValue));
            }
        }

        /// <summary>
        /// 基础字段模板表单值
        /// </summary>
        public BaseFiledTemplate BaseFiledTemplateFromValue
        {
            get
            {
                return baseFiledTemplateFrom;
            }
            set
            {
                baseFiledTemplateFrom = value;
                OnPropertyChanged(nameof(BaseFiledTemplateFromValue));
            }
        }

        public string SelectedFileType
        {
            get { return _selectedFileType; }
            set
            {
                if (_selectedFileType != value)
                {
                    _selectedFileType = value;
                    // 执行你的处理逻辑
                    HandleSelectedFileType(_selectedFileType);
                    OnPropertyChanged(nameof(SelectedFileType));
                }
            }
        }

        public BaseFiledTemplate SelectedBaseFiledTemplate
        {
            get { return _selectedBaseFiledTemplate; }
            set
            {
                if (_selectedBaseFiledTemplate != value)
                {
                    _selectedBaseFiledTemplate = value;
                    // 执行你的处理逻辑
                    HandleSelectedBaseFiledTemplate(_selectedBaseFiledTemplate);
                    OnPropertyChanged(nameof(SelectedBaseFiledTemplate));
                }
            }
        }

        #endregion

        #region 命令

        #region 表信息

        /// <summary>
        /// 表注释翻译
        /// </summary>    
        public ICommand TableTranslationCommand => new DelegateCommand(async obj =>
        {
            if (string.IsNullOrEmpty(GenTable.TableAnnotation))
            {
                MessageBox.Show("请先输入表注释！");
                return;
            }
            var translateRes = await Translation(GenTable.TableAnnotation);
            GenTable.TableName = translateRes;
            if (GenTable.IsTablePrefix)
            {
                GenTable.TablePrefix = translateRes + "_";
            }
            GenTable = GenTable;
        });


        /// <summary>
        /// 取消表前缀
        /// </summary>    
        public ICommand CanlePreFixCommand => new DelegateCommand(async obj =>
        {
            if (!GenTable.IsTablePrefix)
            {
                GenTable.TablePrefix = "";
            }
            GenTable = GenTable;
        });

        #endregion


        #region 字段设置

        /// <summary>
        /// 点击字段翻译
        /// </summary>    
        public ICommand TranslateFiledCommand => new DelegateCommand(async obj =>
        {
            if (Translate.TranslationText != null)
            {
                // 获取原文
                var translationText = Translate.TranslationText;
                // 获取翻译结果
                var translationResults = Translate.TranslationResults = await Translation(translationText);
                // 获取表前缀
                var tablePrefix = GenTable.TablePrefix;

                // 如果有表前缀，字段拼接表前缀
                if (GenTable.IsTablePrefix) GenField.FiledName = tablePrefix + translationResults;
                else GenField.FiledName = translationResults;

                // 原文直接赋值给 字段注释
                GenField.FiledAnnotate = translationText;

                Translate = Translate;
                GenField = GenField;
            }
            else
            {
                MessageBox.Show("请先输入您需要翻译的文字");
                return;
            }
        });


        /// <summary>
        /// 添加字段
        /// </summary>    
        public ICommand AddFiledCommand => new DelegateCommand(obj =>
        {
            if (string.IsNullOrEmpty(GenTable.TableAnnotation))
            {
                MessageBox.Show("请先输入表注释！");
                return;
            }

            // 判断是否存在重复的 FiledName
            bool isDuplicate = GenFieldsList.Any(item => item.FiledName == GenField.FiledName);

            if (isDuplicate)
            {
                MessageBox.Show("有重复字段，请确认后再试");
                return;
            }


            if (GenField != null)
            {
                var genFieldItem = new GenFieldItem
                {
                    FiledName = GenField.FiledName,
                    FiledAnnotate = GenField.FiledAnnotate,
                    FiledType = GenField.FiledType,
                    FiledSize = GenField.FiledSize,
                    FiledDecimal = GenField.FiledDecimal,
                    IsNull = GenField.IsNull,
                };

                GenFieldsList.Add(genFieldItem);
            }
        });

        /// <summary>
        /// 刷新字段列表
        /// </summary>    
        public ICommand RefreshFiledCommand => new DelegateCommand(obj =>
        {
            GenFieldsList = GenFieldsList;
        });


        /// <summary>
        /// 删除字段
        /// </summary>    
        public ICommand DeleteFiledCommand => new DelegateCommand(obj =>
        {
            var selectItem = SelectedGenField;
            if (selectItem == null)
            {
                MessageBox.Show("请先选择字段");
                return;
            }
            GenFieldsList.Remove(selectItem);
            SelectedGenField = null;
        });

        /// <summary>
        /// 清空字段
        /// </summary>    
        public ICommand EmptyFiledCommand => new DelegateCommand(obj =>
        {
            // 弹出确认框
            MessageBoxResult result = MessageBox.Show("是否要清空列表？", "确认", MessageBoxButton.YesNo);

            // 根据用户选择执行相应的操作
            if (result == MessageBoxResult.Yes)
            {
                // 清空列表
                GenFieldsList.Clear();
            }
        });

        /// <summary>
        /// 上移字段
        /// </summary>    
        public ICommand MoveUpFiledCommand => new DelegateCommand(obj =>
        {
            if (SelectedGenField == null)
                return;

            int selectedIndex = GenFieldsList.IndexOf(SelectedGenField);
            if (selectedIndex > 0)
            {
                GenFieldsList.Move(selectedIndex, selectedIndex - 1);
            }
        });

        /// <summary>
        /// 下移字段
        /// </summary>    
        public ICommand MoveDownFiledCommand => new DelegateCommand(obj =>
        {
            if (SelectedGenField == null)
                return;

            int selectedIndex = GenFieldsList.IndexOf(SelectedGenField);
            if (selectedIndex < GenFieldsList.Count - 1)
            {
                GenFieldsList.Move(selectedIndex, selectedIndex + 1);
            }
        });

        #endregion


        /// <summary>
        /// 生成Sql语句
        /// </summary>    
        public ICommand CreateFiledCommand => new DelegateCommand(obj =>
        {
            // 业务字段集合
            var businessFieldList = GenFieldsList;
            // 基础字段模板
            var baseFiledTemplate = BaseFiledTemplateValue.BaseFiledTemplateContent;

            if (businessFieldList.Count <= 0)
            {
                MessageBox.Show("请先添加业务字段");
                return;
            }
            if (string.IsNullOrEmpty(baseFiledTemplate))
            {
                MessageBox.Show("基础模板内容不能为空");
                return;
            }

            var res = HandleBuidlSql(businessFieldList, baseFiledTemplate);

            SqlResultWindow sqlResultWindow = new SqlResultWindow(res,GenVersion);
            sqlResultWindow.Show();
        });


        #region 设置

        /// <summary>
        /// 刷新基础字段模板
        /// </summary>    
        public ICommand RefreshBaseFiledTemplateCommand => new DelegateCommand(obj =>
        {
            Refresh();
        });

        /// <summary>
        /// 修改基础字段模板
        /// </summary>    
        public ICommand UpdateBaseFiledTemplateCommand => new DelegateCommand(async obj =>
        {
            var msg = await UpdateBaseFiledTemplate();
            MessageBox.Show(msg);
        });

        /// <summary>
        /// 添加基础字段模板
        /// </summary>    
        public ICommand AddBaseFiledTemplateCommand => new DelegateCommand(async obj =>
        {
            var msg = await AddBaseFiledTemplate();
            MessageBox.Show(msg);
            BaseFiledTemplateFromValue.IsShow = false;
            BaseFiledTemplateFromValue = BaseFiledTemplateFromValue;
            BaseFiledTemplateFromValue = null;
            Refresh();
        });

        /// <summary>
        /// 删除基础字段模板
        /// </summary>    
        public ICommand DeleteBaseFiledTemplateCommand => new DelegateCommand(async obj =>
        {
            // 弹出确认框
            MessageBoxResult result = MessageBox.Show($"确定要删除【{BaseFiledTemplateValue.BaseFiledTemplateName}】吗？", "确认删除", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                // 执行删除操作
                var msg = await DeleteBaseFiledTemplateApi(BaseFiledTemplateValue.BaseFiledTemplateId.ToString());
                MessageBox.Show(msg);

                // 更新列表
                Refresh();
            }
        });


        /// <summary>
        /// 连接数据库测试
        /// </summary>    
        public ICommand ConnectToDatabaseCommand => new DelegateCommand(obj =>
        {
            string connectionString = File.ReadAllText("dataBase.txt");

            if (GenVersion.IsMysql)
            {
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
            }
            else if (GenVersion.IsSqlserver)
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



        });


        /// <summary>
        /// 打开连接数据库文件
        /// </summary>    
        public ICommand OpenConnectFilesCommand => new DelegateCommand(obj =>
        {
            string filePath = "dataBase.txt";
            if (File.Exists(filePath))
            {
                // 文件存在，执行打开文件的逻辑操作
                OpenFile(filePath);
            }
            else
            {
                // 文件不存在，执行添加文件的逻辑操作
                AddFile(filePath, "server=127.0.0.1;Database=database;Uid=uid;Pwd=pwd;SslMode=none;CharSet=utf8mb4;AllowLoadLocalInfile=true;AllowUserVariables=true;");

                // 添加文件后立即打开
                OpenFile(filePath);
            }
        });

        #endregion


        #endregion



        #region 请求

        /// <summary>
        /// 获取基础字段模板列表
        /// </summary>
        /// <returns></returns>
        public List<BaseFiledTemplate> GetBaseFiledTemplateList()
        {
            var url = "/BaseFiledTemplateApi/getBaseFiledTemplateList?ToolCustomerGuid=" + UserInfo.ToolCustomerGuid;
            var list = GetListData<BaseFiledTemplate>(url);
            return list;
        }


        /// <summary>
        /// 获取基础字段模板列表
        /// </summary>
        /// <returns></returns>
        public BaseFiledTemplate GetBaseFiledTemplateDetails(long guid)
        {
            var data = Get<BaseFiledTemplate>($"/BaseFiledTemplateApi/getBaseFiledTemplateDetails?BaseFiledTemplateGuid={guid}");
            return data.data;
        }

        /// <summary>
        /// 修改基础字段模板
        /// </summary>
        private async Task<string> UpdateBaseFiledTemplate()
        {
            BaseFiledTemplateValue.BaseFiledTemplateCustomerGuid = UserInfo.ToolCustomerGuid;
            var res = await SendPostRequest("/BaseFiledTemplateApi/addOrUpdateBaseFiledTemplate", BaseFiledTemplateValue);
            return res.data;
        }

        /// <summary>
        /// 添加基础字段模板
        /// </summary>
        private async Task<string> AddBaseFiledTemplate()
        {
            BaseFiledTemplateFromValue.BaseFiledTemplateCustomerGuid = UserInfo.ToolCustomerGuid;
            var res = await SendPostRequest("/BaseFiledTemplateApi/addOrUpdateBaseFiledTemplate", BaseFiledTemplateFromValue);
            return res.data;
        }

        /// <summary>
        /// 删除基础字段模板
        /// </summary>
        private async Task<string> DeleteBaseFiledTemplateApi(string ids)
        {
            BaseFiledTemplateValue.BaseFiledTemplateCustomerGuid = UserInfo.ToolCustomerGuid;
            var res = await DeleteData("/BaseFiledTemplateApi", ids);
            return res;
        }

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

        /// <summary>
        /// 翻译
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public async Task<string> Translation(string str)
        {
            try
            {
                var tool = new BaiduTranslation("20230309001593351", "ToaS5lsJVbSXbyADnTgQ");
                var res = await tool.GetTranslation(str);

                var dst = "";
                foreach (var item in res.trans_result)
                {
                    dst = item.dst.Replace(" ", "_").ToLower();
                }
                return dst;

            }
            catch (Exception)
            {
                MessageBox.Show("翻译出错了，请稍后再试");
                throw;
            }


        }


        /// <summary>
        /// 选中基础字段模板
        /// </summary>
        /// <param name="selectedFileType"></param>
        private void HandleSelectedFileType(string selectedFileType)
        {
            // 定义映射关系字典
            Dictionary<object, (string FiledSize, string FiledDecimal)> mapping = new Dictionary<object, (string, string)>
            {
                { "varchar", ("20", "0") },
                { "int", ("1", "0") },
                { "bigint", ("0", "0") },
                { "bit", ("1", "0") },
                { "tinyint", ("1", "0") },
                { "float", ("11", "1") },
                { "decimal", ("11", "2") },
                { "text", ("0", "0") },
                { "longtext", ("0", "0") },
                { "date", ("0", "0") },
                { "datetime", ("0", "0") }
            };

            // 获取选中项
            // 根据选中项获取对应的字段大小和字段小数位数
            if (mapping.TryGetValue(selectedFileType, out var mappingValue))
            {
                GenField.FiledType = selectedFileType;
                GenField.FiledSize = mappingValue.FiledSize;
                GenField.FiledDecimal = mappingValue.FiledDecimal;
            }
            GenField = GenField;
        }

        private void HandleSelectedBaseFiledTemplate(BaseFiledTemplate selectedBaseFiledTemplate)
        {
            // 获取选中项
            BaseFiledTemplateValue = selectedBaseFiledTemplate;
        }

        public void Refresh()
        {
            BaseFiledTemplateList = GetBaseFiledTemplateList();
            SelectedBaseFiledTemplate = BaseFiledTemplateList.First();
            BaseFiledTemplateValue = BaseFiledTemplateList.First();
        }


        /// <summary>
        /// 生成SQL语句
        /// </summary>
        public string HandleBuidlSql(ObservableCollection<GenFieldItem> fieldsList, string baseFieldTemplate)
        {
            var tableName = GenTable.TableName;
            var tableNamePrefix = GenTable.TableNamePrefix;
            var tableNameSub = GenTable.TablePrefix;
            var tableAnnotate = GenTable.TableAnnotation;

            var sqlStr = "";

            foreach (var item in fieldsList)
            {
                var sqlRes = HandleSqlType(item);
                sqlStr += sqlRes;
            }

            var text = baseFieldTemplate;

            var tablFieldList = sqlStr;
            var res = text
                .Replace("${tableName}", tableName)
                .Replace("${tableNamePrefix}", tableNamePrefix)
                .Replace("${tableNameSub}", tableNameSub)
                .Replace("${tablBusinessFieldList}", tablFieldList)
                .Replace("${tableAnnotate}", tableAnnotate);

            return res;
        }

        /// <summary>
        /// 判断Sql类型
        /// </summary>
        public string HandleSqlType(GenFieldItem item)
        {
            var sqlStr = "";

            var nullStr = "";
            if (item.IsNull) nullStr = "NOT NULL";
            else nullStr = "NULL DEFAULT NULL";

            if (GenVersion.IsMysql)
            {
                switch (item.FiledType)
                {
                    case "varchar":
                        sqlStr = $"`{item.FiledName}` varchar({item.FiledSize}) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci {nullStr} COMMENT '{item.FiledAnnotate}', \r\n  ";
                        break;
                    case "text":
                        sqlStr = $"`{item.FiledName}` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci {nullStr} COMMENT '{item.FiledAnnotate}', \r\n ";
                        break;
                    case "longtext":
                        sqlStr = $"`{item.FiledName}` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci {nullStr} COMMENT '{item.FiledAnnotate}', \r\n ";
                        break;
                    case "int":
                        sqlStr = $"`{item.FiledName}` int({item.FiledSize}) {nullStr} COMMENT '{item.FiledAnnotate}', \r\n ";
                        break;
                    case "bigint":
                        sqlStr = $"`{item.FiledName}` bigint({item.FiledSize}) {nullStr} COMMENT '{item.FiledAnnotate}', \r\n ";
                        break;
                    case "decimal":
                        sqlStr = $"`{item.FiledName}` decimal({item.FiledSize}, {item.FiledDecimal}) {nullStr} COMMENT '{item.FiledAnnotate}', \r\n ";
                        break;
                    case "datetime":
                        sqlStr = $"`{item.FiledName}` datetime(0) {nullStr} COMMENT '{item.FiledAnnotate}', \r\n ";
                        break;
                    case "date":
                        sqlStr = $"`{item.FiledName}` date {nullStr} COMMENT '{item.FiledAnnotate}', \r\n ";
                        break;
                    case "tinyint":
                        sqlStr = $"`{item.FiledName}` tinyint({item.FiledSize}) {nullStr} COMMENT '{item.FiledAnnotate}', \r\n ";
                        break;
                    case "float":
                        sqlStr = $"`{item.FiledName}` float({item.FiledSize}, {item.FiledDecimal}) {nullStr} COMMENT '{item.FiledAnnotate}', \r\n ";
                        break;


                    default:
                        sqlStr = $"`{item.FiledName}` varchar({item.FiledSize}) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci {nullStr} COMMENT '{item.FiledAnnotate}', \r\n  ";
                        break;
                }
            }

            if (GenVersion.IsSqlserver)
            {

            }

            return sqlStr;
        }

        


        #endregion

    }
}
