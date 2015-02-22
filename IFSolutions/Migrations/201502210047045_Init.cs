namespace IFSolutions.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Campus",
                c => new
                    {
                        CampusID = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CampusID);
            
            CreateTable(
                "dbo.Petitions",
                c => new
                    {
                        PetitionID = c.Int(nullable: false, identity: true),
                        CategoryID = c.Int(nullable: false),
                        CampusID = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        Title = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Solved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PetitionID)
                .ForeignKey("dbo.Campus", t => t.CampusID, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.CategoryID)
                .Index(t => t.CampusID)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CategoryID);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentID = c.Int(nullable: false, identity: true),
                        PetitionID = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        Content = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CommentID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Petitions", t => t.PetitionID, cascadeDelete: false)
                .Index(t => t.PetitionID)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.CommentComplaints",
                c => new
                    {
                        CommentComplaintID = c.Int(nullable: false, identity: true),
                        CommentID = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.CommentComplaintID)
                .ForeignKey("dbo.Comments", t => t.CommentID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: false)
                .Index(t => t.CommentID)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        CampusID = c.Int(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Campus", t => t.CampusID, cascadeDelete: false)
                .Index(t => t.CampusID)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.PetitionComplaints",
                c => new
                    {
                        PetitionComplaintID = c.Int(nullable: false, identity: true),
                        PetitionID = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.PetitionComplaintID)
                .ForeignKey("dbo.Petitions", t => t.PetitionID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: false)
                .Index(t => t.PetitionID)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.PetitionComplaints", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.PetitionComplaints", "PetitionID", "dbo.Petitions");
            DropForeignKey("dbo.Comments", "PetitionID", "dbo.Petitions");
            DropForeignKey("dbo.CommentComplaints", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Petitions", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "CampusID", "dbo.Campus");
            DropForeignKey("dbo.CommentComplaints", "CommentID", "dbo.Comments");
            DropForeignKey("dbo.Petitions", "CategoryID", "dbo.Categories");
            DropForeignKey("dbo.Petitions", "CampusID", "dbo.Campus");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.PetitionComplaints", new[] { "UserId" });
            DropIndex("dbo.PetitionComplaints", new[] { "PetitionID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "CampusID" });
            DropIndex("dbo.CommentComplaints", new[] { "UserId" });
            DropIndex("dbo.CommentComplaints", new[] { "CommentID" });
            DropIndex("dbo.Comments", new[] { "UserId" });
            DropIndex("dbo.Comments", new[] { "PetitionID" });
            DropIndex("dbo.Petitions", new[] { "UserId" });
            DropIndex("dbo.Petitions", new[] { "CampusID" });
            DropIndex("dbo.Petitions", new[] { "CategoryID" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.PetitionComplaints");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.CommentComplaints");
            DropTable("dbo.Comments");
            DropTable("dbo.Categories");
            DropTable("dbo.Petitions");
            DropTable("dbo.Campus");
        }
    }
}
