namespace Faktura.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Firms",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        NIP = c.Int(nullable: false),
                        Mail = c.String(),
                        OwnerName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DateOfCreation = c.DateTime(nullable: false),
                        MyProperty = c.Int(nullable: false),
                        BruttoPrice = c.Double(nullable: false),
                        Firm_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Firms", t => t.Firm_ID)
                .Index(t => t.Firm_ID);
            
            CreateTable(
                "dbo.SoldProducts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Count = c.Int(nullable: false),
                        PriceOfThisProducts = c.Double(nullable: false),
                        Invoice_ID = c.Int(),
                        Product_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Invoices", t => t.Invoice_ID)
                .ForeignKey("dbo.Products", t => t.Product_ID)
                .Index(t => t.Invoice_ID)
                .Index(t => t.Product_ID);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Price = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SoldProducts", "Product_ID", "dbo.Products");
            DropForeignKey("dbo.SoldProducts", "Invoice_ID", "dbo.Invoices");
            DropForeignKey("dbo.Invoices", "Firm_ID", "dbo.Firms");
            DropIndex("dbo.SoldProducts", new[] { "Product_ID" });
            DropIndex("dbo.SoldProducts", new[] { "Invoice_ID" });
            DropIndex("dbo.Invoices", new[] { "Firm_ID" });
            DropTable("dbo.Products");
            DropTable("dbo.SoldProducts");
            DropTable("dbo.Invoices");
            DropTable("dbo.Firms");
        }
    }
}
