namespace Classfinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Schools : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Classes", "School", c => c.String());
            AddColumn("dbo.AspNetUsers", "School", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "School");
            DropColumn("dbo.Classes", "School");
        }
    }
}
