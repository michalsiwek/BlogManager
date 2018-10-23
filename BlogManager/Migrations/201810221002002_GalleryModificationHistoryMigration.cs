namespace BlogManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GalleryModificationHistoryMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Galleries", "LastModification", c => c.DateTime());
            AddColumn("dbo.Galleries", "LastModifiedBy_Id", c => c.Int());
            CreateIndex("dbo.Galleries", "LastModifiedBy_Id");
            AddForeignKey("dbo.Galleries", "LastModifiedBy_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Galleries", "LastModifiedBy_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Galleries", new[] { "LastModifiedBy_Id" });
            DropColumn("dbo.Galleries", "LastModifiedBy_Id");
            DropColumn("dbo.Galleries", "LastModification");
        }
    }
}
