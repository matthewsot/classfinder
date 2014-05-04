namespace Classfinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NoOfficial : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Classes", "Teacher_ID", "dbo.Users");
            DropIndex("dbo.Classes", new[] { "Teacher_ID" });
            AddColumn("dbo.Classes", "Teacher", c => c.String());
            DropColumn("dbo.Classes", "UnofficialTeacher");
            DropColumn("dbo.Classes", "Teacher_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Classes", "Teacher_ID", c => c.Int());
            AddColumn("dbo.Classes", "UnofficialTeacher", c => c.String());
            DropColumn("dbo.Classes", "Teacher");
            CreateIndex("dbo.Classes", "Teacher_ID");
            AddForeignKey("dbo.Classes", "Teacher_ID", "dbo.Users", "ID");
        }
    }
}
