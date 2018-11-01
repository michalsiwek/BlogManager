namespace BlogManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PasswordRecoveryQuestionsMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PasswordRecoveryQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Question = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "PasswordRecoveryAnswer", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "PasswordRecoveryQuestion_Id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "PasswordRecoveryQuestion_Id");
            AddForeignKey("dbo.AspNetUsers", "PasswordRecoveryQuestion_Id", "dbo.PasswordRecoveryQuestions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "PasswordRecoveryQuestion_Id", "dbo.PasswordRecoveryQuestions");
            DropIndex("dbo.AspNetUsers", new[] { "PasswordRecoveryQuestion_Id" });
            DropColumn("dbo.AspNetUsers", "PasswordRecoveryQuestion_Id");
            DropColumn("dbo.AspNetUsers", "PasswordRecoveryAnswer");
            DropTable("dbo.PasswordRecoveryQuestions");
        }
    }
}
