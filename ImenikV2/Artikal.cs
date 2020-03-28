using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImenikV2
{
	[Serializable]
	class Artikal : INotifyPropertyChanged
	{
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

		private int marza;
		public int Marza 
		{ 
			get
			{
				return marza;
			}
			set
			{
				marza = value;
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
				PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Kolicina"));
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
