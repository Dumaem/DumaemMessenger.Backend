﻿namespace Messenger.Migrator.Migrations
{
    [Migration(2023100503)]
    public class Migration2023100503 : MessengerMigration
    {
        public override void Up()
        {
            Alter
                .Table("message_content")
                .InSchema("public")
                .AlterColumn("content")
                .AsString(1000)
                .NotNullable();
        }
    }
}
