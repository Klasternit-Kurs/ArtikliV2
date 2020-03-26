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

		public MainWindow()
		{
			InitializeComponent();
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
	}
}
