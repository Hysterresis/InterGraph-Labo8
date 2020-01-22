using FileExplorer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InterGraph_Labo8
{
    public class LoadingBatchListEventArgs : EventArgs
    {
        public string Path { get; set; }
    }

    /// <summary>
    /// Logique d'interaction pour BatchListBox.xaml
    /// </summary>
    public partial class UcBatchListBox : UserControl
    {
        public UcBatchListBox()
        {
            FileExplorer = new FileExplorer.Controller.Controller(new FileExplorer.Model.DirInfo(new DirectoryInfo(@"../../XML")));
            InitializeComponent();
            BatchListFileExplorer.DataContext = FileExplorer;

            DependencyPropertyDescriptor rootDirectoryDescriptor =
                DependencyPropertyDescriptor.FromProperty(UcBatchListBox.RootDirectoryProperty,
                typeof(UcBatchListBox));
            rootDirectoryDescriptor.AddValueChanged(this, RootDirectoryChange);
        }

        /*Fonction appelée quaud une propriété est modifiée afin de pouvoir faire les 
        * modifications nécéssaires dans la View*/
        private void RootDirectoryChange(object sender, EventArgs e)
        {
            FileExplorer.RootDirectory = new FileExplorer.Model.DirInfo(new DirectoryInfo(RootDirectory));
            BatchListFileExplorer.DataContext = null;
            BatchListFileExplorer.DataContext = FileExplorer;
        }


        public FileExplorer.Controller.Controller FileExplorer { get; set; }

        private static readonly DependencyProperty BatchListProperty =
            DependencyProperty.Register("BatchList", typeof(BatchList), typeof(UcBatchListBox));
        public BatchList BatchList
        {
            get { return (BatchList)GetValue(BatchListProperty); }
            set { SetValue(BatchListProperty, value); }
        }

        private static readonly DependencyProperty RootDirectoryProperty =
            DependencyProperty.Register("RootDirectory", typeof(string), typeof(UcBatchListBox));
        public string RootDirectory
        {
            get { return (string)GetValue(RootDirectoryProperty); }
            set { SetValue(RootDirectoryProperty, value); }
        }

        public event EventHandler LoadingBatchList;
        private void BtnLoadXML_Click(object sender, RoutedEventArgs e)
        {
            if (FileExplorer.SelectedDirectory?.DirType == (int)ObjectType.File)
            {
                LoadingBatchListEventArgs args = new LoadingBatchListEventArgs();
                args.Path = FileExplorer.SelectedDirectory.Path;
                LoadingBatchList?.Invoke(this, args);
            }
        }

        private void BtnCloseLoadBloc_Click(object sender, RoutedEventArgs e)
        {
            LoadFileBloc.Visibility = Visibility.Collapsed;
            BtnLoadBloc_Open.Visibility = Visibility.Visible;
        }

        private void BtnLoadBloc_Open_Click(object sender, RoutedEventArgs e)
        {
            BtnLoadBloc_Open.Visibility = Visibility.Hidden;
            LoadFileBloc.Visibility = Visibility.Visible;
        }
    }
}
