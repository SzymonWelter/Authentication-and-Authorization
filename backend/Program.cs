using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;

namespace backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const int port = 50000;
            Server server = new Server{
                Services = {Test.BindService(new TestImpl())},
                Ports = {new ServerPort("localhost", port, ServerCredentials.Insecure)}
            };
            server.Start();
            Console.WriteLine("RouteGuide server listening on port " + port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }

    }
}
