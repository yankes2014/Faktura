namespace Faktura.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Invoices", "Firm_ID", "dbo.Firms");
            DropForeignKey("dbo.SoldProducts", "Product_ID", "dbo.Products");
            DropForeignKey("dbo.SoldProducts", "Invoice_ID", "dbo.Invoices");
            DropIndex("dbo.Invoices", new[] { "Firm_ID" });
            DropIndex("dbo.SoldProducts", new[] { "Product_ID" });
            DropIndex("dbo.SoldProducts", new[] { "Invoice_ID" });
            RenameColumn(table: "dbo.Invoices", name: "Firm_ID", newName: "FirmID");
            RenameColumn(table: "dbo.SoldProducts", name: "Product_ID", newName: "ProductID");
            RenameColumn(table: "dbo.SoldProducts", name: "Invoice_ID", newName: "InvoiceID");
            AlterColumn("dbo.Invoices", "FirmID", c => c.Int(nullable: false));
            AlterColumn("dbo.SoldProducts", "ProductID", c => c.Int(nullable: false));
            AlterColumn("dbo.SoldProducts", "InvoiceID", c => c.Int(nullable: false));
            CreateIndex("dbo.Invoices", "FirmID");
            CreateIndex("dbo.SoldProducts", "InvoiceID");
            CreateIndex("dbo.SoldProducts", "ProductID");
            AddForeignKey("dbo.Invoices", "FirmID", "dbo.Firms", "ID", cascadeDelete: true);
            AddForeignKey("dbo.SoldProducts", "ProductID", "dbo.Products", "ID", cascadeDelete: true);
            AddForeignKey("dbo.SoldProducts", "InvoiceID", "dbo.Invoices", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SoldProducts", "InvoiceID", "dbo.Invoices");
            DropForeignKey("dbo.SoldProducts", "ProductID", "dbo.Products");
            DropForeignKey("dbo.Invoices", "FirmID", "dbo.Firms");
            DropIndex("dbo.SoldProducts", new[] { "ProductID" });
            DropIndex("dbo.SoldProducts", new[] { "InvoiceID" });
            DropIndex("dbo.Invoices", new[] { "FirmID" });
            AlterColumn("dbo.SoldProducts", "InvoiceID", c => c.Int());
            AlterColumn("dbo.SoldProducts", "ProductID", c => c.Int());
            AlterColumn("dbo.Invoices", "FirmID", c => c.Int());
            RenameColumn(table: "dbo.SoldProducts", name: "InvoiceID", newName: "Invoice_ID");
            RenameColumn(table: "dbo.SoldProducts", name: "ProductID", newName: "Product_ID");
            RenameColumn(table: "dbo.Invoices", name: "FirmID", newName: "Firm_ID");
            CreateIndex("dbo.SoldProducts", "Invoice_ID");
            CreateIndex("dbo.SoldProducts", "Product_ID");
            CreateIndex("dbo.Invoices", "Firm_ID");
            AddForeignKey("dbo.SoldProducts", "Invoice_ID", "dbo.Invoices", "ID");
            AddForeignKey("dbo.SoldProducts", "Product_ID", "dbo.Products", "ID");
            AddForeignKey("dbo.Invoices", "Firm_ID", "dbo.Firms", "ID");
        }
    }
}
