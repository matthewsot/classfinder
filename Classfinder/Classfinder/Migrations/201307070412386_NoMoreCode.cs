namespace Classfinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NoMoreCode : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Classes", "ClassCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Classes", "ClassCode", c => c.String());
        }
    }
}
