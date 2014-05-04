namespace Classfinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PartElevation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Elevation", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Elevation");
        }
    }
}
