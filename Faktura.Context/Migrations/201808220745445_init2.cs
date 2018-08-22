namespace Faktura.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Invoices", "MyProperty");
            DropColumn("dbo.SoldProducts", "PriceOfThisProducts");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SoldProducts", "PriceOfThisProducts", c => c.Double(nullable: false));
            AddColumn("dbo.Invoices", "MyProperty", c => c.Int(nullable: false));
        }
    }
}
