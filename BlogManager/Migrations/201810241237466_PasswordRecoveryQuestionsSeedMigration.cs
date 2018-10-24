using System.Data.SqlClient;

namespace BlogManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PasswordRecoveryQuestionsSeedMigration : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO dbo.PasswordRecoveryQuestions (Question) VALUES ('What is your favorite animal?')");
            Sql(@"INSERT INTO dbo.PasswordRecoveryQuestions (Question) VALUES ('What was the brand of your first mobile phone?')");
            Sql(@"INSERT INTO dbo.PasswordRecoveryQuestions (Question) VALUES ('What is your dream car model?')");
        }
        
        public override void Down()
        {
        }
    }
}
