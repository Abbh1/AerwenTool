using CyanDesignDemo.Model.Business.CreateTable;
using CyanDesignDemo.Model.Business.Gen;
using CyanDesignDemo.ViewModel.Business.Gen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ComboBox = System.Windows.Controls.ComboBox;

namespace CyanDesignDemo.View.Business
{
    /// <summary>
    /// PageCreateTable.xaml 的交互逻辑
    /// </summary>
    public partial class PageCreateTable : Page
    {
        public PageCreateTable()
        {
            InitializeComponent();

            var createTableViewModel = new CreateTableViewModel(CreateTableFrame);
            this.DataContext = createTableViewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TableAnnotation.Text))
            {
                System.Windows.MessageBox.Show("请先输入表注释！");
                return;
            }
            if (string.IsNullOrEmpty(TableName.Text))
            {
                System.Windows.MessageBox.Show("请先输入表名称！");
                return;
            }
            TabControler.SelectedIndex = 1;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 获取选中的项
            var selectedItem = comboBox.SelectedItem as BaseFiledTemplate;
            if (selectedItem != null)
            {
                var createTableViewModel = new CreateTableViewModel(CreateTableFrame);
                createTableViewModel.BaseFiledTemplateValue = selectedItem;
                createTableViewModel.BaseFiledTemplateValue = createTableViewModel.BaseFiledTemplateValue;
            }
        }

        private void ComboBoxType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 定义映射关系字典
            Dictionary<object, (string FiledSize, string FiledDecimal)> mapping = new Dictionary<object, (string, string)>
            {
                { "varchar", ("255", "0") },
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
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            string selectedValue = selectedItem.Content.ToString();
            var createTableViewModel = new CreateTableViewModel(CreateTableFrame);

            // 根据选中项获取对应的字段大小和字段小数位数
            if (mapping.TryGetValue(selectedValue, out var mappingValue))
            {
                createTableViewModel.GenField.FiledSize = mappingValue.FiledSize;
                createTableViewModel.GenField.FiledDecimal = mappingValue.FiledDecimal;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DataBaseWindow dataBaseWindowdataBaseWindow = new DataBaseWindow();
            dataBaseWindowdataBaseWindow.Show();
        }
    }

}
