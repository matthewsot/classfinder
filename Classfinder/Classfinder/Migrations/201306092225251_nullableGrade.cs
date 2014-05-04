namespace Classfinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullableGrade : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Classes", "Grade", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Classes", "Grade", c => c.Int(nullable: false));
        }
    }
}
