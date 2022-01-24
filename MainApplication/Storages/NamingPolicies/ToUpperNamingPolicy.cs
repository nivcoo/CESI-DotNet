using System.Text.Json;

namespace MainApplication.Storages.NamingPolicies;

public class ToUpperNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        return name.ToUpper();
    }
}