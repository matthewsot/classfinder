namespace Classfinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GradedClasses : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Classes", "Grade", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Classes", "Grade");
        }
    }
}
