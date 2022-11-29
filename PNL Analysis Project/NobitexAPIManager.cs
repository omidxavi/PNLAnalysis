using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Net.Http;
using PNL_Analysis_Project.DataLayer;
using Serilog;

namespace PNL_Analysis_Project;

public class NobitexAPIManager
{
    private HttpClient HttpClient;
    private const string ApiKey = "64455a87-c749-4781-9e54-9e5d1a692a18";
    private const string AccountUId = "b7991661-37c4-49d1-9a64-7300e3be210d";
    private const string Url = "http://nobitex.bigreport.ir/api/wallets";

    public List<Balances> GetAsset()
    {
        try
        {
            var balances = new List<Balances>();
            HttpClient = new HttpClient();
       
            using (var client=new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Api-Key",ApiKey);
                client.DefaultRequestHeaders.Add("Account-UId",AccountUId);
                var serializer = client.GetStringAsync(Url).Result;
                var deserializer = JsonConvert.DeserializeObject<Root>(serializer);
                for (int i = 0; i < deserializer.balances.Count; i++)
                {
                    balances.Add(deserializer.balances[i]);
                }
            }
            return balances ;


        }
        catch (Exception e)
        {
            Log.Information(e.Message);
            Thread.Sleep(1000);
            return GetAsset();
        }
    }
}