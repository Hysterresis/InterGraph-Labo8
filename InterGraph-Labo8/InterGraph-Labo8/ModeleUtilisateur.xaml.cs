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
using System.ComponentModel;
using Microsoft.Win32;

namespace InterGraph_Labo8
{
    /// <summary>
    /// Logique d'interaction pour ModeleUtilisateur.xaml
    /// </summary>
    public partial class ModeleUtilisateur : UserControl
    {
        public string TextName
        {
            get { return Name.Text; }
            set { Name.Text = value; }
        }
        public string SourceImage
        {
            set { SetValue(ImageSourceProperty, value); }
            get { return (string)GetValue(ImageSourceProperty); }
        }
        public bool isSelected { get; set; }
        public ModeleUtilisateur()
        {
            isSelected = false;
            InitializeComponent();

            DependencyPropertyDescriptor imageSourceDescriptor =
                DependencyPropertyDescriptor.FromProperty(ModeleUtilisateur.ImageSourceProperty, typeof(ModeleUtilisateur));
            imageSourceDescriptor.AddValueChanged(this, ImageSourceChange);
        }

        private void ImageSourceChange(object sender, EventArgs e)
        {
            UserImage.Source = new BitmapImage(new Uri(SourceImage, UriKind.Relative));
            Name.Text = SourceImage.ToString();
        }

        private static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("SourceImage", typeof(string), typeof(ModeleUtilisateur));
        
        private void UserSelected_Click(object sender, RoutedEventArgs e)
        {
            isSelected = true;
            //Name.Text = "true";
        }
    }
}
