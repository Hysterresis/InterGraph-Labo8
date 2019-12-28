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
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constants

        private const string defaultMachineIP = "127.0.0.1";
        private const int defaultMachinePort = 9999;

        #endregion

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
            PaintingMachine = new PaintingMachine(defaultMachineIP, defaultMachinePort);
        }

        #endregion

        #region Propreties

        public PaintingMachine PaintingMachine { get; set; }

        #endregion

        #region Methods
        #endregion

        #region Events

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

        #endregion
    }
}
