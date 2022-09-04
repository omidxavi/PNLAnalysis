// See https://aka.ms/new-console-template for more information

using System.Threading.Channels;
using Microsoft.Extensions.Configuration;
using PNL_Analysis_Project;
using PNL_Analysis_Project.DataLayer;
using PNL_Analysis_Project.Services;
using PNL_Analysis_Project.Wallets;
using Serilog;


public class Program
{
    private const string configPath ="C:\\Users\\Omid\\RiderProjects\\PnlAnalysis Project\\Config.csv";

    public static Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile(
                $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
                true)
            .Build();

        ConfigLogging(configuration);
        var test = new DailyBalanceManager().GetNobitexToken();
        Console.WriteLine($"{test}");
        Console.ReadKey();

        while (true)
        {
            if (IsInTime())
            {
                Log.Information("Starting..."); 
                var daily = new DailyBalanceManager();
                daily.DailyBalanceMakerNobitex();
                daily.DailyBalanceMakerJibimo();
                daily.DailyBalanceMarketWalletDto();
                daily.TotalRial();
                daily.PnlCalculator();
                return Task.CompletedTask;
                
            }
            else
            {
                Thread.Sleep(TimeSpan.FromMinutes(1));
            }
        }
    }

    public static bool IsInTime()
    {
        var now = DateTime.Now.TimeOfDay;
        now = new TimeSpan(now.Hours, now.Minutes, 0);
        var lines = File.ReadAllLines(configPath);
        var setTime = lines[0].Split(",")[1];
        TimeSpan time = TimeSpan.Parse(setTime);
        if (now == time)
        {
            return true;
        }

        return false;
    }

    
    private static void ConfigLogging(IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
    }
}