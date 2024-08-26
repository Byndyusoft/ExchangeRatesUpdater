using FluentMigrator;
using JetBrains.Annotations;

namespace ExchangeRates.Migrator.Migrations;

[Migration(202212201631), UsedImplicitly]
public class Migration202212201631 : ForwardOnlyMigration
{
    public override void Up()
    {
        Alter.Column("currency_code")
             .OnTable("exchange_rate").AsString().NotNullable();
        Execute.Sql(@"
UPDATE
    exchange_rate
SET currency_code = 
    CASE currency_code
        WHEN '0' THEN 'USD'
        WHEN '1' THEN 'EUR'
        WHEN '2' THEN 'CNY'
        ELSE NULL
    END");
    }
}