using Byndyusoft.Data.Relational;

namespace ExchangeRates.DataAccess;

public abstract class Repository : DbSessionConsumer
{
    internal const string SessionKey = "1mt.ExchangeRates";

    protected Repository(IDbSessionAccessor sessionAccessor) 
            : base(sessionAccessor)
    {
    }

    protected IDbSession Session => DbSessions[SessionKey];
}
