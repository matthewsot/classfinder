namespace Classfinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GradYearAndSignupLevel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "GradYear", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "SignUpLevel", c => c.Int(nullable: false));
            DropColumn("dbo.AspNetUsers", "Grade");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Grade", c => c.Int(nullable: false));
            DropColumn("dbo.AspNetUsers", "SignUpLevel");
            DropColumn("dbo.AspNetUsers", "GradYear");
        }
    }
}
