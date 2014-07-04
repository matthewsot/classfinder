namespace Classfinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedSchools : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "School_Id", "dbo.Schools");
            DropForeignKey("dbo.Teachers", "School_Id", "dbo.Schools");
            DropIndex("dbo.AspNetUsers", new[] { "School_Id" });
            DropIndex("dbo.Teachers", new[] { "School_Id" });
            DropColumn("dbo.AspNetUsers", "School_Id");
            DropColumn("dbo.Teachers", "School_Id");
            DropTable("dbo.Schools");
        }
        
        public override void Down()
        {
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
            
            AddColumn("dbo.Teachers", "School_Id", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "School_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Teachers", "School_Id");
            CreateIndex("dbo.AspNetUsers", "School_Id");
            AddForeignKey("dbo.Teachers", "School_Id", "dbo.Schools", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUsers", "School_Id", "dbo.Schools", "Id", cascadeDelete: true);
        }
    }
}
