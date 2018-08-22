using Faktura.Model;
using System.Data.Entity;

namespace Faktura.Context
{
    public class FakturaContext : DbContext
    {
        public DbSet<Firm> Firms { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SoldProduct> SoldProducts { get; set; }

        public FakturaContext() : base("Faktura")
        {

        }
    }
}
