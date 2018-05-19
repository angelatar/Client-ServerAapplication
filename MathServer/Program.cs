

namespace MathServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //MathServer server = new MathServer(ServiceType.TCP);

            MathServer server = new MathServer(ServiceType.UDP);

            server.Start();
             
        }
    }
}
