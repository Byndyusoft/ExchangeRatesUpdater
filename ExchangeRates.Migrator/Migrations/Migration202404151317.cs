using FluentMigrator;
using JetBrains.Annotations;

namespace ExchangeRates.Migrator.Migrations;

[Migration(2024_04_15_13_17), UsedImplicitly] // ReSharper disable once InconsistentNaming
public class Migration202404151317_ExchangeRate_Date : ForwardOnlyMigration
{
    public override void Up()
    {
        Alter.Column("on_date")
             .OnTable("exchange_rate").AsDate().NotNullable();
    }
}
