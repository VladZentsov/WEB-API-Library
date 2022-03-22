using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Entities
{
    public class ReaderProfile : BaseEntity
    {
        [Key]
        public int ReaderId { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public virtual Reader Reader { get; set; }
    }
}
