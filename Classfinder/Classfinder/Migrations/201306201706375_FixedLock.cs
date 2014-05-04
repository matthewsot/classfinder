namespace Classfinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedLock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Schools", "Locks", c => c.String(nullable: false, defaultValue: ""));
            DropColumn("dbo.Schools", "LockClasses");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Schools", "LockClasses", c => c.Boolean(nullable: false));
            DropColumn("dbo.Schools", "Locks");
        }
    }
}
