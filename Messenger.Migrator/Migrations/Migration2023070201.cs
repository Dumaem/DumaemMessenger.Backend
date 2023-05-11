namespace Messenger.Migrator.Migrations
{
    [Migration(2023070201)]
    public class Migration2023070201 : MessengerMigration
    {
        public override void Up()
        {
            Alter.Table("user")
                .AddColumn("is_verified")
                .AsBoolean()
                .WithDefaultValue(false);
            Create.Table("verification")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("token").AsString(250)
                .WithColumn("is_actual").AsBoolean().WithDefaultValue(true)  
                .WithColumn("expiry_date").AsCustom("timestamp with time zone")
                .WithColumn("user_id").AsInt32().ForeignKey("user", "id");
            Create.UniqueConstraint("uniqToken")
                .OnTable("verification")
                .Columns("token", "user_id");
        }
    }
}