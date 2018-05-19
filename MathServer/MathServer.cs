using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MathServer
{
    /// <summary>
    /// Simple Server project with TCP and UDP
    /// </summary>
    public class MathServer : IMathService
    {
        /// <summary>
        /// Tools for TCP server
        /// </summary>
        private TcpListener tcp = null;
        private TcpClient tcpClient;

        /// <summary>
        /// Tool for UDP server
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
        /// Server's end point
        /// </summary>
        private IPEndPoint endPoint;

        /// <summary>
        /// Stream for information exchange
        /// </summary>
        private NetworkStream stream;

        /// <summary>
        /// Ctor 
        /// </summary>
        /// <param name="type">Service type</param>
        public MathServer(ServiceType type)
        {
            this.Type = type;

            this.ip = IPAddress.Any;

            this.port = 4500;

            this.endPoint = new IPEndPoint(this.ip, this.port);

            if (this.Type == ServiceType.TCP)
            {
                this.tcp = new TcpListener(this.endPoint);
            }
            else
            {
                this.udp = new UdpClient(port);
            }
        }

        /// <summary>
        /// Server start waiting questions
        /// </summary>
        public void Start()
        {
            if (this.Type == ServiceType.TCP)
                this.TCPListen();
            else
                this.UDPListen();
        }

        /// <summary>
        /// Listener for TCP server
        /// </summary>
        private void TCPListen()
        {
            this.tcp = new TcpListener(this.endPoint);

            this.tcp.Start();
            Console.WriteLine("Server started");


            this.tcpClient = this.tcp.AcceptTcpClient();
            Console.WriteLine("Client is accepted!");

            while (true)
            {
                this.stream = this.tcpClient.GetStream();

                var msg = Encoding.ASCII.GetBytes("Server is waiting!/nProtoclo syntax : operator: first_value:second_value");
                stream.Write(msg,0, msg.Length);

                this.stream = this.tcpClient.GetStream();
                byte[] tbuffer = new byte[1000];
                var length = stream.Read(tbuffer, 0, 1000);
                var buffer = new byte[length];
                Array.Copy(tbuffer, 0, buffer, 0, length);
                string message = Encoding.ASCII.GetString(buffer);
                Console.WriteLine("Question : " + message);

                double temp = this.Calculator(message);

                msg = Encoding.ASCII.GetBytes("Answer is : "+temp);
                stream.Write(msg, 0, msg.Length);
            }
        }

        /// <summary>
        /// Listener for UDP server
        /// </summary>
        private void UDPListen()
        {
            var tempEndP = new IPEndPoint(IPAddress.Any, 4500);
            var temp = this.udp.Receive(ref tempEndP);
            var tempstring = Encoding.ASCII.GetString(temp);
            Console.WriteLine(tempstring);

            Console.WriteLine("Server started");
            while (true)
            {
                var msg = Encoding.ASCII.GetBytes("Server is waiting!/nProtoclo syntax : operator: first_value:second_value");
                this.udp.Send(msg, msg.Length, tempEndP);


                temp = this.udp.Receive(ref tempEndP);
                tempstring = Encoding.ASCII.GetString(temp);

                Console.WriteLine("Question : " + tempstring);

                var ans = this.Calculator(tempstring);

                var ansmsg = Encoding.ASCII.GetBytes(ans.ToString());
                this.udp.Send(ansmsg, ansmsg.Length, tempEndP);
            }
        }

        /// <summary>
        /// Calculate what user want
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private double Calculator(string msg)
        {
            var temp = msg.Split(new string[] { ":" },StringSplitOptions.RemoveEmptyEntries);

            double n1;
            double n2;

            if (temp.Length != 3)
                throw new Exception("Incorrect syntax!");

            if(!double.TryParse(temp[1],out n1) || !double.TryParse(temp[2], out n2))
                throw new Exception("Incorrect syntax!");

            switch (temp[0])
            {
                case "+":
                    return this.Add(n1, n2);

                case "-":
                    return this.Sub(n1, n2);

                case "*":
                    return this.Mult(n1, n2);

                case "/":
                    return this.Div(n1, n2);
                default:
                    throw new Exception("Incorrect syntax!");
            }
        }

        #region Interface region
        public double Add(double firstValue, double secondValue)
        {
            return firstValue + secondValue;
        }

        public double Div(double firstValue, double secondValue)
        {
            if (secondValue == 0)
                throw new Exception("You cannot do division by zero!");
            return firstValue / secondValue;
        }

        public double Mult(double firstValue, double secondValue)
        {
            return firstValue * secondValue;
        }

        public double Sub(double firstValue, double secondValue)
        {
            return firstValue - secondValue;
        }
        #endregion
    }
}
