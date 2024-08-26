namespace ExchangeRates.Domain;

using AutoFixture;
using FluentAssertions;
using Interfaces;
using Moq;
using Xunit;

public class ExchangeRateServiceTests
{
    private readonly Fixture _fixture = CustomFixture.Create();
    private readonly CancellationToken _cancellationToken = new ();

    private readonly Mock<IExchangeRateRepository> _exchangeRateRepository;

    private readonly ExchangeRateService _exchangeRateService;

    public ExchangeRateServiceTests()
    {
        _exchangeRateRepository = new Mock<IExchangeRateRepository>();
        _exchangeRateService = new ExchangeRateService(
                _exchangeRateRepository.Object);
    }

    [Fact]
    public async Task ListByDate_Test()
    {
        // arrange
        var date = DateOnly.FromDateTime(DateTime.Today);

        var rates = _fixture.Create<ExchangeRate[]>();
        _exchangeRateRepository
                .Setup(x => x.ListByDates(new[] {date}, _cancellationToken))
                .ReturnsAsync(rates);

        // act
        var result = await _exchangeRateService.ListByDate(date, _cancellationToken);

        // assert
        result.Should().BeSameAs(rates);
    }

    [Fact]
    public async Task ListByDates_Test()
    {
        // arrange
        var dates = new[] {DateOnly.FromDateTime(DateTime.Today)};

        var rates = _fixture.Create<ExchangeRate[]>();
        _exchangeRateRepository
                .Setup(x => x.ListByDates(dates, _cancellationToken))
                .ReturnsAsync(rates);

        // act
        var result = await _exchangeRateService.ListByDates(dates, _cancellationToken);

        // assert
        result.Should().BeSameAs(rates);
    }

    [Fact]
    public async Task Update_Test()
    {
        // arrange
        var rates = _fixture.Create<ExchangeRate[]>();
        _exchangeRateRepository
                .Setup(x => x.Update(rates, _cancellationToken))
                .Returns(Task.CompletedTask);

        // act
        await _exchangeRateService.Update(rates, _cancellationToken);

        // assert
        _exchangeRateRepository.VerifyAll();
    }

}
