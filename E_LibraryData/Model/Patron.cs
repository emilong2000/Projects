using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Library.E_LibraryData.Model
{
    public class Patron
    {
        public int patronId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public DateTime DateTime { get; set; }
        public string PhoneNo { get; set; }

        //public virtual LibraryCard LibraryCard { get; set; }
    }
}
