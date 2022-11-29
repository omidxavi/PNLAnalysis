using System.Transactions;
using Newtonsoft.Json;
using Serilog;

namespace PNL_Analysis_Project;

public class JibimoAPIManager
{
     
     
     string Url = "http://jibimo.liquipto.io/wallets";
     
     public JibimoAsset GetAsset()
     {
          try
          {
               var jibimoAsset = new JibimoAsset();
               using (var client =new HttpClient())
               {
                    var serializer = client.GetStringAsync(Url).Result;
                    var deserializer = JsonConvert.DeserializeObject<JibimoAsset>(serializer);
                    jibimoAsset = deserializer;
               }

               Log.Information($"free:{jibimoAsset.free} locked:{jibimoAsset.locked} total:{jibimoAsset.total}");
               return jibimoAsset;

          }
          catch (Exception e)
          {
               Log.Information(e.Message);
               Thread.Sleep(1000);
               return GetAsset();
          }

     }
}