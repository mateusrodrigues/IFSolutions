namespace IFSolutions.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Signatures : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Signatures",
                c => new
                    {
                        SignatureID = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        PetitionID = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.SignatureID)
                .ForeignKey("dbo.Petitions", t => t.PetitionID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.PetitionID)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Signatures", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Signatures", "PetitionID", "dbo.Petitions");
            DropIndex("dbo.Signatures", new[] { "User_Id" });
            DropIndex("dbo.Signatures", new[] { "PetitionID" });
            DropTable("dbo.Signatures");
        }
    }
}
