namespace IFSolutions.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateCorrection : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Petitions", "DateSolved", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Petitions", "DateSolved", c => c.DateTime(nullable: false));
        }
    }
}
