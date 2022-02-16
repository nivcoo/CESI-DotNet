using System.Text.Json;

namespace MainApplication.Storages.NamingPolicies;

public class ToUpperNamingPolicy : JsonNamingPolicy
{
    /// <summary>
    ///     Convert string to upper, for parser
    /// </summary>
    /// <param name="name"></param>
    /// <returns>upper string</returns>
    public override string ConvertName(string name)
    {
        return name.ToUpper();
    }
}