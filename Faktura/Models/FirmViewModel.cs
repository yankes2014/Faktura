using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Faktura.Models
{
    //Niewykorzystywany :)
    public class FirmViewModel
    {
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