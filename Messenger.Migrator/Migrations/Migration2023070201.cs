using FluentMigrator;

namespace Messenger.Migrator.Migrations
{
    [Migration(2023070201)]
    public class Migration2023070201 : Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            Alter.Column("is_verified")
                .OnTable("user")
                .AsBoolean()
                .WithDefaultValue(false);
            Create.Table("verification")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("token").AsString(250)
                .WithColumn("expiry_date").AsCustom("timestamp with time zone")
                .WithColumn("user_id").AsInt32().ForeignKey("user", "id");
            Create.UniqueConstraint("uniqToken")
                .OnTable("verification")
                .Columns("token", "user_id");
        }
    }
}