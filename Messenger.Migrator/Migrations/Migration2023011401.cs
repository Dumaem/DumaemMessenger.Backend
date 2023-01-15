using FluentMigrator;

namespace Messenger.Migrator.Migrations
{
    [Migration(2023_01_14_01)]
    public class Migration2023011401 : Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            Rename.Table("readed_message").To("read_message");
            Rename.Table("type_content").To("content_type");
        }
    }
}
