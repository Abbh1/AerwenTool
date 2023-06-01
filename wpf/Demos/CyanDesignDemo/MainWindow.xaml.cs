using DMSkinDemo.ViewModel;
using System.Windows;
using System.Windows.Controls;
using CyanDesignDemo.View.Business;

namespace CyanDesignDemo
{
    public partial class MainWindow
    {
        private MainViewModel mainViewModel;


        public MainWindow()
        {
            InitializeComponent();

            mainViewModel = new MainViewModel(MainFrame,this);

            // 默认跳转
            var page = new PageOpen();
            MainFrame.Navigate(page);

            this.DataContext = mainViewModel;
        }


    }
}
