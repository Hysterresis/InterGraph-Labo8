using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace InterGraph_Labo8
{
    /*Attention! Les fonctions de la class PaintingMachine sont susceptible de 
    * lever les exceptions:
    *  - "SocketException" si la connexion à la machine est perdu 
    *  - "Exception" si la machine répond un message inconnu du protocol
    *  
    *  Ces exceptions devront être géré dans la class parente
    */
    public class PaintingMachine
    {
        #region Constants
        private const int UdpTimeout = 400;
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
        #endregion

        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public PaintingMachine(string ip="0.0.0.0", int port=0)
        {
            Port = port;
            Ip = ip;
            // create object to get remote adress when receiving data
            sender = new IPEndPoint(IPAddress.Any, 0);
        }

        #endregion

        #region Variables

        private UdpClient udpClient;
        private IPEndPoint sender;

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

        public bool Connected
        {
            get
            {
                try
                {
                    Send("ConveyorMoving");
                }
                catch (SocketException)
                {
                    return false;
                }
                return true;
            }
        }

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

        public bool BucketsLoadingEnabled
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
                }
            }
        }

        #endregion

        #region Methods

        public void EmergencyStop()
        {
            this.ActiveColor = MachineColor.None;
            this.ConveyorOn = false;
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

        #endregion
    }
}
