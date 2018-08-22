using System;

namespace Faktura.Model
{
    /// <summary>
    /// Faktura
    /// </summary>
    public class Invoice
    {
        public int ID { get; set; }
        public Firm Firm { get; set; }
        public int FirmID { get; set; }
        public DateTime DateOfCreation { get; set; }
        public double BruttoPrice { get; set; }

    }
}
