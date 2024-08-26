using FluentMigrator;
using JetBrains.Annotations;

namespace ExchangeRates.Migrator.Migrations;

[Migration(201507201414), UsedImplicitly]
public class Migration201507201414 : ForwardOnlyMigration
{
    public override void Up()
    {
        Execute.Sql(@"DELETE FROM exchange_rate");

        Delete
                .Column("usd")
                .Column("eur")
                .Column("cny")
                .FromTable("exchange_rate");

        Create.Column("hash").OnTable("exchange_rate").AsString().NotNullable();
        Create.Column("currency_code").OnTable("exchange_rate").AsInt32().NotNullable();
        Create.Column("nomination").OnTable("exchange_rate").AsInt32().NotNullable();
        Create.Column("rate").OnTable("exchange_rate").AsDouble().NotNullable();

        Execute.Sql("ALTER TABLE exchange_rate DROP CONSTRAINT \"IX_unique_exchange_rate_on_date\"");

        Execute.Sql("ALTER TABLE exchange_rate ADD CONSTRAINT \"IX_unique_exchange_rate_on_date\" UNIQUE(on_date, currency_code);");
    }
}