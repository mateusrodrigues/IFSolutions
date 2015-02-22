namespace IFSolutions.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DatesOnPetition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Petitions", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Petitions", "DateSolved", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Petitions", "DateSolved");
            DropColumn("dbo.Petitions", "DateCreated");
        }
    }
}
