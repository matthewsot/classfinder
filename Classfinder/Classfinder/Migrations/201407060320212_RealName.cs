namespace Classfinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RealName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "RealName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "RealName");
        }
    }
}
