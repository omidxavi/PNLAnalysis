using System.Net.Http.Headers;
using Newtonsoft.Json;
using PNL_Analysis_Project.Wallets;
using Serilog;

namespace PNL_Analysis_Project.Services;

public class NobitexWebClient
{
    public NobitexWalletDto GetWallet(string key)
    {
        try
        {
            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.nobitex1.ir/users/wallets/list");
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Token", key);
            var response = client.SendAsync(request).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var result = JsonConvert.DeserializeObject<NobitexWalletDto>(content);
                return result;
            }

            return null;
        }

        catch (Exception e)
        {
            Log.Information(e.Message);
            Thread.Sleep(1000);
            return GetWallet(key);
        }
    }
}