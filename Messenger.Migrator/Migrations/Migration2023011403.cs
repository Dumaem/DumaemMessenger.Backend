using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Migrator.Migrations
{
    [Migration(2023_01_14_03)]
    public class Migration2023011403 : Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            Alter.Table("user").AddColumn("password").AsFixedLengthString(32);
            Alter.Table("user").AddColumn("email").AsString(255);
        }
    }
}
