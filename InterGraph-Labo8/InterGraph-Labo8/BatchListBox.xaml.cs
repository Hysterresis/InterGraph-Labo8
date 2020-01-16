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

namespace InterGraph_Labo8
{
    /// <summary>
    /// Logique d'interaction pour BatchListBox.xaml
    /// </summary>
    public partial class BatchListBox : UserControl
    {
        public BatchListBox()
        {
            InitializeComponent();
        }

        private static readonly DependencyProperty BatchListProperty =
            DependencyProperty.Register("BatchList", typeof(BatchList), typeof(BatchListBox));

        public BatchList BatchList
        {
            get { return (BatchList)GetValue(BatchListProperty); }
            set { SetValue(BatchListProperty, value); }
        }


    }
}
