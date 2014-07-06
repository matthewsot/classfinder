namespace Classfinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClassGradYear : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Classes", "GradYear", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Classes", "GradYear");
        }
    }
}