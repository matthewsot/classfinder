namespace Classfinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedTeacher : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Classes", "Teacher_Id", "dbo.Teachers");
            DropIndex("dbo.Classes", new[] { "Teacher_Id" });
            AddColumn("dbo.Classes", "Name", c => c.String());
            DropColumn("dbo.Classes", "Subject");
            DropColumn("dbo.Classes", "Teacher_Id");
            DropTable("dbo.Teachers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Classes", "Teacher_Id", c => c.Int());
            AddColumn("dbo.Classes", "Subject", c => c.String());
            DropColumn("dbo.Classes", "Name");
            CreateIndex("dbo.Classes", "Teacher_Id");
            AddForeignKey("dbo.Classes", "Teacher_Id", "dbo.Teachers", "Id");
        }
    }
}
