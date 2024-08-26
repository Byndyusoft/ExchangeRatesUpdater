using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ExchangeRates.JobsScheduler.Cbr.Client.Rest;

/// <summary>
///     HTTP-клиент для получения списка валют
/// </summary>
/// <see href="http://www.cbr.ru/development/sxml/"/>
public sealed class CbrExchangeRatesHttpClient : ICbrExchangeRatesClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CbrExchangeRatesHttpClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<CbrExchangeRate[]> ListDateRates(
            DateOnly date,
            CancellationToken cancellationToken)
    {
        var dateTime = date.ToDateTime(TimeOnly.MinValue);

        using var httpClient = _httpClientFactory.CreateClient();

        var uri = $"http://www.cbr.ru/scripts/XML_daily.asp?date_req={dateTime:dd/MM/yyyy}";

        var xml = await httpClient.GetStringAsync(uri, cancellationToken);

        return ParseXml(date, xml).ToArray();
    }

    internal static IEnumerable<CbrExchangeRate> ParseXml(
            DateOnly date,
            string xml)
    {
        var document = XDocument.Parse(xml);

        foreach (var valute in document.Root!.Elements("Valute"))
        {
            var numCode = valute.Element("NumCode")!.Value;
            var charCode = valute.Element("CharCode")!.Value;
            var nominal = int.Parse(valute.Element("Nominal")!.Value);
            var name = valute.Element("Name")!.Value;
            var value = decimal.Parse(valute.Element("Value")!.Value.Replace(",", "."), NumberStyles.Float, CultureInfo.InvariantCulture);

            yield return new CbrExchangeRate
            {
                Name = name,
                Value = value,
                CharCode = charCode,
                Nominal = nominal,
                NumCode = numCode,
                Date = date
            };
        }
    }
}
