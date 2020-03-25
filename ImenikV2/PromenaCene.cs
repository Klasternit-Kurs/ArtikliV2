using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImenikV2
{
	public class PromenaCene
	{
		public DateTime Vreme { get; set; } = DateTime.Now;
		public Dictionary<Artikal, int> NoveMarze { get; set; } = new Dictionary<Artikal, int>();
	}
}
