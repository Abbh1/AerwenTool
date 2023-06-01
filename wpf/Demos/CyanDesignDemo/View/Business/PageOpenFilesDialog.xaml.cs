using CyanDesignDemo.Model.Business.Open;
using DMSkinDemo.ViewModel.Business.Open;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// PageOpenFilesDialog.xaml 的交互逻辑
    /// </summary>
    public partial class PageOpenFilesDialog : Page
    {
        private OpenFilesDialogViewModel _OpenFilesDialogViewModel;
        public PageOpenFilesDialog(Project project,ProjectFiles projectFilesData, Frame OpenFrame)
        {
            InitializeComponent();

            _OpenFilesDialogViewModel = new OpenFilesDialogViewModel(project, projectFilesData, OpenFrame);
            this.DataContext = _OpenFilesDialogViewModel;
        }


        /// <summary>
        /// 选中Git
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            var path = _OpenFilesDialogViewModel.ProjectFiles.ProjectFilesFileOpenPath;
            _OpenFilesDialogViewModel.ProjectFiles.ProjectFilesGitPath = path;
            _OpenFilesDialogViewModel.ProjectFiles = _OpenFilesDialogViewModel.ProjectFiles;
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            _OpenFilesDialogViewModel.ProjectFiles.ProjectFilesGitPath = "";
        }
    }

}
