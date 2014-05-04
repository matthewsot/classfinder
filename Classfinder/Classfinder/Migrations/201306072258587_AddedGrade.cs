namespace Classfinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedGrade : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Grade", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Grade");
        }
    }
}
