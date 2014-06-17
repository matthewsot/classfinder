namespace Classfinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Schedules : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Schedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Term = c.Int(nullable: false),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Classes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Subject = c.String(),
                        Period = c.Int(nullable: false),
                        Teacher_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teachers", t => t.Teacher_Id)
                .Index(t => t.Teacher_Id);
            
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        School_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Schools", t => t.School_Id, cascadeDelete: true)
                .Index(t => t.School_Id);
            
            CreateTable(
                "dbo.Schools",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        MinGrade = c.Int(nullable: false),
                        MaxGrade = c.Int(nullable: false),
                        Terms = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ScheduleClasses",
                c => new
                    {
                        Schedule_Id = c.Int(nullable: false),
                        Class_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Schedule_Id, t.Class_Id })
                .ForeignKey("dbo.Schedules", t => t.Schedule_Id, cascadeDelete: true)
                .ForeignKey("dbo.Classes", t => t.Class_Id, cascadeDelete: true)
                .Index(t => t.Schedule_Id)
                .Index(t => t.Class_Id);
            
            AddColumn("dbo.AspNetUsers", "Grade", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "School_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.AspNetUsers", "School_Id");
            AddForeignKey("dbo.AspNetUsers", "School_Id", "dbo.Schools", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Schedules", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ScheduleClasses", "Class_Id", "dbo.Classes");
            DropForeignKey("dbo.ScheduleClasses", "Schedule_Id", "dbo.Schedules");
            DropForeignKey("dbo.Classes", "Teacher_Id", "dbo.Teachers");
            DropForeignKey("dbo.Teachers", "School_Id", "dbo.Schools");
            DropForeignKey("dbo.AspNetUsers", "School_Id", "dbo.Schools");
            DropIndex("dbo.ScheduleClasses", new[] { "Class_Id" });
            DropIndex("dbo.ScheduleClasses", new[] { "Schedule_Id" });
            DropIndex("dbo.Teachers", new[] { "School_Id" });
            DropIndex("dbo.Classes", new[] { "Teacher_Id" });
            DropIndex("dbo.Schedules", new[] { "User_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "School_Id" });
            DropColumn("dbo.AspNetUsers", "School_Id");
            DropColumn("dbo.AspNetUsers", "Grade");
            DropTable("dbo.ScheduleClasses");
            DropTable("dbo.Schools");
            DropTable("dbo.Teachers");
            DropTable("dbo.Classes");
            DropTable("dbo.Schedules");
        }
    }
}
