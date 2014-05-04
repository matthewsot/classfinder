namespace Classfinder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClassStudents : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "Class_Id", "dbo.Classes");
            DropIndex("dbo.Users", new[] { "Class_Id" });
            DropColumn("dbo.Users", "Class_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Class_Id", c => c.Int());
            CreateIndex("dbo.Users", "Class_Id");
            AddForeignKey("dbo.Users", "Class_Id", "dbo.Classes", "Id");
        }
    }
}
