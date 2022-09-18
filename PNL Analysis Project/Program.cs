// See https://aka.ms/new-console-template for more information

using System.Globalization;
using System.Threading.Channels;
using Microsoft.Extensions.Configuration;
using PNL_Analysis_Project;
using PNL_Analysis_Project.DataLayer;
using PNL_Analysis_Project.Services;
using PNL_Analysis_Project.Wallets;
using Serilog;


public class Program
{
    private static string configPath =Path.Combine(Directory.GetCurrentDirectory(), "Config.csv");

     static void Main()
    {
        AppDomain.CurrentDomain.UnhandledException += GlobalizationExceptionHandler;
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile(
                $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
                true)
            .Build();

        ConfigLogging(configuration);
        Log.Information("Starting...");
        while (true)
        {
            if (IsInTime())
            {
                Log.Information("Time to Project Works..."); 
                var daily = new DailyBalanceManager();
                daily.DailyBalanceMakerNobitex();
                daily.DailyBalanceMakerJibimo();
                daily.DailyBalanceMarketWalletDto();
                daily.TotalRial();
                daily.PnlCalculator();
                Log.Information("Data Added to Database Successfully");
                Log.Information($"Sleep for 1430 minute...");
                Thread.Sleep(TimeSpan.FromMinutes(1430));
                //return Task.CompletedTask;

            }
            else
            {
                Log.Information("not in time...");
                Thread.Sleep(TimeSpan.FromMinutes(1));
            }
        }
    }

    private static void GlobalizationExceptionHandler(object sender, UnhandledExceptionEventArgs e)
    {
        Log.Information($"An Exception has raised {e.ExceptionObject.ToString()}");
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