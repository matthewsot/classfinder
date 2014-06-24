namespace Classfinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstAndSecondSemester : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ScheduleClasses", "Schedule_Id", "dbo.Schedules");
            DropForeignKey("dbo.ScheduleClasses", "Class_Id", "dbo.Classes");
            DropForeignKey("dbo.Schedules", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Schedules", new[] { "User_Id" });
            DropIndex("dbo.ScheduleClasses", new[] { "Schedule_Id" });
            DropIndex("dbo.ScheduleClasses", new[] { "Class_Id" });
            CreateTable(
                "dbo.UserAccountClasses",
                c => new
                    {
                        UserAccount_Id = c.String(nullable: false, maxLength: 128),
                        Class_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserAccount_Id, t.Class_Id })
                .ForeignKey("dbo.AspNetUsers", t => t.UserAccount_Id, cascadeDelete: true)
                .ForeignKey("dbo.Classes", t => t.Class_Id, cascadeDelete: true)
                .Index(t => t.UserAccount_Id)
                .Index(t => t.Class_Id);
            
            CreateTable(
                "dbo.UserAccountClass1",
                c => new
                    {
                        UserAccount_Id = c.String(nullable: false, maxLength: 128),
                        Class_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserAccount_Id, t.Class_Id })
                .ForeignKey("dbo.AspNetUsers", t => t.UserAccount_Id, cascadeDelete: true)
                .ForeignKey("dbo.Classes", t => t.Class_Id, cascadeDelete: true)
                .Index(t => t.UserAccount_Id)
                .Index(t => t.Class_Id);
            
            DropTable("dbo.Schedules");
            DropTable("dbo.ScheduleClasses");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ScheduleClasses",
                c => new
                    {
                        Schedule_Id = c.Int(nullable: false),
                        Class_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Schedule_Id, t.Class_Id });
            
            CreateTable(
                "dbo.Schedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Term = c.Int(nullable: false),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.UserAccountClass1", "Class_Id", "dbo.Classes");
            DropForeignKey("dbo.UserAccountClass1", "UserAccount_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserAccountClasses", "Class_Id", "dbo.Classes");
            DropForeignKey("dbo.UserAccountClasses", "UserAccount_Id", "dbo.AspNetUsers");
            DropIndex("dbo.UserAccountClass1", new[] { "Class_Id" });
            DropIndex("dbo.UserAccountClass1", new[] { "UserAccount_Id" });
            DropIndex("dbo.UserAccountClasses", new[] { "Class_Id" });
            DropIndex("dbo.UserAccountClasses", new[] { "UserAccount_Id" });
            DropTable("dbo.UserAccountClass1");
            DropTable("dbo.UserAccountClasses");
            CreateIndex("dbo.ScheduleClasses", "Class_Id");
            CreateIndex("dbo.ScheduleClasses", "Schedule_Id");
            CreateIndex("dbo.Schedules", "User_Id");
            AddForeignKey("dbo.Schedules", "User_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ScheduleClasses", "Class_Id", "dbo.Classes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ScheduleClasses", "Schedule_Id", "dbo.Schedules", "Id", cascadeDelete: true);
        }
    }
}
