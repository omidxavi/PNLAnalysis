using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using PNL_Analysis_Project.DataLayer;
using PNL_Analysis_Project.Wallets;

using Serilog;

namespace PNL_Analysis_Project.Services;

public class DailyBalanceManager
{
    public void DailyBalanceMakerNobitex()
    {
        var nobitex = new NobitexAPIManager().GetAsset();
        foreach (var item in nobitex)
        {
            var dailyBalance = new DailyBalance();
            dailyBalance.asset = item.asset;
            dailyBalance.amount = item.total;
            dailyBalance.src = item.Name;
            Log.Information("{SourceName}:{Amount}:{Asset}", dailyBalance.src, dailyBalance.amount, dailyBalance.asset);
            var db = new DailyBalanceAddToDb();
            db.SetToDatabase(dailyBalance);
        }
    }

    public void DailyBalanceMakerJibimo()
    {
        var jibimo = new JibimoAPIManager().GetAsset();
        var dailyBalance = new DailyBalance
        {
            asset = jibimo.asset,
            amount = jibimo.total,
            src = jibimo.Name
        };
        Log.Information("{SourceName}:{Amount}:{Asset}", dailyBalance.src, dailyBalance.amount, dailyBalance.asset);
        var db = new DailyBalanceAddToDb();
        db.SetToDatabase(dailyBalance);
    }

    public List<NobitexToken> GetNobitexToken()
    {

     string cs =
        "host=37.152.186.76:5532;Username=postgres;Password=sp2Kfc7ucFBtTlb5es4K;Database=Crypto.General";
     using var con = new NpgsqlConnection(cs);
     con.Open();
     using var cmd = new NpgsqlCommand();
     cmd.Connection = con;
     var result = con.QueryFirstOrDefault($"SELECT nobitex FROM account_auth_data where account_id=12");
     var deserializer = JsonConvert.DeserializeObject<NobitexToken>(result);
     return deserializer;
    }

private decimal GetTotalRialBalanceNobitex()
{
    var result = new NobitexWebClient().GetWallet("57737808b1166e828b783262d9d6c626ba90d9a0");
    decimal total = 0;
    //decimal totalSell = 0;
    if (result != null)
        foreach (var wallet in result.Wallets)
        {
            total += wallet.RialBalance;
            //totalSell += wallet.RialBalanceSell;
        }

    return total;
}

public DailyBalance DailyBalanceMarketWalletDto()
{
    var dailyBalance = new DailyBalance();
    dailyBalance.asset = "irr";
    dailyBalance.amount = GetTotalRialBalanceNobitex().ToString();
    dailyBalance.src = "nobitex_total-rial";
    Log.Information("{SourceName}:{Amount}:{Asset}", dailyBalance.src, dailyBalance.amount, dailyBalance.asset);
    var db = new DailyBalanceAddToDb();
    db.SetToDatabase(dailyBalance);
    return dailyBalance;
}

public DailyBalance TotalRial()
{
    var total1 = GetTotalRialBalanceNobitex();
    var jibimo = new JibimoAPIManager().GetAsset().total;
    var nobitex = total1;
    var total = (Convert.ToInt64(jibimo) + Convert.ToInt64(nobitex)).ToString();
    var dailyBalance = new DailyBalance();
    dailyBalance.asset = "total";
    dailyBalance.amount = total;
    dailyBalance.src = "jibimo-nobitex-total-rial";
    Log.Information("{SourceName}:{Amount}:{Asset}", dailyBalance.src, dailyBalance.amount, dailyBalance.asset);
    var db = new DailyBalanceAddToDb();
    db.SetToDatabase(dailyBalance);
    return dailyBalance;
}

public DailyBalance PnlCalculator()
{
    //var get = new DailyBalanceAddToDb().GetFromDatabase();
    var getAllTotalsFromDatabase = new DailyBalanceAddToDb().GetAllTotalsFromDatabase();
    var pnl = new Pnl();
    var dailyBalance = new DailyBalance();
    getAllTotalsFromDatabase.Reverse();

    if (getAllTotalsFromDatabase.Count > 1)
    {
        dailyBalance.asset = pnl.asset;
        dailyBalance.src = pnl.src;
        dailyBalance.amount =
            ((Convert.ToDecimal(getAllTotalsFromDatabase[0].amount) -
              Convert.ToDecimal(getAllTotalsFromDatabase[1].amount)) /
             Convert.ToDecimal(getAllTotalsFromDatabase[1].amount)).ToString();
    }
    else
    {
        dailyBalance.asset = pnl.asset;
        dailyBalance.src = pnl.src;
        dailyBalance.amount = "0";
    }


    var db = new DailyBalanceAddToDb();
    db.SetToDatabase(dailyBalance);
    Log.Information("{SourceName}:{Amount}:{Asset}", dailyBalance.src, dailyBalance.amount, dailyBalance.asset);
    return dailyBalance;
}

}