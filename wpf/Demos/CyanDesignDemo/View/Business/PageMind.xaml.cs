using DMSkinDemo.ViewModel;
using DMSkinDemo.ViewModel.Business;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CyanDesignDemo.View.Business
{
    /// <summary>
    /// PageMind.xaml 的交互逻辑
    /// </summary>
    public partial class PageMind : Page
    {
        public PageMind()
        {
            InitializeComponent();

            var _MindMapViewModel = new MindMapViewModel();
            this.DataContext = _MindMapViewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string text = ResTextBox.Text;

            if (!string.IsNullOrEmpty(text))
            {
                Clipboard.SetText(text);
                MessageBox.Show("文本已复制到剪贴板！请打开Xmind软件，点击任意主题粘贴即可！");
            }
            else
            {
                MessageBox.Show("文本为空！");
            }
        }
    }
}
