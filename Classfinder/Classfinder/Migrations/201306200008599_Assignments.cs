namespace Classfinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Assignments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Assignments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        Name = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ClassAssignments",
                c => new
                    {
                        Class_Id = c.Int(nullable: false),
                        Assignment_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Class_Id, t.Assignment_Id })
                .ForeignKey("dbo.Classes", t => t.Class_Id, cascadeDelete: true)
                .ForeignKey("dbo.Assignments", t => t.Assignment_Id, cascadeDelete: true)
                .Index(t => t.Class_Id)
                .Index(t => t.Assignment_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ClassAssignments", new[] { "Assignment_Id" });
            DropIndex("dbo.ClassAssignments", new[] { "Class_Id" });
            DropForeignKey("dbo.ClassAssignments", "Assignment_Id", "dbo.Assignments");
            DropForeignKey("dbo.ClassAssignments", "Class_Id", "dbo.Classes");
            DropTable("dbo.ClassAssignments");
            DropTable("dbo.Assignments");
        }
    }
}
