namespace Classfinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PdOffset : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Schools", "PeriodOffset", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Schools", "PeriodOffset");
        }
    }
}
