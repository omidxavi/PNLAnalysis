using Newtonsoft.Json;

namespace PNL_Analysis_Project.Wallets;

public class NobitexToken
{
    [JsonProperty("token")]
    public string Token { get; set; }

    [JsonProperty("password")]
    public string Password { get; set; }

    [JsonProperty("username")]
    public string Username { get; set; }
}
