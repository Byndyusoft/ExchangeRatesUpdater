using FluentMigrator;
using JetBrains.Annotations;

namespace ExchangeRates.Migrator.Migrations;

[Migration(201507161018), UsedImplicitly]
public class Migration201507161018 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("exchange_rate")
              .WithColumn("id").AsInt32().NotNullable().Identity().PrimaryKey()
              .WithColumn("usd").AsDouble().NotNullable()
              .WithColumn("eur").AsDouble().NotNullable()
              .WithColumn("cny").AsDouble().NotNullable()
              .WithColumn("on_date").AsDate().NotNullable()
              .WithColumn("created_at").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);
    }
}
