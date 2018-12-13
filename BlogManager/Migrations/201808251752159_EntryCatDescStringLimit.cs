namespace BlogManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EntryCatDescStringLimit : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ContentCategories", "Description", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ContentCategories", "Description", c => c.String());
        }
    }
}
