
using System.Net;

namespace MathClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //MathClient client = new MathClient(ServiceType.TCP, IPAddress.Parse("192.168.1.4"), 4500);

            MathClient client = new MathClient(ServiceType.UDP, IPAddress.Parse("192.168.1.4"), 4500);

            client.Connect();
            client.Ask();
        }
    }
}
