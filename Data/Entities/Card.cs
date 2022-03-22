using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
namespace Data.Entities
{
	public class Card : BaseEntity
	{
		[Key]
		public int Id { get; set; }
		public DateTime Created { get; set; }
		public int ReaderId { get; set; }
		public virtual ICollection<History> Books { get; set; }
		public virtual Reader Reader { get; set; }

	}
}
