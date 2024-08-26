namespace ExchangeRates.UpdaterWorker.Cbr.Client;

using System.Text;
using ExchangeRates.Cbr.Client;
using ExchangeRates.Cbr.Client.Rest;
using FluentAssertions;
using Xunit;

public class CbrExchangeRatesHttpClientTests
{
    static CbrExchangeRatesHttpClientTests()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    [Fact]
    public void ParseXml_Test()
    {
        // arrange
        var date = DateOnly.FromDateTime(DateTime.Today);
        var xml = File.ReadAllText("Assets/XML_daily.xml", Encoding.GetEncoding(1251));

        // act
        var dtos = CbrExchangeRatesHttpClient.ParseXml(date, xml).ToArray();

        // assert
        dtos.Should().ContainSingle(x => x.CharCode == "USD")
                      .Which.Should().BeEquivalentTo(
                              new CbrExchangeRate
                              {
                                      Value = 76.6044M,
                                      Name = "Доллар США",
                                      Nominal = 1,
                                      CharCode = "USD",
                                      Date = date,
                                      NumCode = "840"
                              });
        dtos.Should().ContainSingle(x => x.CharCode == "EUR")
            .Which.Should().BeEquivalentTo(
                    new CbrExchangeRate
                    {
                            Value = 81.4635M,
                            Name = "Евро",
                            Nominal = 1,
                            CharCode = "EUR",
                            Date = date,
                            NumCode = "978"
                    });
        dtos.Should().ContainSingle(x => x.CharCode == "CNY")
            .Which.Should().BeEquivalentTo(
                    new CbrExchangeRate
                    {
                            Value = 11.1226M,
                            Name = "Китайский юань",
                            Nominal = 1,
                            CharCode = "CNY",
                            Date = date,
                            NumCode = "156"
                    });
    }
}
