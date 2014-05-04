namespace Classfinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSchoolCode : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Schools",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SignupCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Users", "School_Id", c => c.Int());
            AddForeignKey("dbo.Users", "School_Id", "dbo.Schools", "Id");
            CreateIndex("dbo.Users", "School_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", new[] { "School_Id" });
            DropForeignKey("dbo.Users", "School_Id", "dbo.Schools");
            DropColumn("dbo.Users", "School_Id");
            DropTable("dbo.Schools");
        }
    }
}
