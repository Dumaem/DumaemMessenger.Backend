using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Migrator.Migrations
{
    [Migration(2023_01_14_04)]
    public class Migration2023011404 : Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            Create.ForeignKey()
                .FromTable("message").ForeignColumn("forwarded_message_id")
                .ToTable("message").PrimaryColumn("id");

            Create.ForeignKey()
                .FromTable("message").ForeignColumn("replied_message_id")
                .ToTable("message").PrimaryColumn("id");

            Create.ForeignKey()
                .FromTable("message").ForeignColumn("chat_id")
                .ToTable("chat").PrimaryColumn("id");

            Create.ForeignKey()
                .FromTable("message").ForeignColumn("sender_id")
                .ToTable("user").PrimaryColumn("id");
            
            Create.ForeignKey()
                .FromTable("message_content").ForeignColumn("message_id")
                .ToTable("message").PrimaryColumn("id");
            
            Create.ForeignKey()
                .FromTable("message_content").ForeignColumn("type_id")
                .ToTable("type").PrimaryColumn("id");
            
            Create.ForeignKey()
                .FromTable("user_chat").ForeignColumn("user_id")
                .ToTable("user").PrimaryColumn("id");
            
            Create.ForeignKey()
                .FromTable("deleted_message").ForeignColumn("user_id")
                .ToTable("user").PrimaryColumn("id");
            
            Create.ForeignKey()
                .FromTable("deleted_message").ForeignColumn("message_id")
                .ToTable("message").PrimaryColumn("id");
            
            Create.ForeignKey()
                .FromTable("readed_message").ForeignColumn("user_id")
                .ToTable("user").PrimaryColumn("id");
            
            Create.ForeignKey()
                .FromTable("readed_message").ForeignColumn("message_id")
                .ToTable("message").PrimaryColumn("id");
        }
    }
}
