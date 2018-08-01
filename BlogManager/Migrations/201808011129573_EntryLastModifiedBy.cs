namespace BlogManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EntryLastModifiedBy : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Entries", "LastModifiedBy_Id", c => c.Int());
            CreateIndex("dbo.Entries", "LastModifiedBy_Id");
            AddForeignKey("dbo.Entries", "LastModifiedBy_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Entries", "LastModifiedBy_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Entries", new[] { "LastModifiedBy_Id" });
            DropColumn("dbo.Entries", "LastModifiedBy_Id");
        }
    }
}
