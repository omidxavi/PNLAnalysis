using Newtonsoft.Json;

namespace PNL_Analysis_Project.Wallets;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

public class NobitexWalletDto
{
    [JsonProperty("status")] public string Status { get; set; }

    [JsonProperty("wallets")] public List<Wallet> Wallets { get; set; }
}

public class Wallet
{
    [JsonProperty("depositAddress")] public string DepositAddress { get; set; }

    [JsonProperty("depositTag")] public int? DepositTag { get; set; }

    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("currency")] public string Currency { get; set; }

    [JsonProperty("balance")] public decimal Balance { get; set; }

    [JsonProperty("blockedBalance")] public decimal BlockedBalance { get; set; }

    [JsonProperty("activeBalance")] public decimal ActiveBalance { get; set; }

    [JsonProperty("rialBalance")] public decimal RialBalance { get; set; }

    [JsonProperty("rialBalanceSell")] public decimal RialBalanceSell { get; set; }

    public string asset = "irr";

    public string name = "nobitex";
}