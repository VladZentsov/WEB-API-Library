using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models
{
    public class BookModel
    {
        public int Id;
        public string Title;
        public int Year;
        public string Author;
        public ICollection<int> CardsIds;
    }
}
