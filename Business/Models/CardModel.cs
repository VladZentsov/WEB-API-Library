using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models
{
    public class CardModel
    {
        public int Id;
        public DateTime Created;
        public int ReaderId;
        public ICollection<int> BooksIds;
    }
}
