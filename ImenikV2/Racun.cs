using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImenikV2 
{
	class Racun : INotifyPropertyChanged
	{
		public DateTime VremeIzdavanja { get; set; }

		public decimal Total 
		{ 
			get
			{
				decimal t = 0;
				foreach(Artikal a in Artikli.Keys)
				{
					t += a.IzlaznaCena * Artikli[a];
				}
				return t;
			}
		}

		private Dictionary<Artikal, int> artikli = new Dictionary<Artikal, int>();
		public Dictionary<Artikal, int> Artikli 
		{ 
			get
			{
				return artikli;
			}
			set
			{
				artikli = value;
				PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Artikli"));
				PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Total"));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
