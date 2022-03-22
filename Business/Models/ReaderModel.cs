using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models
{
    public class ReaderModel
    {
        public int Id;
        public string Name;
        public string Email;
        public string Phone;
        public string Address;
        public ICollection<int> CardsIds;
    }
}
