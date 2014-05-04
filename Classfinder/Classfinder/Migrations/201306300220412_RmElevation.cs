namespace Classfinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RmElevation : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "Elevation");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Elevation", c => c.String());
        }
    }
}
