namespace PNL_Analysis_Project;

public class Balances
{
    public string asset { get; set; }
    public string free { get; set; }
    public string locked { get; set; }
    public string total { get; set; }

    public string Name = "nobitex";
}

public class Root
{
    public List<Balances> balances { get; set; }
    public string exchange { get; set; }
}
