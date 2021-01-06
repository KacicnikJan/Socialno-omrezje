using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Diagnostics;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Dodaj_prijatelja : Window
    {
        string stp = "";
        //MainWindow main;
        public Dodaj_prijatelja()
        {
            InitializeComponent();
        }
        TextBlock opozorilo = new TextBlock();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string ime = Ime_prijatelja.Text;
            string priimek = Priimek_prijatelja.Text;
            string kraj = Kraj_prijatelja.Text;
            if (ime.Length <= 0 || priimek.Length <= 0 || kraj.Length <= 0 || stp == null)
            {
                if (opozorilo.Text != "Vnesti je potrebno vse podatke!")
                {
                    opozorilo.Margin = new Thickness(5);
                    opozorilo.Foreground = new SolidColorBrush(Colors.Red);

                    opozorilo.Text = "Vnesti je potrebno vse podatke!";
                    Grid.SetColumnSpan(opozorilo, 2);
                    
                    opozorilo.TextAlignment = TextAlignment.Center;
                    opozorilo.VerticalAlignment = VerticalAlignment.Bottom;
                    Grid.SetRow(opozorilo, 4);
                    Grid.SetColumn(opozorilo, 1);
                    dodajanje.Children.Add(opozorilo);
                }
            }
            else
            {
                if (opozorilo != null)
                    opozorilo.Text = null;
                dodajanje.Children.Remove(opozorilo);
                string prijatelj = ime + " " + priimek;
                ((MainWindow)Application.Current.MainWindow).prijatelj_dodaj(new Prijatelji(ime, priimek, kraj,stp));
                    this.DataContext = ((MainWindow)Application.Current.MainWindow).podatki;
                //((MainWindow)Application.Current.MainWindow).seznam_prijateljev.ItemsSource = ((MainWindow)Application.Current.MainWindow).pod.seznam_prijateljev;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Dober_Selected(object sender, RoutedEventArgs e)
        {
            stp = Prijatelji.d_p;
        }

        private void Znanec_Selected(object sender, RoutedEventArgs e)
        {
            stp = Prijatelji.z;
        }

        private void Neznanec_Selected(object sender, RoutedEventArgs e)
        {
            stp = Prijatelji.n;
        }
    }
}
