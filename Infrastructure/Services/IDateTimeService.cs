using System;

namespace Infrastructure.Services;

public interface IDateTimeService
{
    DateTime GetNonCommercialArchiveLastStoreDate();
    DateTime GetAdsCampaignContentLastStoreDate();
    DateTime GetMonitorTimelineHistoryLastStoreDate();
    DateTime GetActionLogLastStoreDate();
    DateTime GetMonitorCodeHistoryLastStoreDate();
    DateTime GetWeatherForecastLastStoreDate();
    DateTime GetActiveMonitorLastConnectionDate();
    DateTime GetPrintScreenCommandCheckDate();
    DateTime GetScreenCastCommandCheckLastTime();
    DateTime GetTestConnectionCommandCheckDate();
    DateTime GetPartnerFederalCampaignStartDateBeforeConfirmationNotification();
    DateTime GetFederalCampaignStartDateForWarningNotification();

    DateTime GetUtcNow();
    DateTimeOffset GetNow(int offset);
    DateTimeOffset GetNow(TimeSpan offset);

    DateTime GetAdsCampaignsArchivationTime(DateTime date);

    /// <summary>
    ///     Получить сегодняшнюю дату
    /// </summary>
    DateOnly GetToday();

    /// <summary>
    ///     Получить предыдущий рабочий день.
    /// </summary>
    DateOnly GetPreviousWorkDate(DateOnly date);

    /// <summary>
    ///     Проверяет что время верно. Не сильно остает в прошлое и не слишком в будущем
    /// </summary>
    bool IsTimeCorrect(DateTimeOffset dateTime);

    DateTimeOffset GetCorrectedTime(DateTimeOffset dateTime);

    DateTime GetCorrectedDateTime(DateTime dateTime);

    (DateTimeOffset time, bool isCorrect) CorrectTime(DateTimeOffset time);
}