using FluentMigrator;
using JetBrains.Annotations;

namespace ExchangeRates.Migrator.Migrations;

[Migration(201507161122), UsedImplicitly]
public class Migration201507161122 : ForwardOnlyMigration
{
    public override void Up()
    {
        Execute.Sql("ALTER TABLE exchange_rate ADD CONSTRAINT \"IX_unique_exchange_rate_on_date\" UNIQUE(on_date);");
    }
}