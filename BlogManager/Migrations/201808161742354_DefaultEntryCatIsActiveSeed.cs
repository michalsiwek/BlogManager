namespace BlogManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DefaultEntryCatIsActiveSeed : DbMigration
    {
        public override void Up()
        {
            Sql(@"UPDATE dbo.ContentCategories SET IsActive = 'true' WHERE Id = 1");
        }
        
        public override void Down()
        {
        }
    }
}
