namespace Classfinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Buildup : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserClasses", "User_ID", "dbo.Users");
            DropForeignKey("dbo.UserClasses", "Class_Id", "dbo.Classes");
            DropIndex("dbo.UserClasses", new[] { "User_ID" });
            DropIndex("dbo.UserClasses", new[] { "Class_Id" });
            AddColumn("dbo.Users", "Class_Id", c => c.Int());
            AddColumn("dbo.Classes", "User_ID", c => c.Int());
            AddColumn("dbo.Classes", "User_ID1", c => c.Int());
            AddForeignKey("dbo.Users", "Class_Id", "dbo.Classes", "Id");
            AddForeignKey("dbo.Classes", "User_ID", "dbo.Users", "ID");
            AddForeignKey("dbo.Classes", "User_ID1", "dbo.Users", "ID");
            CreateIndex("dbo.Users", "Class_Id");
            CreateIndex("dbo.Classes", "User_ID");
            CreateIndex("dbo.Classes", "User_ID1");
            DropTable("dbo.UserClasses");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserClasses",
                c => new
                    {
                        User_ID = c.Int(nullable: false),
                        Class_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_ID, t.Class_Id });
            
            DropIndex("dbo.Classes", new[] { "User_ID1" });
            DropIndex("dbo.Classes", new[] { "User_ID" });
            DropIndex("dbo.Users", new[] { "Class_Id" });
            DropForeignKey("dbo.Classes", "User_ID1", "dbo.Users");
            DropForeignKey("dbo.Classes", "User_ID", "dbo.Users");
            DropForeignKey("dbo.Users", "Class_Id", "dbo.Classes");
            DropColumn("dbo.Classes", "User_ID1");
            DropColumn("dbo.Classes", "User_ID");
            DropColumn("dbo.Users", "Class_Id");
            CreateIndex("dbo.UserClasses", "Class_Id");
            CreateIndex("dbo.UserClasses", "User_ID");
            AddForeignKey("dbo.UserClasses", "Class_Id", "dbo.Classes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserClasses", "User_ID", "dbo.Users", "ID", cascadeDelete: true);
        }
    }
}
