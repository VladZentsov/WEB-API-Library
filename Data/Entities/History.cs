using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Entities
{
    public class History : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public int CardId { get; set; }
        public int BookId { get; set; }
        public DateTime TakeDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public virtual Card Card { get; set; }
        public virtual Book Book { get; set; }
    }
}
