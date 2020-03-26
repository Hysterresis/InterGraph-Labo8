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
    /// Logique d'interaction pour UcProfilManage.xaml
    /// </summary>
    public partial class UcProfilManage : UserControl
    {
        private Profil user;
        private Profil foreman;
        private Profil admin;

        public UcProfilManage()
        {
            user = new Profil("Utilisateurs", Acreditation.Low, "Operator", "Images/059-mechanic.png");
            foreman = new Profil("Contremaitre", Acreditation.Medium, "Manager", "Images/047-foreman.png");
            admin = new Profil("Admin", Acreditation.High, "Administrator", "Images/078-programmer.png");

            InitializeComponent();

        }


        private static readonly DependencyProperty currentProfilProperty =
            DependencyProperty.Register("currentProfil", typeof(Profil), typeof(UcProfilManage));
        public Profil currentProfil
        {
            get { return (Profil)GetValue(currentProfilProperty); }
            set { SetValue(currentProfilProperty, value); }
        }

        private void UserSelected(object sender, EventArgs e)
        {
            currentProfil = user;
            User.IsSelected = true;
            Foreman.IsSelected = false;
            Admin.IsSelected = false;
            ConnectionBlock.Visibility = Visibility.Visible;
            Connection.Visibility = Visibility.Visible;
            MdpInvalid.Visibility = Visibility.Hidden;
        }

        private void AdminSelected(object sender, EventArgs e)
        {
            currentProfil = admin;
            User.IsSelected = false;
            Foreman.IsSelected = false;
            Admin.IsSelected = true;
            ConnectionBlock.Visibility = Visibility.Visible;
            Connection.Visibility = Visibility.Visible;
            MdpInvalid.Visibility = Visibility.Hidden;
        }

        private void ForemanSelected(object sender, EventArgs e)
        {
            currentProfil = foreman;
            User.IsSelected = false;
            Foreman.IsSelected = true;
            Admin.IsSelected = false;
            ConnectionBlock.Visibility = Visibility.Visible;
            Connection.Visibility = Visibility.Visible;
            MdpInvalid.Visibility = Visibility.Hidden;
        }

        private void Connection_Click(object sender, RoutedEventArgs e)
        {
            if (Password.Password != null)
            {
                if (currentProfil.ComparisonPassword(Password.Password))
                {
                    MdpInvalid.Visibility = Visibility.Hidden;
                    CurrentProfil.SourceImage = currentProfil.ImageSource;
                    CurrentProfil.TextName = currentProfil.ProfilName;
                    CurrentProfil.Visibility = Visibility.Visible;
                    ModifyPassword.Visibility = Visibility;
                    User.Visibility = Visibility.Hidden;
                    Foreman.Visibility = Visibility.Hidden;
                    Admin.Visibility = Visibility.Hidden;
                    Deconnection.Visibility = Visibility.Visible;
                    Connection.Visibility = Visibility.Hidden;
                    ConnectionBlock.Visibility = Visibility.Hidden;

                    ProfilChange_function();
                }
                else
                {
                    MdpInvalid.Visibility = Visibility.Visible;
                    Password.Password = "";
                }
            }
            else
            {
                MdpInvalid.Visibility = Visibility.Visible;
                Password.Password = "";
            }
        }


        private void Deconnection_Click(object sender, RoutedEventArgs e)
        {
            currentProfil = new Profil();
            CurrentProfil.Visibility = Visibility.Hidden;
            Deconnection.Visibility = Visibility.Hidden;
            ModifyPassword.Visibility = Visibility.Hidden;
            NewPasswordBlock.Visibility = Visibility.Hidden;
            User.Visibility = Visibility.Visible;
            Foreman.Visibility = Visibility.Visible;
            Admin.Visibility = Visibility.Visible;
            Password.Password = "";
            User.IsSelected = false;
            Foreman.IsSelected = false;
            Admin.IsSelected = false;
            currentProfil = new Profil();
            ProfilChange_function();
        }

        private void ModifyPassword_Click(object sender, RoutedEventArgs e)
        {
            ModifyPassword.Visibility = Visibility.Collapsed;
            NewPasswordBlock.Visibility = Visibility.Visible;
        }

        private void SaveNewPassword_Click(object sender, RoutedEventArgs e)
        {
            currentProfil.Password = NewPassword.Text;
            NewPassword.Text = "";
            NewPasswordBlock.Visibility = Visibility.Hidden;
            ModifyPassword.Visibility = Visibility.Visible;
        }

        public event EventHandler ProfilChange;
        private void ProfilChange_function()
        {
            ProfilChangeArgs args = new ProfilChangeArgs();
            args.profil = currentProfil;
            ProfilChange.Invoke(this, args);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NewPassword.Text = "";
            NewPasswordBlock.Visibility = Visibility.Hidden;
            ModifyPassword.Visibility = Visibility.Visible;
        }
    }

    public class ProfilChangeArgs : EventArgs
    {
        public Profil profil { get; set; }
    }
}
