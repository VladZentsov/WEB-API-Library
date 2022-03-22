using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Data.Entities
{
	public class Reader : BaseEntity
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public virtual ICollection<Card> Cards { get; set; }
		public virtual ReaderProfile ReaderProfile { get; set; }
	}
}
