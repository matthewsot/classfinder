namespace Classfinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Classes", "AbsPeriod", c => c.Int(nullable: false));
            AddColumn("dbo.Classes", "Limbo", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Classes", "AbsPeriod");
            DropColumn("dbo.Classes", "Limbo");
        }
    }
}
