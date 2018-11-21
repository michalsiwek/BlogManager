namespace BlogManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PropertiesStringLengthUpdateMigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Entries", "Title", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Entries", "Description", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Galleries", "Title", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Galleries", "Description", c => c.String(maxLength: 255));
            AlterColumn("dbo.Pictures", "Descripton", c => c.String(maxLength: 255));
            AlterColumn("dbo.Pictures", "Author", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Pictures", "Author", c => c.String());
            AlterColumn("dbo.Pictures", "Descripton", c => c.String());
            AlterColumn("dbo.Galleries", "Description", c => c.String());
            AlterColumn("dbo.Galleries", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.Entries", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.Entries", "Title", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
