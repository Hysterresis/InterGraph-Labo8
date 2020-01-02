using System;
using System.Collections.Generic;
using System.Linq;
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
        private const double defaultMachineFlow = 10.0;
        private Color defaultMachineColorA = Color.FromArgb(255, 69, 134, 191);
        private Color defaultMachineColorB = Color.FromArgb(255, 125, 185, 105);
        private Color defaultMachineColorC = Color.FromArgb(255, 253, 240, 2);
        private Color defaultMachineColorD = Color.FromArgb(255, 242, 146, 5);

        #endregion

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;
            PaintingMachine = new PaintingMachine(defaultMachineIP, defaultMachinePort,
                defaultMachineFlow,
                defaultMachineColorA, defaultMachineColorB,
                defaultMachineColorC, defaultMachineColorD);
            PaintingMachine.PropertyChanged += PaintingMachine_PropertyChanged;
            PaintingMachine.LoadBatchList("BatchList.xml");
            PaintingMachine.ExecuteProductionAsync();
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
                        PaintingMachine.EmergencyStop();
                    }
                    catch (System.Net.Sockets.SocketException) //la connexion est perdu
                    {
                        MessageBox.Show(
                            "La machine n'a pas pu être stoppée car elle est déconnecté",
                            "Erreur de connexion", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception error) //La machine répond un message inconnu
                    {
                        MessageBox.Show(error.Message, "Erreur", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    break;
            }
        }



        private void BtnHelp_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnStartCycle_Click(object sender, RoutedEventArgs e)
        {
            PaintingMachine.Start();
        }

        private void BtnStopCycle_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnEmergencyStop_Click(object sender, RoutedEventArgs e)
        {
            PaintingMachine.EmergencyStop();
        }
        #endregion


    }
}
