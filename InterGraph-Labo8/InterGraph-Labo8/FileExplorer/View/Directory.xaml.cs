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
using FileExplorer.Model;
using FileExplorer.Controller;


namespace FileExplorer.View
{
    /// <summary>
    /// Interaction logic for Directory.xaml
    /// </summary>
    public partial class Directory : UserControl
    {
        public Directory()
        {
            InitializeComponent();
        }

        private void DirectoryTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ////Perform actions when SelectedItem changes
            DirInfo dir = e.NewValue as DirInfo;

            Controller.Controller controller = DataContext as Controller.Controller;
            controller.SelectedDirectory = dir;
        }
    }
}
