namespace Classfinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSchoolToClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Classes", "ClassCode", c => c.String());
            AddColumn("dbo.Classes", "ClassSchool_Id", c => c.Int());
            AddForeignKey("dbo.Classes", "ClassSchool_Id", "dbo.Schools", "Id");
            CreateIndex("dbo.Classes", "ClassSchool_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Classes", new[] { "ClassSchool_Id" });
            DropForeignKey("dbo.Classes", "ClassSchool_Id", "dbo.Schools");
            DropColumn("dbo.Classes", "ClassSchool_Id");
            DropColumn("dbo.Classes", "ClassCode");
        }
    }
}
