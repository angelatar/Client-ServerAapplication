using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MathClient
{
    /// <summary>
    /// Simple Client project with TCP and UDP
    /// </summary>
    public class MathClient
    {
        /// <summary>
        /// Field for TCP client
        /// </summary>
        private TcpClient tcp = null;

        /// <summary>
        /// Field for UDP client
        /// </summary>
        private UdpClient udp = null;

        /// <summary>
        /// User can choose service type
        /// </summary>
        public ServiceType Type { private set; get; }

        /// <summary>
        /// Server's ip address
        /// </summary>
        private IPAddress ip;

        /// <summary>
        /// Connection port
        /// </summary>
        private int port;

        /// <summary>
        /// Client's end point
        /// </summary>
        private IPEndPoint endPoint;

        /// <summary>
        /// Stream for information exchange
        /// </summary>
        private NetworkStream stream;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public MathClient(ServiceType type, IPAddress ip, int port)
        {

            this.Type = type;

            this.ip = ip;

            this.port = port;

            this.endPoint = new IPEndPoint(this.ip, this.port);

            if (this.Type == ServiceType.TCP)
            {
                this.tcp = new TcpClient();
            }
            else
            {
                this.udp = new UdpClient();
            }
        }

        /// <summary>
        /// Connect client with server
        /// </summary>
        public void Connect()
        {
            if (this.Type == ServiceType.TCP)
            {
                this.tcp.Connect(this.endPoint);
            }
            else
            {
                this.udp.Connect(this.endPoint);
            }
        }

        /// <summary>
        /// Ask questions to server
        /// </summary>
        public void Ask()
        {
            if (this.Type == ServiceType.TCP)
            {
                this.AskTCP();
            }
            else
            {
                this.AskUDP();
            }
        }

        /// <summary>
        /// Asker for TCP
        /// </summary>
        private void AskTCP()
        {
            while (true)
            {
                this.stream = this.tcp.GetStream();                
                byte[] tbuffer = new byte[1000];
                var length = stream.Read(tbuffer, 0, 1000);
                var buffer = new byte[length];
                Array.Copy(tbuffer, 0, buffer, 0, length);
                Console.WriteLine(Encoding.ASCII.GetString(buffer));

                Console.Write("Enter your question : ");
                string message = Console.ReadLine();
                
                var msg = Encoding.ASCII.GetBytes(message);
                stream.Write(msg, 0, msg.Length);

                this.stream = this.tcp.GetStream();
                length = stream.Read(tbuffer, 0, 1000);
                buffer = new byte[length];
                Array.Copy(tbuffer, 0, buffer, 0, length);
                Console.WriteLine(Encoding.ASCII.GetString(buffer));

            }
        }

        /// <summary>
        /// Asker for UDP
        /// </summary>
        private void AskUDP()
        {

            var msg = Encoding.ASCII.GetBytes("Can I ask?");
            this.udp.Send(msg, msg.Length);

            while (true)
            {
                var temp = this.udp.Receive(ref this.endPoint);
                var tempstring = Encoding.ASCII.GetString(temp);
                Console.WriteLine(tempstring);

                Console.Write("Enter your question : ");
                string message = Console.ReadLine();

                msg = Encoding.ASCII.GetBytes(message);
                this.udp.Send(msg, msg.Length);


                temp = this.udp.Receive(ref this.endPoint);
                tempstring = Encoding.ASCII.GetString(temp);
                Console.WriteLine("Answer is : " + tempstring);
            }
        }
    }
}
