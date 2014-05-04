namespace Classfinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSchedule : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Classes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Period = c.Int(nullable: false),
                        Name = c.String(),
                        Teacher_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Teacher_ID)
                .Index(t => t.Teacher_ID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Classes", new[] { "Teacher_ID" });
            DropForeignKey("dbo.Classes", "Teacher_ID", "dbo.Users");
            DropTable("dbo.Classes");
        }
    }
}
