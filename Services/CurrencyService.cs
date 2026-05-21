using System.Globalization;
using System.Xml.Linq;
using SmartAssetTrackingSystem.Services;

public class CurrencyService : ICurrencyService
{
    private Dictionary<string, decimal>? _cachedRates;
    private DateTime _lastFetchTime;

    public async Task<Dictionary<string, decimal>> GetRatesAsync()
    {
        // Cache rates for 12 hours to reduce load on ECB and improve performance
        if (_cachedRates != null && DateTime.Now - _lastFetchTime < TimeSpan.FromHours(12))
            return _cachedRates;

        _cachedRates = await LoadEcbRatesAsync();
        _lastFetchTime = DateTime.Now;

        return _cachedRates;
    }

    public async Task<decimal> ConvertAsync(decimal amount, string fromCurrency, string toCurrency)
    {
        var rates = await GetRatesAsync();

        if (!rates.ContainsKey(fromCurrency) || !rates.ContainsKey(toCurrency))
            throw new Exception("Unsupported currency");

        decimal amountInEur = amount / rates[fromCurrency];
        decimal result = amountInEur * rates[toCurrency];

        return Math.Round(result, 2);
    }

    private static async Task<Dictionary<string, decimal>> LoadEcbRatesAsync()
    {
        string url = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml";

        using HttpClient client = new HttpClient();
        string xml = await client.GetStringAsync(url);

        XDocument doc = XDocument.Parse(xml);
        XNamespace ns = "http://www.ecb.int/vocabulary/2002-08-01/eurofxref";

        var rates = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase)
        {
            ["EUR"] = 1m
        };

        var dailyCube = doc.Root?
            .Element(ns + "Cube")?
            .Element(ns + "Cube");

        if (dailyCube == null)
            throw new Exception("ECB XML structure changed");

        foreach (var cube in dailyCube.Elements(ns + "Cube"))
        {
            string? currency = (string?)cube.Attribute("currency");
            string? rateStr = (string?)cube.Attribute("rate");

            if (!string.IsNullOrEmpty(currency) &&
                decimal.TryParse(rateStr,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out decimal rate))
            {
                rates[currency] = rate;
            }
        }

        return rates;
    }
}