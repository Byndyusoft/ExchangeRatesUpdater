using FluentMigrator;
using JetBrains.Annotations;

namespace ExchangeRates.Migrator.Migrations;

[Migration(201508271415), UsedImplicitly]
public class Migration201508271415 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Column("nominal_rate").OnTable("exchange_rate").AsDouble().Nullable();

        Execute.Sql(@"UPDATE exchange_rate 
                          SET nominal_rate = rate / nomination");

        Alter.Column("nominal_rate").OnTable("exchange_rate").AsDouble().NotNullable();
    }
}