using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml;

namespace InterGraph_Labo8
{
    /*Attention! Les fonctions de la class PaintingMachine sont susceptible de 
    * lever les exceptions:
    *  - "SocketException" si la connexion à la machine est perdu 
    *  - "Exception" si la machine répond un message inconnu du protocol
    *  
    *  Ces exceptions devront être géré dans la class parente
    */
    public class PaintingMachine : INotifyPropertyChanged
    {

        #region PropretyChangeInterface

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void DoPropertyChanged(string preopretyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(preopretyName));
        }

        #endregion

        #region Constants
        private const int UdpTimeout = 400;
        private const string CorruptedFileMessage = "Fichier corrompu";
        #endregion

        #region Enumerations
        public enum MachineColor
        {
            None,
            PaintA,
            PaintB,
            PaintC,
            PaintD
        }

        public enum MachineStates
        {
            Stoped,
            BucketComing,
            BucketLockedAndWaiting,
            PaintAFilling,
            PaintBFilling,
            PaintCFilling,
            PaintDFilling,
            WorkDone
        }

        #endregion

        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public PaintingMachine(string ip = "0.0.0.0", int port = 0, double flow = 0.0, Color colorA = new Color(),
            Color colorB = new Color(), Color colorC = new Color(), Color colorD = new Color())
        {
            Flow = flow;
            ColorA = colorA;
            ColorB = colorB;
            ColorC = colorC;
            ColorD = colorD;
            Port = port;
            Ip = ip;
            sender = new IPEndPoint(IPAddress.Any, 0);
            BatchList = new BatchList();
            checkConnectionTimer = new Timer(CheckConnection, null, 0, 200);
            paintInjectionStopWatch = new Stopwatch();
        }

        #endregion

        #region Variables

        private UdpClient udpClient;
        private IPEndPoint sender;
        private MachineColor activeColorMemory;
        private bool conveyorOnMemory;
        private MachineStates currentStateMemory;
        Stopwatch paintInjectionStopWatch;
        Timer checkConnectionTimer;

        #endregion

        #region Propreties

        public string Ip
        {
            get { return ip; }
            set
            {
                ip = value;
                udpClient?.Close();
                // Create communication socket
                udpClient = new UdpClient(Ip, Port);
                // adjust receive time out
                udpClient.Client.SetSocketOption(SocketOptionLevel.Socket,
                    SocketOptionName.ReceiveTimeout, UdpTimeout);
            }
        }
        private string ip;
        public int Port { get; set; }
        public double ComputationPerSeconds { get; } = 1000 / 250;
        public Color ColorA { get; set; }
        public Color ColorB { get; set; }
        public Color ColorC { get; set; }
        public Color ColorD { get; set; }
        public double Flow { get; set; }
        public BatchList BatchList { get; set; }
        public MachineStates CurrentState { get; set; }

        public bool Connected
        {
            get { return connected; }
            set
            {
                connected = value;
                DoPropertyChanged(nameof(Connected));
            }
        }
        private bool connected;

        public bool ConveyorOn
        {
            get
            {
                string reply = Send("ConveyorMoving");
                if (reply == "True")
                {
                    return true;
                }
                else if (reply == "False")
                {
                    return false;
                }
                else
                {
                    throw new Exception("Invalid reply from painting machine");
                }
            }
            set
            {
                string reply;
                if (value == true)
                {
                    Send("PaintNone"); //Securité
                    reply = Send("ConveyorStart");
                }
                else
                {
                    reply = Send("ConveyorStop");
                }
                if (reply != "Ok")
                {
                    throw new Exception("Invalid reply from painting machine");
                }
            }
        }

        public bool BucketsLoading
        {
            set
            {
                string reply;
                if (value == true)
                {
                    reply = Send("EnableBucketsLoading");
                }
                else
                {
                    reply = Send("DisableBucketsLoading");
                }
                if (reply != "Ok")
                {
                    throw new Exception("Invalid reply from painting machine");
                }
            }
        }

        public bool BucketLocked
        {
            get
            {
                string reply = Send("BucketLocked");
                if (reply == "True")
                {
                    return true;
                }
                else if (reply == "False")
                {
                    return false;
                }
                else
                {
                    throw new Exception("Invalid reply from painting machine");
                }
            }
        }

        public MachineColor ActiveColor
        {
            get
            {
                return activeColor;
            }
            set
            {
                string reply;
                if (BucketLocked) //Sécurité
                {
                    switch (value)
                    {
                        case MachineColor.None:
                            reply = Send("PaintNone");
                            break;
                        case MachineColor.PaintA:
                            reply = Send("PaintA");
                            break;
                        case MachineColor.PaintB:
                            reply = Send("PaintB");
                            break;
                        case MachineColor.PaintC:
                            reply = Send("PaintC");
                            break;
                        case MachineColor.PaintD:
                            reply = Send("PaintD");
                            break;
                        default:
                            throw new Exception("Color unkown");
                    }
                    if (reply != "Ok")
                    {
                        throw new Exception("Invalid reply from painting machine");
                    }
                    activeColor = value;
                }
            }
        }
        private MachineColor activeColor;

        public Thread ProductionThread { get; set; }
        #endregion

        #region Methods

        public void CheckConnection(Object state)
        {
            try
            {
                Send("ConveyorMoving");
                Connected = true;
            }
            catch (SocketException)
            {
                lock (this) { Connected = false; }
            }
        }

        public void EmergencyStop()
        {
            try
            {
                activeColorMemory = ActiveColor;
                conveyorOnMemory = ConveyorOn;
                currentStateMemory = CurrentState;
                ActiveColor = MachineColor.None;
                ConveyorOn = false;
                paintInjectionStopWatch.Stop();
            }
            catch (SocketException)
            {
                lock (this) { Connected = false; }
            }
        }

        public void Start()
        {
            try
            {
                ActiveColor = activeColorMemory;
                ConveyorOn = conveyorOnMemory;
                CurrentState = currentStateMemory;
                paintInjectionStopWatch.Start();
            }
            catch (SocketException)
            {
                lock (this) { Connected = false; }
            }
        }

        private string Send(string message)
        {
            // clean all pending messages from receive buffer before sending command
            while (udpClient.Available > 0)
                udpClient.Receive(ref sender);
            //Send
            byte[] commandBytes = Encoding.ASCII.GetBytes(message);
            udpClient.Send(commandBytes, commandBytes.Length);
            //Receive
            byte[] answerBytes = udpClient.Receive(ref sender);
            return Encoding.ASCII.GetString(answerBytes);
        }

        public void XmlRead(XmlReader reader)
        {
            reader.ReadStartElement(nameof(PaintingMachine));
            Flow = reader.ReadElementContentAsDouble(nameof(Flow), "");
            reader.ReadStartElement("PaintColors");
            ColorA.XmlRead(reader, nameof(ColorA));
            ColorB.XmlRead(reader, nameof(ColorB));
            ColorC.XmlRead(reader, nameof(ColorC));
            ColorD.XmlRead(reader, nameof(ColorD));
            reader.ReadEndElement();
            reader.ReadEndElement();
        }
        public void XmlWrite(XmlWriter writer)
        {
            writer.WriteStartElement(nameof(PaintingMachine));
            writer.WriteElementString(nameof(Flow), Flow.ToString());
            writer.WriteStartElement("PaintColors");
            ColorA.XmlWrite(writer, nameof(ColorA));
            ColorB.XmlWrite(writer, nameof(ColorB));
            ColorC.XmlWrite(writer, nameof(ColorC));
            ColorD.XmlWrite(writer, nameof(ColorD));
            writer.WriteEndElement();
            writer.WriteEndElement();
        }
        public void ExecuteProductionAsync()
        {
            ProductionThread = new Thread(new ThreadStart(ExecuteProduction));
            ProductionThread.Start();
        }

        private void ExecuteProduction()
        {
            try
            {
                foreach (Batch batch in BatchList.Batches)
                {
                    for (int i = 0; i < batch.NumberOfElements; i++)
                    {
                        BucketsLoading = true;
                        ConveyorOn = true;
                        CurrentState = MachineStates.BucketComing;
                        while (CurrentState != MachineStates.WorkDone)
                        {
                            switch (CurrentState)
                            {
                                case MachineStates.BucketComing:
                                    if (BucketLocked)
                                    {
                                        ActiveColor = MachineColor.PaintA;
                                        paintInjectionStopWatch.Restart();
                                        CurrentState = MachineStates.PaintAFilling;
                                    }
                                    break;
                                case MachineStates.PaintAFilling:
                                    if (paintInjectionStopWatch.ElapsedMilliseconds >= Convert.ToInt32((batch.Recipe.QuantityA / Flow) * 1000))
                                    {
                                        ActiveColor = MachineColor.PaintB;
                                        paintInjectionStopWatch.Restart();
                                        CurrentState = MachineStates.PaintBFilling;
                                    }
                                    break;
                                case MachineStates.PaintBFilling:
                                    if (paintInjectionStopWatch.ElapsedMilliseconds >= Convert.ToInt32((batch.Recipe.QuantityB / Flow) * 1000))
                                    {
                                        ActiveColor = MachineColor.PaintC;
                                        paintInjectionStopWatch.Restart();
                                        CurrentState = MachineStates.PaintCFilling;
                                    }
                                    break;
                                case MachineStates.PaintCFilling:
                                    if (paintInjectionStopWatch.ElapsedMilliseconds >= Convert.ToInt32((batch.Recipe.QuantityC / Flow) * 1000))
                                    {
                                        ActiveColor = MachineColor.PaintD;
                                        paintInjectionStopWatch.Restart();
                                        CurrentState = MachineStates.PaintDFilling;
                                    }
                                    break;
                                case MachineStates.PaintDFilling:
                                    if (paintInjectionStopWatch.ElapsedMilliseconds >= Convert.ToInt32((batch.Recipe.QuantityD / Flow) * 1000))
                                    {
                                        ActiveColor = MachineColor.None;
                                        paintInjectionStopWatch.Reset();
                                        CurrentState = MachineStates.WorkDone;
                                    }
                                    break;
                                case MachineStates.Stoped:
                                    break;
                                default:
                                    throw new Exception("Comportement anormal de la machine");
                            }
                            Thread.Sleep(25);
                        }
                    }
                }
            }
            catch (SocketException)
            {
                lock (this) { Connected = false; }
            }
        }

        public void LoadBatchList(string filePath)
        {
            try
            {
                using (XmlReader reader = XmlReader.Create(filePath))
                {
                    BatchList.XmlRead(reader);
                }
            }
            catch (System.Exception e)
            {
                MessageBox.Show(CorruptedFileMessage + e.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
    }
}
