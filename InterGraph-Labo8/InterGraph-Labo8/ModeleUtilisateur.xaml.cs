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
        const string propertyNameIsSelected = "IsSelected";
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
        bool _isSelected;
        public bool IsSelected 
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                if (value)
                    Border.Background = new SolidColorBrush(Color.FromRgb(0x80, 0x80, 0x80));
                else
                    Border.Background = new SolidColorBrush(Color.FromRgb(0x43, 0x43, 0x43));
                /*if(_isSelected != value)
                {
                    _isSelected = value;
                    DoPropertyChanged(propertyNameIsSelected);
                    Name.Text = _isSelected.ToString();
                }*/
            } 
        }

        public ModeleUtilisateur()
        {
            //IsSelected = false;
            InitializeComponent();

            //Border.DataContext = this;

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

        //public event PropertyChangedEventHandler PropertyChanged;

        /*protected virtual void DoPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }*/

        public event EventHandler SelectProfil;
        private void UserSelected_Click(object sender, RoutedEventArgs e)
        {
            SelectProfil.Invoke(this, new EventArgs());
        }
    }
}
