using System;
using Scrutor;

namespace Infrastructure.Services;

[ServiceDescriptor]
public class DateTimeService : IDateTimeService
{
    /// <summary>
    ///     Количество минут между повторными обработками сообщений
    /// </summary>
    public static readonly TimeSpan RabbitMqMessageRetryTimeout = TimeSpan.FromMinutes(10);

    /// <summary>
    ///     Количество дней хранения роликов по подписке
    /// </summary>
    private const int NonCommercialArchiveStoreDays = 32;

    /// <summary>
    ///     Количество дней хранения видео РК после завершения
    /// </summary>
    private const int AdsCampaignStoreDays = 7;

    /// <summary>
    ///     Количество дней хранения действий пользователей
    /// </summary>
    private const int ActionLogStoreDays = 62;

    /// <summary>
    ///     Количество дней хранения истории проигрывания видео монитора
    /// </summary>
    private const int MonitorTimelineHistoryStoreDays = 9;

    /// <summary>
    ///     Количество дней хранения истории кодов монитора
    /// </summary>
    private const int MonitorCodeHistoryStoreDays = 15;

    /// <summary>
    ///     Количество дней хранения прогноза погоды
    /// </summary>
    private const int WeatherForecastStoreDays = 7;

    /// <summary>
    ///     Количество дней когда монитор считается активным
    /// </summary>
    private const int ActiveMonitorLastConnectionDays = 30;

    /// <summary>
    ///     Количество дней когда завершенная кампания считается архивной
    /// </summary>
    private const int AdsCampaignsArchivationDays = 6;

    /// <summary>
    ///     Количество минут для проверки уже созданной команды скриншота
    /// </summary>
    private const int PrintScreenCommandCheckMinutes = 1;

    /// <summary>
    ///     Количество минут для проверки уже созданной команды записи экрана
    /// </summary>
    public const int PrintScreenCastCheckMinutes = 5;

    /// <summary>
    ///     Количество минут для проверки уже созданной команды проверки связи
    /// </summary>
    private const int TestConnectionCommandCheckMinutes = 5;

    /// <summary>
    ///     Количество минут, после которого нужно отправлять уведолмения менеджерам букинга о неподтвержденной федеральной РК для партнера
    /// </summary>
    private const int PartnerFederalCampaignMinutesBeforeConfirmationNotification = 120;

    /// <summary>
    ///     Количество дней от старта РК, за которое нужно предупреждать об отсутствии контента или подтверждения федеральной РК
    /// </summary>
    private const int FederalCampaignDaysForWarningNotification = 3;

    public DateTime GetNonCommercialArchiveLastStoreDate()
    {
        return GetUtcNow().AddDays(-NonCommercialArchiveStoreDays);
    }

    public DateTime GetAdsCampaignContentLastStoreDate()
    {
        return GetUtcNow().AddDays(-AdsCampaignStoreDays);
    }

    public DateTime GetMonitorTimelineHistoryLastStoreDate()
    {
        return GetUtcNow().AddDays(-MonitorTimelineHistoryStoreDays);
    }

    public DateTime GetMonitorCodeHistoryLastStoreDate()
    {
        return GetUtcNow().AddDays(-MonitorCodeHistoryStoreDays);
    }

    public DateTime GetWeatherForecastLastStoreDate()
    {
        return GetUtcNow().AddDays(-WeatherForecastStoreDays);
    }

    public DateTime GetActionLogLastStoreDate()
    {
        return GetUtcNow().AddDays(-ActionLogStoreDays);
    }

    public DateTime GetActiveMonitorLastConnectionDate()
    {
        return GetUtcNow().AddDays(-ActiveMonitorLastConnectionDays);
    }

    public DateTime GetPrintScreenCommandCheckDate()
    {
        return GetUtcNow().AddMinutes(-PrintScreenCommandCheckMinutes);
    }

    public DateTime GetScreenCastCommandCheckLastTime()
    {
        return GetUtcNow().AddMinutes(-PrintScreenCastCheckMinutes);
    }

    public DateTime GetTestConnectionCommandCheckDate()
    {
        return GetUtcNow().AddMinutes(-TestConnectionCommandCheckMinutes);
    }

    public DateTime GetPartnerFederalCampaignStartDateBeforeConfirmationNotification()
    {
        return GetUtcNow().AddMinutes(-PartnerFederalCampaignMinutesBeforeConfirmationNotification);
    }

    public DateTime GetFederalCampaignStartDateForWarningNotification()
    {
        return GetUtcNow().AddMinutes(FederalCampaignDaysForWarningNotification);
    }

    public virtual DateTime GetUtcNow()
    {
        return DateTime.UtcNow;
    }

    public DateTimeOffset GetNow(int offset)
    {
        return GetNow(TimeSpan.FromMinutes(offset));
    }

    public DateTimeOffset GetNow(TimeSpan offset)
    {
        return DateTimeOffset.UtcNow.ToOffset(offset);
    }

    /// <summary>
    ///     Проверяет что время верно. Не сильно остает в прошлое и не слишком в будущем
    /// </summary>
    public bool IsTimeCorrect(DateTimeOffset dateTime)
    {
        var utcNow = GetUtcNow();

        var isDateNotTooFarFromThePast = Math.Abs(dateTime.Year - utcNow.Year) < 2;
        var isDateNotTooFarFromFuture = (dateTime - utcNow).TotalMinutes < 60;

        return isDateNotTooFarFromThePast && isDateNotTooFarFromFuture;
    }

    public DateTimeOffset GetCorrectedTime(DateTimeOffset dateTime)
    {
        var isDateCorrect = IsTimeCorrect(dateTime);
        if (isDateCorrect)
            return dateTime;

        return DateTimeOffset.UtcNow.ToOffset(dateTime.Offset);
    }

    public DateTime GetCorrectedDateTime(DateTime dateTime) =>
        GetCorrectedTime(dateTime).UtcDateTime;

    public (DateTimeOffset time, bool isCorrect) CorrectTime(DateTimeOffset time)
    {
        var isDateCorrect = IsTimeCorrect(time);
        if (isDateCorrect)
            return (time, true);

        return (DateTimeOffset.UtcNow.ToOffset(time.Offset), false);
    }

    public DateTime GetAdsCampaignsArchivationTime(DateTime dateTime)
    {
        return dateTime.AddDays(-AdsCampaignsArchivationDays);
    }

    public DateOnly GetToday()
    {
        return DateOnly.FromDateTime(DateTime.Today);
    }

    public DateOnly GetPreviousWorkDate(DateOnly date)
    {
        date = date.AddDays(-1);
        while (true)
        {
            if (date.DayOfWeek != DayOfWeek.Saturday
                && date.DayOfWeek != DayOfWeek.Sunday)
                break;

            date = date.AddDays(-1);
        }

        return date;
    }
}