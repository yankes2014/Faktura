namespace Faktura.Model
{
    public class Firm
    {
        public int ID { get; set; }
        /// <summary>
        /// Name of Firm
        /// </summary>
        public string Name { get; set; }
        public string Address { get; set; }
        public int NIP { get; set; }
        public string Mail { get; set; }
        /// <summary>
        /// Name of person who orders items
        /// </summary>
        public string OwnerName { get; set; }

    }
}
