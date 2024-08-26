using System;
using System.Runtime.Serialization;

namespace ExchangeRatesUpdaterWorker.Cbr.Client;

[DataContract]
public record CbrExchangeRate
{
    [DataMember] public DateOnly Date { get; init; }

    [DataMember] public string NumCode { get; init; } = default!;

    [DataMember] public string CharCode { get; init; } = default!;

    [DataMember] public int Nominal { get; init; }

    [DataMember] public string Name { get; init; } = default!;

    [DataMember] public decimal Value { get; init; }
}
