using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constants

        private const string defaultMachineIP = "127.0.0.1";
        private const int defaultMachinePort = 9999;
        private const string messageConnectionErrorText = "L'action sur la machine n'a pas pu être effectué car elle est déconnecté";
        private const string messageConnectionErrorTitle = "Erreur de connexion";
        private PaintingMachineConfiguration defaultMachineConfiguration =
            new PaintingMachineConfiguration(
                10.0,
                Color.FromArgb(255, 69, 134, 191),
                Color.FromArgb(255, 125, 185, 105),
                Color.FromArgb(255, 253, 240, 2),
                Color.FromArgb(255, 242, 146, 5));
        public Profil curentProfil;

        #endregion

        #region Constructors

        public MainWindow()
        {
            curentProfil = new Profil();

            InitializeComponent();

            this.DataContext = this;
            PaintingMachine = new PaintingMachine(defaultMachineIP, defaultMachinePort,
                defaultMachineConfiguration);
            PaintingMachine.PropertyChanged += PaintingMachine_PropertyChanged;
            PaintingMachine.LoadBatchList("../../BatchList.xml");
        }

        #endregion

        #region Propreties

        public PaintingMachine PaintingMachine { get; set; }

        #endregion

        #region Methods
        #endregion

        #region Events

        private void PaintingMachine_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PaintingMachine paintingMachine = sender as PaintingMachine;
            if (e.PropertyName == nameof(PaintingMachine.Connected))
            {
                if (paintingMachine.Connected)
                {
                    Dispatcher.Invoke(() => ImgConnexionStatus.Source = new BitmapImage(new Uri("Images/connectArrow.png", UriKind.Relative)));
                }
                else
                {
                    Dispatcher.Invoke(() => ImgConnexionStatus.Source = new BitmapImage(new Uri("Images/disconnectArrow.png", UriKind.Relative)));
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result =
                MessageBox.Show("Voulez-vous arrêter la machine avant de quitter?", "Quitter",
                MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            switch (result)
            {
                case MessageBoxResult.Cancel:
                    e.Cancel = true;
                    break;
                case MessageBoxResult.Yes:
                    try
                    {
                        PaintingMachine.ProductionThread?.Abort();
                        PaintingMachine.EmergencyStop();
                        break;                    }
                    catch (System.Net.Sockets.SocketException) //la connexion est perdu
                    {
                        MessageBox.Show(
                            messageConnectionErrorText,
                            messageConnectionErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;
                case MessageBoxResult.No:
                    PaintingMachine.ProductionThread?.Abort();
                    break;
                default:
                    break;
            }
        }

        private void BtnHelp_Click(object sender, RoutedEventArgs e) { }

        private void BtnEmergencyStop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PaintingMachine.EmergencyStop();
            }
            catch (SocketException) //connection perdu
            {
                MessageBox.Show(messageConnectionErrorText,
                    messageConnectionErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        private void UcProfilManage_ProfilChange(object sender, EventArgs e)
        {
            ProfilChangeArgs args = e as ProfilChangeArgs;
            curentProfil = args.profil;
            if(curentProfil.ProfilName == null)
            {
                TabJob.Visibility = Visibility.Hidden;
                TabMachine.Visibility = Visibility.Hidden;
                TabConfiguration.Visibility = Visibility.Hidden;
            }
            else
            {
                TabJob.Visibility = Visibility.Visible;
                TabMachine.Visibility = Visibility.Visible;
                if(curentProfil.AcreditationLevel == Acreditation.High)
                {
                    TabConfiguration.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
