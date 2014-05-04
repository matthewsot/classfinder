namespace Classfinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentsinClass : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Classes", "Grade");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Classes", "Grade", c => c.Int());
        }
    }
}
