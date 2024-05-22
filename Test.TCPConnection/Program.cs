using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SalutemCRM.TCP;

namespace Test.TCPConnection;

internal class Program
{
    public static IHost? Host { get; private set; }

    static void Main(string[] args)
    {
        Console.WriteLine("TCP server connection");

        Host =
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
            .ConfigureServices(services => {
                services.AddSingleton<TCPChannel>();
            })
            .Build();

        try
        {
            Host!.Services.GetService<TCPChannel>()!.Open();
            Console.WriteLine($"Connected to {Host!.Services.GetService<TCPChannel>()!.thisServer.IpAddress}:{Host!.Services.GetService<TCPChannel>()!.thisServer.Port}!");
        }
        catch
        {
            Console.WriteLine($"Connection troubles...");
        }

        Console.WriteLine("Press any key.");
        Console.ReadKey();
    }
}
