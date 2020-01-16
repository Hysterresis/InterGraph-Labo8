using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace InterGraph_Labo8
{
    /// <summary>
    /// Logique d'interaction pour MachineSupervision.xaml
    /// </summary>
    public partial class UcMachineSupervision : UserControl
    {
        public UcMachineSupervision()
        {
            InitializeComponent();
        }

        private static readonly DependencyProperty PaintingMachineProperty =
            DependencyProperty.Register("PaintingMachine", typeof(PaintingMachine), typeof(UcMachineSupervision));

        public PaintingMachine PaintingMachine
        {
            get { return (PaintingMachine)GetValue(PaintingMachineProperty); }
            //get
            //{
            //    if (!this.Dispatcher.CheckAccess())
            //    {
            //        return (PaintingMachine)this.Dispatcher.Invoke(
            //            DispatcherPriority.Background,
            //            (DispatcherOperationCallback)delegate { 
            //                return this.GetValue(PaintingMachineProperty); 
            //            }, PaintingMachineProperty);
            //    }
            //    else
            //    {
            //        return (PaintingMachine)this.GetValue(PaintingMachineProperty);
            //    }
            //}
            set { SetValue(PaintingMachineProperty, value); }
        }

        private void PaintingMachine_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PaintingMachine paintingMachine = sender as PaintingMachine;
            if (e.PropertyName == nameof(PaintingMachine.CurrentState))
            {
                switch (PaintingMachine.CurrentState)
                {
                    case PaintingMachine.MachineStates.Stoped:
                        MachineStatus.Source = new BitmapImage(new Uri("Images/conveyor-stop.png", UriKind.Relative));
                        break;
                    case PaintingMachine.MachineStates.BucketComing:
                        MachineStatus.Source = new BitmapImage(new Uri("Images/conveyor-moving.png", UriKind.Relative));
                        break;
                    case PaintingMachine.MachineStates.BucketLockedAndWaiting:
                        MachineStatus.Source = new BitmapImage(new Uri("Images/conveyor-waiting.png", UriKind.Relative));
                        break;
                    case PaintingMachine.MachineStates.PaintAFilling:
                        MachineStatus.Source = new BitmapImage(new Uri("Images/conveyor-blue.png", UriKind.Relative));
                        break;
                    case PaintingMachine.MachineStates.PaintBFilling:
                        MachineStatus.Source = new BitmapImage(new Uri("Images/conveyor-green.png", UriKind.Relative));
                        break;
                    case PaintingMachine.MachineStates.PaintCFilling:
                        MachineStatus.Source = new BitmapImage(new Uri("Images/conveyor-yellow.png", UriKind.Relative));
                        break;
                    case PaintingMachine.MachineStates.PaintDFilling:
                        MachineStatus.Source = new BitmapImage(new Uri("Images/conveyor-orange.png", UriKind.Relative));
                        break;
                    case PaintingMachine.MachineStates.WorkDone:
                        MachineStatus.Source = new BitmapImage(new Uri("Images/conveyor-waiting.png", UriKind.Relative));
                        break;
                    default:
                        break;
                }
            }
        }

        private void BtnStartCycle_Click(object sender, RoutedEventArgs e) => PaintingMachine.StartProduction();

        private void BtnStopCycle_Click(object sender, RoutedEventArgs e) => PaintingMachine.StopCycle();

        private void BtnReset_Click(object sender, RoutedEventArgs e) => PaintingMachine.ResetProduction();

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            PaintingMachine.PropertyChanged += PaintingMachine_PropertyChanged;
        }
    }
}
