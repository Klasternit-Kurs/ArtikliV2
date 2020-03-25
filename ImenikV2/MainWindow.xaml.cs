using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel;

namespace ImenikV2
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		ObservableCollection<Artikal> Artikli = new ObservableCollection<Artikal>();
		ObservableCollection<Racun> Racuni = new ObservableCollection<Racun>();

		public event PropertyChangedEventHandler PropertyChanged;

		private string sifraZaRacun;
		public string SifraZaRacun 
		{ 
			get
			{
				return sifraZaRacun;
			}
			set
			{
				sifraZaRacun = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SifraZaRacun"));
			}
		}

		private int kolicinaZaRacun;
		public int KolicinaZaRacun 
		{ 
			get
			{
				return kolicinaZaRacun;
			}
				
			set
			{
				kolicinaZaRacun = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("KolicinaZaRacun"));
			}
		}

		public MainWindow()
		{
			InitializeComponent();

			DataContext = new Racun();
			UniGrid.DataContext = this;
			dgTrenutniRacun.ItemsSource = (DataContext as Racun).Artikli;
			
			BinaryFormatter bf = new BinaryFormatter();
			if (File.Exists("art.dat"))
			{
				using (FileStream fs = new FileStream("art.dat", FileMode.Open, FileAccess.Read))
				{
					Artikli = bf.Deserialize(fs) as ObservableCollection<Artikal>;
				}
			}
			if (File.Exists("kalk.dat"))
			{
				using (FileStream fs = new FileStream("kalk.dat", FileMode.Open, FileAccess.Read))
				{
					Artikal.Kalkulacije = bf.Deserialize(fs) as List<PromenaCene>;
				}
			}

			if (File.Exists("rac.dat"))
			{
				using (FileStream fs = new FileStream("rac.dat", FileMode.Open, FileAccess.Read))
				{
					Racuni = bf.Deserialize(fs) as ObservableCollection<Racun>;
				}
			}

			dgRacuni.ItemsSource = Racuni;
			dg.ItemsSource = Artikli;
		}

		private void ZatvaramSe(object sender, System.ComponentModel.CancelEventArgs e)
		{
			BinaryFormatter bf = new BinaryFormatter();
			using (FileStream fs = new FileStream("art.dat", FileMode.Create, FileAccess.Write))
			{
				bf.Serialize(fs, Artikli);
			}
			using (FileStream fs = new FileStream("kalk.dat", FileMode.Create, FileAccess.Write))
			{
				bf.Serialize(fs, Artikal.Kalkulacije);
			}
			using (FileStream fs = new FileStream("rac.dat", FileMode.Create, FileAccess.Write))
			{
				bf.Serialize(fs, Racuni);
			}
		}

		private void UnosArtikla(object sender, RoutedEventArgs e)
		{
			foreach (Artikal a in Artikli)
			{
				if (a.Sifra == SifraZaRacun)
				{
					if (a.Kolicina >= KolicinaZaRacun)
					{
						var recnik = (DataContext as Racun).Artikli;
						if (KolicinaZaRacun == 0)
						{
							recnik.Remove(a);
						} else if (recnik.ContainsKey(a))
						{
							recnik[a] = KolicinaZaRacun;
						} else
						{
							recnik.Add(a, KolicinaZaRacun);
						}

						dgTrenutniRacun.ItemsSource = null;
						dgTrenutniRacun.ItemsSource = (DataContext as Racun).Artikli;

						var temp = (DataContext as Racun).Artikli;
						(DataContext as Racun).Artikli = null;
						(DataContext as Racun).Artikli = temp;

						SifraZaRacun = "";
						KolicinaZaRacun = 0;
					}
				}
			}
		}

		private void PromenaSelekcije(object sender, SelectionChangedEventArgs e)
		{
			var dg = sender as DataGrid;


			if (dg.SelectedItem != null)
			{
				SifraZaRacun = ((KeyValuePair<Artikal, int>)dg.SelectedItem).Key.Sifra;
				KolicinaZaRacun = ((KeyValuePair<Artikal, int>)dg.SelectedItem).Value;
			} else
			{
				SifraZaRacun = "";
				KolicinaZaRacun = 0;
			}
		}

		private void Izdaj(object sender, RoutedEventArgs e)
		{
			var rac = DataContext as Racun;
			if (rac.Artikli.Count > 0)
			{
				rac.VremeIzdavanja = DateTime.Now;
				Racuni.Add(rac);
				foreach (Artikal art in rac.Artikli.Keys)
				{
					art.Kolicina -= rac.Artikli[art];
				}

			}
		}

		private void Izlistaj(object sender, RoutedEventArgs e)
		{
			List<Racun> racuni = new List<Racun>();
			foreach (Racun r in Racuni)
			{
				if (r.VremeIzdavanja >= dateOd.SelectedDate && r.VremeIzdavanja <= dateDo.SelectedDate.Value.AddDays(1))
				{
					racuni.Add(r);
				}
			}
			dgIzvestaj.ItemsSource = racuni;
		}
	}
}
