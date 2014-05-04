namespace Classfinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Many : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Classes", "User_ID", "dbo.Users");
            DropForeignKey("dbo.Classes", "User_ID1", "dbo.Users");
            DropIndex("dbo.Classes", new[] { "User_ID" });
            DropIndex("dbo.Classes", new[] { "User_ID1" });
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
            
            CreateTable(
                "dbo.UserClass1",
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
            
            DropColumn("dbo.Classes", "User_ID");
            DropColumn("dbo.Classes", "User_ID1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Classes", "User_ID1", c => c.Int());
            AddColumn("dbo.Classes", "User_ID", c => c.Int());
            DropIndex("dbo.UserClass1", new[] { "Class_Id" });
            DropIndex("dbo.UserClass1", new[] { "User_ID" });
            DropIndex("dbo.UserClasses", new[] { "Class_Id" });
            DropIndex("dbo.UserClasses", new[] { "User_ID" });
            DropForeignKey("dbo.UserClass1", "Class_Id", "dbo.Classes");
            DropForeignKey("dbo.UserClass1", "User_ID", "dbo.Users");
            DropForeignKey("dbo.UserClasses", "Class_Id", "dbo.Classes");
            DropForeignKey("dbo.UserClasses", "User_ID", "dbo.Users");
            DropTable("dbo.UserClass1");
            DropTable("dbo.UserClasses");
            CreateIndex("dbo.Classes", "User_ID1");
            CreateIndex("dbo.Classes", "User_ID");
            AddForeignKey("dbo.Classes", "User_ID1", "dbo.Users", "ID");
            AddForeignKey("dbo.Classes", "User_ID", "dbo.Users", "ID");
        }
    }
}
