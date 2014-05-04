namespace Classfinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DidScheduleManytoOne : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserClasses",
                c => new
                    {
                        User_ID = c.Int(nullable: false),
                        Class_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_ID, t.Class_Id })
                .ForeignKey("dbo.Users", t => t.User_ID, cascadeDelete: true)
                .ForeignKey("dbo.Classes", t => t.Class_Id, cascadeDelete: true)
                .Index(t => t.User_ID)
                .Index(t => t.Class_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserClasses", new[] { "Class_Id" });
            DropIndex("dbo.UserClasses", new[] { "User_ID" });
            DropForeignKey("dbo.UserClasses", "Class_Id", "dbo.Classes");
            DropForeignKey("dbo.UserClasses", "User_ID", "dbo.Users");
            DropTable("dbo.UserClasses");
        }
    }
}
