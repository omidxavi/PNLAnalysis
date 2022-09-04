using System.Transactions;
using Newtonsoft.Json;

namespace PNL_Analysis_Project;

public class JibimoAPIManager
{
     string Url = "http://jibimo.liquipto.io/wallets";

     public JibimoAsset GetAsset()
     {
          var jibimoAsset = new JibimoAsset();
          using (var client =new HttpClient())
          {
               var serializer = client.GetStringAsync(Url).Result;
               var deserializer = JsonConvert.DeserializeObject<JibimoAsset>(serializer);
               jibimoAsset = deserializer;
          }

          Console.WriteLine($"free:{jibimoAsset.free} locked:{jibimoAsset.locked} total:{jibimoAsset.total}");
          return jibimoAsset;
     }
}