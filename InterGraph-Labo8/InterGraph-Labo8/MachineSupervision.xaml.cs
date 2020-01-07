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
    /// Logique d'interaction pour MachineSupervision.xaml
    /// </summary>
    public partial class MachineSupervision : UserControl
    {
        public MachineSupervision()
        {
            InitializeComponent();
        }

        private static readonly DependencyProperty PaintingMachineProperty =
            DependencyProperty.Register("PaintingMachine", typeof(PaintingMachine), typeof(MachineSupervision));

        public PaintingMachine PaintingMachine
        {
            get { return (PaintingMachine)GetValue(PaintingMachineProperty); }
            set { SetValue(PaintingMachineProperty, value); }
        }

        //private void PaintingMachine_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    PaintingMachine paintingMachine = sender as PaintingMachine;
        //    if (e.PropertyName == nameof(PaintingMachine.CurrentState))
        //    {
        //        switch (PaintingMachine.CurrentState)
        //        {
        //            case PaintingMachine.MachineStates.Stoped:
        //                Dispatcher.Invoke(() => MachineStatus.Source = new BitmapImage(new Uri("Images/conveyor-stop.png", UriKind.Relative)));
        //                break;
        //            case PaintingMachine.MachineStates.BucketComing:
        //                Dispatcher.Invoke(() => MachineStatus.Source = new BitmapImage(new Uri("Images/conveyor-moving.png", UriKind.Relative)));
        //                break;
        //            case PaintingMachine.MachineStates.BucketLockedAndWaiting:
        //                Dispatcher.Invoke(() => MachineStatus.Source = new BitmapImage(new Uri("Images/conveyor-waiting.png", UriKind.Relative)));
        //                break;
        //            case PaintingMachine.MachineStates.PaintAFilling:
        //                Dispatcher.Invoke(() => MachineStatus.Source = new BitmapImage(new Uri("Images/conveyor-blue.png", UriKind.Relative)));
        //                break;
        //            case PaintingMachine.MachineStates.PaintBFilling:
        //                Dispatcher.Invoke(() => MachineStatus.Source = new BitmapImage(new Uri("Images/conveyor-green.png", UriKind.Relative)));
        //                break;
        //            case PaintingMachine.MachineStates.PaintCFilling:
        //                Dispatcher.Invoke(() => MachineStatus.Source = new BitmapImage(new Uri("Images/conveyor-yellow.png", UriKind.Relative)));
        //                break;
        //            case PaintingMachine.MachineStates.PaintDFilling:
        //                Dispatcher.Invoke(() => MachineStatus.Source = new BitmapImage(new Uri("Images/conveyor-orange.png", UriKind.Relative)));
        //                break;
        //            case PaintingMachine.MachineStates.WorkDone:
        //                Dispatcher.Invoke(() => MachineStatus.Source = new BitmapImage(new Uri("Images/conveyor-waiting.png", UriKind.Relative)));
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //}

        private void BtnStartCycle_Click(object sender, RoutedEventArgs e) => PaintingMachine.Start();

        private void BtnStopCycle_Click(object sender, RoutedEventArgs e) { }

        private void BtnReset_Click(object sender, RoutedEventArgs e) { }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //PaintingMachine.PropertyChanged += PaintingMachine_PropertyChanged;
        }
    }
}
