using backend.TokenServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using System;

namespace Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var crc = new Crc() { Message = new byte[] { 101, 111, 51, 14, 81, 12, 128 }, Divisor = new byte[] { 11, 168, 8, 101 } };
            var result = crc.Encode();
            var isvalid = crc.Valid(result, new byte[] { 11, 168, 8, 101 });
            Console.WriteLine(isvalid);
            //CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    .ConfigureKestrel(options =>
                    {
                        options.Limits.MinRequestBodyDataRate = null;
                        options.ListenLocalhost(50051, listenOptions =>
                        {
                            listenOptions.Protocols = HttpProtocols.Http2;
                        });
                    });
                });
    }
}
