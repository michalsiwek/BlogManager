namespace BlogManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GalleryStatusAttMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Galleries", "IsVisible", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Galleries", "IsVisible");
        }
    }
}
