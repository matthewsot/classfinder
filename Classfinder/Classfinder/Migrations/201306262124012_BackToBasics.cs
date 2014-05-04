namespace Classfinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BackToBasics : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ClassAssignments", "Class_Id", "dbo.Classes");
            DropForeignKey("dbo.ClassAssignments", "Assignment_Id", "dbo.Assignments");
            DropIndex("dbo.ClassAssignments", new[] { "Class_Id" });
            DropIndex("dbo.ClassAssignments", new[] { "Assignment_Id" });
            DropColumn("dbo.Schools", "Locks");
            DropTable("dbo.Assignments");
            DropTable("dbo.ClassAssignments");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ClassAssignments",
                c => new
                    {
                        Class_Id = c.Int(nullable: false),
                        Assignment_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Class_Id, t.Assignment_Id });
            
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
            
            AddColumn("dbo.Schools", "Locks", c => c.String());
            CreateIndex("dbo.ClassAssignments", "Assignment_Id");
            CreateIndex("dbo.ClassAssignments", "Class_Id");
            AddForeignKey("dbo.ClassAssignments", "Assignment_Id", "dbo.Assignments", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ClassAssignments", "Class_Id", "dbo.Classes", "Id", cascadeDelete: true);
        }
    }
}
