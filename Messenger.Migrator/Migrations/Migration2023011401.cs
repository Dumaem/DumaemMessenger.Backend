using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Migrator.Migrations
{
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
