namespace CMSCore.Service.Content
{
    using System.Net;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;

    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(x => { x.Listen(IPAddress.Parse("127.0.0.1"), 5001); })
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
    }
}