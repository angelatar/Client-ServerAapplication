using System;
using System.Net;
using System.Net.Sockets;

namespace Client_ServerAapplication
{
    public class MathServer : IMathService
    {
        private TcpListener tcp = null;

        private UdpClient udp = null;
        
        public ServiceType Type { private set; get; }
        
        private IPAddress ip;

        private int port;

        private int sumPort;

        private int subPort;

        private int multPort;

        private int divPort;

        private IPEndPoint endPoint;

        public MathServer(ServiceType type)
        {
            this.Type = type;

            this.ip = IPAddress.Parse("192.168.1.4");

            this.port = 4500;

            this.sumPort = 4501;

            this.subPort = 4502;

            this.multPort = 4503;

            this.divPort = 4504;

            this.endPoint = new IPEndPoint(this.ip, this.port);

            if (this.Type == ServiceType.TCP)
            {
                this.tcp = new TcpListener(this.endPoint);
            }
            else
            {
                this.udp = new UdpClient(this.endPoint);
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
