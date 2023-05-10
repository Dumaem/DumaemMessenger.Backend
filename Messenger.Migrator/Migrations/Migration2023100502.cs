namespace Messenger.Migrator.Migrations
{
    [Migration(2023100502)]
    public class Migration2023100502 : MessengerMigration
    {
        public override void Up()
        {
            Alter.Table("chat")
                .AddColumn("groupName")
                .AsString(50)
                .Nullable();
        }
    }
}
