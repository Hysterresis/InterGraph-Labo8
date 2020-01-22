using FileExplorer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// <summary>
    /// Logique d'interaction pour UcAdminPannel.xaml
    /// </summary>
    public partial class UcAdminPannel : UserControl
    {
        public UcAdminPannel()
        {
            FileExplorer = new FileExplorer.Controller.Controller();
            InitializeComponent();
            RootDirectoryExplorer.DataContext = FileExplorer;
        }

        public FileExplorer.Controller.Controller FileExplorer { get; set; }


        private static readonly DependencyProperty IpProperty = 
            DependencyProperty.Register("Ip", typeof(string), typeof(UcAdminPannel));
        public string Ip
        {
            get { return (string)GetValue(IpProperty); }
            set { SetValue(IpProperty, value); }
        }

        private static readonly DependencyProperty RootDirectoryProperty =
            DependencyProperty.Register("RootDirectory", typeof(string), typeof(UcAdminPannel));
        public string RootDirectory
        {
            get { return (string)GetValue(RootDirectoryProperty); }
            set { SetValue(RootDirectoryProperty, value); }
        }

        private void BtnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (FileExplorer.SelectedDirectory?.DirType == (int)ObjectType.Directory)
            {
                RootDirectory = FileExplorer.SelectedDirectory?.Path;
            }
            else
            {
                MessageBox.Show("Aucun dossier n'a été selectionné","PaintManager",MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            Ip = TbIP.Text;
        }
    }
}
