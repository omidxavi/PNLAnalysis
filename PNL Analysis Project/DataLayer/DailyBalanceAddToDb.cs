using Dapper;
using Npgsql;
using System.Globalization;

namespace PNL_Analysis_Project.DataLayer;

public class DailyBalanceAddToDb
{
    private string cs = "host=37.152.186.76:5532;Username=postgres;Password=sp2Kfc7ucFBtTlb5es4K;Database=Crypto.PnlAnalysis";

    public void SetToDatabase(DailyBalance dailyBalance)
    {
        using var con = new NpgsqlConnection(cs);
        con.Open();
        using var cmd = new NpgsqlCommand();
        cmd.Connection = con; 
        cmd.CommandText = $"INSERT INTO daily_balance (asset,amount,src) VALUES ('{dailyBalance.asset}',{dailyBalance.amount},'{dailyBalance.src}') ";
        cmd.ExecuteNonQuery();
        con.Close();
    }

    public List<DailyBalance> GetFromDatabase()
    {
        
            using var con = new NpgsqlConnection(cs);
            con.Open();
            using var cmd = new NpgsqlCommand();
            cmd.Connection = con;
            var result = con.Query<DailyBalance>($"SELECT id,analysis_date,asset,amount,src FROM daily_balance")
                .ToList();
            return result;
    }

    public List<DailyBalance> GetAllTotalsFromDatabase()
    {
        
            using var con = new NpgsqlConnection(cs);
            con.Open();
            var yesterday = DateTime.Now.AddDays(-1).ToString("yy-MM-dd");
            using var cmd = new NpgsqlCommand();
            cmd.Connection = con;
            var result = con.Query<DailyBalance>($"SELECT id,analysis_date,asset,amount,src FROM daily_balance where asset='total'")
                
                .ToList();
            return result;
        

    }
}
