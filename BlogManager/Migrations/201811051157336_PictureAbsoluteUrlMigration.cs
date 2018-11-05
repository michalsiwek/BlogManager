namespace BlogManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PictureAbsoluteUrlMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pictures", "AbsoluteUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pictures", "AbsoluteUrl");
        }
    }
}
