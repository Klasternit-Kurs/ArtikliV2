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

namespace ImenikV2
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		ObservableCollection<Artikal> Artikli = new ObservableCollection<Artikal>();
		
		public string SifraZaRacun { get; set; }
		public int KolicinaZaRacun { get; set; }

		public MainWindow()
		{
			InitializeComponent();
			DataContext = new Racun();
			UniGrid.DataContext = this;
			dgTrenutniRacun.ItemsSource = (DataContext as Racun).Artikli;

			BinaryFormatter bf = new BinaryFormatter();
			using (FileStream fs = new FileStream("art.dat", FileMode.Open, FileAccess.Read))
			{
				Artikli = bf.Deserialize(fs) as ObservableCollection<Artikal>;
			}
			dg.ItemsSource = Artikli;
		}

		private void ZatvaramSe(object sender, System.ComponentModel.CancelEventArgs e)
		{
			BinaryFormatter bf = new BinaryFormatter();
			using (FileStream fs = new FileStream("art.dat", FileMode.Create, FileAccess.Write))
			{
				bf.Serialize(fs, Artikli);
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
			}
		}
	}
}
