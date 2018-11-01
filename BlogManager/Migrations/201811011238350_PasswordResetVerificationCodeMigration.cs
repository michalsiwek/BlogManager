namespace BlogManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PasswordResetVerificationCodeMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PasswordResetVerificationCodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        ExpirationDate = c.DateTime(nullable: false),
                        Account_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Account_Id)
                .Index(t => t.Account_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PasswordResetVerificationCodes", "Account_Id", "dbo.AspNetUsers");
            DropIndex("dbo.PasswordResetVerificationCodes", new[] { "Account_Id" });
            DropTable("dbo.PasswordResetVerificationCodes");
        }
    }
}
