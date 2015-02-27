namespace IFSolutions.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContextUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Signatures", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Signatures", new[] { "User_Id" });
            DropColumn("dbo.Signatures", "UserId");
            RenameColumn(table: "dbo.Signatures", name: "User_Id", newName: "UserId");
            AlterColumn("dbo.Signatures", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Signatures", "UserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Signatures", "UserId");
            AddForeignKey("dbo.Signatures", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Signatures", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Signatures", new[] { "UserId" });
            AlterColumn("dbo.Signatures", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Signatures", "UserId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Signatures", name: "UserId", newName: "User_Id");
            AddColumn("dbo.Signatures", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Signatures", "User_Id");
            AddForeignKey("dbo.Signatures", "User_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
