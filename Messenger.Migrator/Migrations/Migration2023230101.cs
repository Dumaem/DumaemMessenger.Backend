using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Migrator.Migrations
{
    [Migration(2023230101)]
    public class Migration2023230101 : Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            Alter.Column("is_edited")
                .OnTable("message")
                .AsBoolean()
                .WithDefaultValue(false);

            Alter.Column("is_deleted")
                .OnTable("message")
                .AsBoolean()
                .WithDefaultValue(false);
        }
    }
}
