using Messenger.Domain.Validation.Validators;

namespace Messenger.Migrator.Migrations;

[Migration(2023_01_15_01)]
public class Migration2023011501 : Migration
{
    public override void Up()
    {
        Alter.Column("username")
            .OnTable("user")
            .AsString(UserValidator.MaximumUsernameLength)
            .Nullable();
        
        Alter.Column("name")
            .OnTable("user")
            .AsString(UserValidator.MaximumNameLength)
            .NotNullable();
        
        Alter.Column("name")
            .OnTable("chat")
            .AsString(ChatValidator.MaximumNameLength)
            .NotNullable();  
        
        Alter.Column("name")
            .OnTable("content_type")
            .AsString(50)
            .NotNullable();
    }

    public override void Down()
    {
        throw new NotImplementedException();
    }
}