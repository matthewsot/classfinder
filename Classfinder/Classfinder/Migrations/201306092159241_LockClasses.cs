namespace Classfinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LockClasses : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Schools", "LockClasses", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Schools", "LockClasses");
        }
    }
}
