namespace ExchangeRates.Domain;

/// <summary>
///     Курс обмена валют
/// </summary>
public class ExchangeRate
{
    public int Id { get; set; }

    /// <summary>
    ///     Дата курса
    /// </summary>
    public DateOnly OnDate { get; set; }

    /// <summary>
    ///     Дата создания
    /// </summary>
    public DateTime CreatedAt { get; set; }

    public string Hash { get; set; } = default!;

    /// <summary>
    ///     ISO Символьный код валюты
    /// </summary>
    public CurrencyCode CurrencyCode { get; set; }

    /// <summary>
    ///     Номинал
    /// </summary>
    public int Nomination { get; set; }

    /// <summary>
    ///     Курс
    /// </summary>
    public decimal Rate { get; set; }

    /// <summary>
    ///     Номинальный курс
    /// </summary>
    public decimal NominalRate => Rate / Nomination;
}
