namespace Classfinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedGradYear : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Classes", "GradYear");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Classes", "GradYear", c => c.Int(nullable: false));
        }
    }
}
