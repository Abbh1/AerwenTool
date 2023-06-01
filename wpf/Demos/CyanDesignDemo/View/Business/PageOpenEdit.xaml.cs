using CyanDesignDemo.Model.Business.Open;
using CyanDesignDemo.ViewModel.Business.Open;
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
    /// PageOpenEdit.xaml 的交互逻辑
    /// </summary>
    public partial class PageOpenEdit : Page
    {

        public PageOpenEdit(Project project,Frame OpenFrame)
        {
            InitializeComponent();

            var pageOpenEdit = new OpenEditViewModel(project, OpenFrame);
            this.DataContext = pageOpenEdit;
        }



    }
}
