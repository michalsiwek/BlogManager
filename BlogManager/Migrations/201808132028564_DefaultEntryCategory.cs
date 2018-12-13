namespace BlogManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DefaultContentCategory : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO dbo.ContentCategories (Name, Description, CreateDate) VALUES ('Default', 'Default choice for entry without specific topic', '01/01/2018 12:00:00')");
        }
        
        public override void Down()
        {
        }
    }
}
