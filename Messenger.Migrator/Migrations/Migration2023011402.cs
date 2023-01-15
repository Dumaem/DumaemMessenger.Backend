using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace Messenger.Migrator.Migrations
{
    [Migration(2023_01_14_02)]
    public class Migration2023011402 : Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            Create.Table("refresh_token")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("token").AsString(250)
                .WithColumn("jwt_id").AsString(250)
                .WithColumn("is_used").AsBoolean()
                .WithColumn("is_revoked").AsBoolean()
                .WithColumn("creation_date").AsDateTime()
                .WithColumn("expiry_date").AsDateTime()
                .WithColumn("user_id").AsInt32().ForeignKey("user","id");              
        }
    }
}
