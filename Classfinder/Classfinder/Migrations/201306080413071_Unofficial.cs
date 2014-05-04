namespace Classfinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Unofficial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Classes", "UnofficialTeacher", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Classes", "UnofficialTeacher");
        }
    }
}
