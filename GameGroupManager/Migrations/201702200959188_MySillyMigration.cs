namespace GameGroupManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MySillyMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GgmUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GgmUsers", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.GgmUsers", new[] { "ApplicationUserId" });
            DropTable("dbo.GgmUsers");
        }
    }
}
