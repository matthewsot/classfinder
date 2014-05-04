namespace Classfinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RealFix : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Classes", "Limbo");
            DropColumn("dbo.Classes", "AbsPeriod");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Classes", "Limbo", c => c.Boolean(nullable: false));
            AddColumn("dbo.Classes", "AbsPeriod", c => c.Int(nullable: false));
        }
    }
}
