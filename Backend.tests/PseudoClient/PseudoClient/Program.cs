using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;

namespace PseudoClient
{
    public class Program
    {
        static async Task Main(string[] args)
        {
             
            // Include port of the gRPC server as an application argument
            var port = args.Length > 0 ? args[0] : "50051";

            var channel = new Channel("localhost:" + port, ChannelCredentials.Insecure);
            var client = new Authentication.AuthenticationClient(channel);

            var reply = await client.AuthenticateAsync(new User(){Username = "test", Password = "xxx", RememberMe = true});
            Console.WriteLine("Response: " + reply.Status);

            await channel.ShutdownAsync();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
