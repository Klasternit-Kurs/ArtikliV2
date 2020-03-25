using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImenikV2
{
	[Serializable]
	public class Artikal : INotifyPropertyChanged
	{
		public static List<PromenaCene> Kalkulacije = new List<PromenaCene>();

		public string Sifra { get; set; }
		public string Naziv { get; set; }

		private decimal ulaznaCena;
		public decimal UlaznaCena 
		{ 
			get
			{
				return ulaznaCena;
			}
			set
			{
				ulaznaCena = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UlaznaCena"));
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IzlaznaCena"));
			}
		}

		public int Marza 
		{ 
			get
			{
				foreach(PromenaCene p in Artikal.Kalkulacije.Reverse<PromenaCene>())
				{
					if (DateTime.Now > p.Vreme && p.NoveMarze.ContainsKey(this))
					{
						return p.NoveMarze[this];
					}
				}
				return 0; //Pro forme
			}

			set
			{
				var Promena = new PromenaCene();
				Promena.NoveMarze.Add(this, value);
				Artikal.Kalkulacije.Add(Promena);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Marza"));
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IzlaznaCena"));
			}
		}

		[field: NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged;
		
		private int kolicina;
		public int Kolicina 
		{ 
			get
			{
				return kolicina;
			}
			set
			{
				if (value > 0)
				{
					kolicina = value;
				} else
				{
					kolicina = 0;
				}
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Kolicina"));
			}
		}

		public decimal IzlaznaCena
		{
			get
			{
				return UlaznaCena * ((decimal)Marza / 100 + 1);
			}
		}

		public override string ToString()
		{
			return $"{Sifra} - {Naziv}";
		}


	}
}
