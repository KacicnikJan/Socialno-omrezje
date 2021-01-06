using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Windows.Media;
using System.IO;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public Podatki podatki =  new Podatki();
        System.Threading.Timer timer;

        public MainWindow()
        {
            InitializeComponent();
             DataContext = podatki;
            //Podatki tmp = new Podatki();
            //tmp.Init();
            //podatki.Add(tmp);
            
            uvozi_podatke("Podatki.xml");

            //timer za shranjevanje podatkov vsakih 5 minut
            timer = new System.Threading.Timer((e) =>
            {
                izvozi_podatke(@"Podatki.xml");
            }, null, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));
            
        }

        public void objava_dodaj(ObjavaNaZidu obz) {
            podatki.DodajObjavo(obz);
        }

        public void prijatelj_dodaj(Prijatelji pri)
        {
            podatki.DodajPrijatelja(pri);
            Debug.WriteLine(this.DataContext.Equals(podatki));
            //DataContext = podatki;
        }

        private void izhod_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Objava wnd = new Objava();
            wnd.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Dodaj_prijatelja wnd = new Dodaj_prijatelja();
            wnd.ShowDialog();
        }

        
        private void Button_Click_1(object sender, RoutedEventArgs e) {
            TextBlock opozorilo = new TextBlock();

            if (seznam_prijateljev.SelectedItem != null)
            {
                Prijatelji pri = seznam_prijateljev.SelectedItem as Prijatelji;
                podatki.OdstraniPrijatelja(pri);
                if (opozorilo != null)
                    opozorilo.Text = null;
                    prijatelji.Children.Remove(opozorilo);
            }
            else
            {
                if (opozorilo.Text != "izberite osebo, ki jo želiti odstraniti!")
                {   
                    opozorilo.Margin = new Thickness(10);
                    opozorilo.Foreground = new SolidColorBrush(Colors.Red);

                    opozorilo.Text = "izberite osebo, ki jo želiti odstraniti!";
                    Grid.SetColumn(opozorilo, 1);
                    Grid.SetRow(opozorilo, 2);
                    prijatelji.Children.Add(opozorilo);
                }
            }
        }

        private void izvozi_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "XML Files (*.xml)|*.xml";

            if (sfd.ShowDialog() == true) { 

                    XmlSerializer ser = new XmlSerializer(typeof(Podatki));
                    TextWriter wr = new StreamWriter(sfd.FileName);
                    ser.Serialize(wr, podatki);
                    wr.Close(); 
            }
        }

        private void izvozi_podatke(string pot) {
            XmlSerializer ser = new XmlSerializer(typeof(Podatki));
            TextWriter wr = new StreamWriter(pot);
            ser.Serialize(wr, podatki);
            wr.Close();
        }


        private void uvozi_podatke(string pot) {
            if (File.Exists(pot))
            {
                XmlSerializer serializer1 = new XmlSerializer(typeof(Podatki));
                Debug.WriteLine(this.DataContext.Equals(podatki));
                StreamReader reader = new StreamReader(@"" + pot);
                podatki = (Podatki)serializer1.Deserialize(reader);
                reader.Close();
                this.DataContext = podatki;
                //if (!File.Exists(@"Resources\Naslovna_b.jpg")) {
                //   Prikazna.Source = new BitmapImage(new Uri(@"pack://application:,,/Resources\profilan_s.jpg"));
                //    Debug.WriteLine("Nastavljena privzeta prikazna");
                //}
                //if (!File.Exists(podatki.Naslovna))
                //{
                //    Naslovna.Source = new BitmapImage(new Uri(@"pack://application:,,/Resources\Naslovna_b.jpg"));
                //    Debug.WriteLine("Nastavljena privzeta naslovna");
                //}
            }
            else {
                podatki = new Podatki();
            }


            zid_objav.Children.Clear();

            foreach (ObjavaNaZidu ob in podatki.VseObjave)
            {
                MojGradnikObjava obj = new MojGradnikObjava();
                obj.dodaj_Lokacijo(ob.Lokacija);
                obj.dodaj_kdo(ob.Zasebnost);
                obj.nastavi_cas(ob.cas);
               
                Color text = new Color();
                Color back = new Color();

                if (ob.Barva_pisave.Contains("Črna"))
                {
                    text = Colors.Black;
                }
                if (ob.Barva_pisave.Contains("Modra"))
                {
                    text = Colors.Blue;
                }
                if (ob.Barva_pisave.Contains("Rdeča"))
                {
                    text = Colors.IndianRed;
                }



                if (ob.Barva_ozadja.Contains("Normalno"))
                {
                    back = Colors.White;
                }

                if (ob.Barva_ozadja.Contains("Vijolično"))
                {
                    back = Colors.LightPink;
                }
                if (ob.Barva_ozadja.Contains("Zeleno"))
                {
                    back = Colors.LightGreen;
                }

                RichTextBox rtb = new RichTextBox();
                TextRange t = new TextRange(rtb.Document.ContentStart,
                                rtb.Document.ContentEnd);
                FileStream file = new FileStream(ob.Text_Objave, FileMode.Open);
                t.Load(file, System.Windows.DataFormats.XamlPackage);
                file.Close();

                obj.dodaj_dopis(rtb, back, text);


                if (ob.Slika != "NULL")
                {
                    obj.dodaj_sliko(ob.Slika);
                }
                else if (ob.Video != "NULL")
                {
                    obj.dodaj_video(ob.Video);
                }

                obj.Name = "objava" + ob.ID.ToString();
                DockPanel.SetDock(obj, Dock.Bottom);

                ((MainWindow)Application.Current.MainWindow).zid_objav.Children.Add(obj);
            }
        }

        private void Uvozi_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Title = "Izberite sliko";
            openFileDialog.Filter = "XML Files (*.xml)|*.xml";

            if (openFileDialog.ShowDialog() == true)
            {
                uvozi_podatke(openFileDialog.FileName);
                
            }
        }

        private void Iskanje_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Prijatelji> filtrirano = new List<Prijatelji>();
            string iskano = Iskanje.Text;

            foreach (Prijatelji pr in podatki.VsiPrijatelji) {
                string ime_priimek = pr.Ime.ToLower() + " " + pr.Priimek.ToLower();
                if (pr.Ime.ToLower().Contains(iskano.ToLower()))
                {
                    filtrirano.Add(pr);
                }
                else if (pr.Priimek.Contains(iskano))
                {
                    filtrirano.Add(pr);
                }
                else if (pr.Lokacija.Contains(iskano)) {
                    filtrirano.Add(pr);
                }
                else if (ime_priimek.Contains(iskano.ToLower())) {
                    filtrirano.Add(pr);
                }
            }
            if (iskano == "")
            {
                seznam_prijateljev.ItemsSource = podatki.VsiPrijatelji;
            }
            else
            {
                seznam_prijateljev.ItemsSource = filtrirano;
            }
        }

        private void btnSpremembaPodatkov(object sender, RoutedEventArgs e)
        {
            if (tbIme.Text != podatki.ImeOsebe.ToString()) {
                podatki.ImeOsebe = tbIme.Text.ToString();
            }
            if (tbPriimek.Text != podatki.PriimekOsebe.ToString())
            {
                podatki.PriimekOsebe = tbPriimek.Text;
            }
            if (tbDatum.Text != podatki.DatumRojstva.ToString())
            {
                podatki.DatumRojstva = tbDatum.Text;
            }
            if (tbSola.Text != podatki.Sola.ToString())
            {
                podatki.Sola = tbSola.Text;
            }
            if (tbSpol.Text != podatki.Spol.ToString())
            {
                podatki.Spol = tbSpol.Text;
            }
            if (tbDelo.Text != podatki.Delo.ToString())
            {
                podatki.Delo = tbDelo.Text;
            }

            //animacija gumba
            DoubleAnimation animacija_gumba = new DoubleAnimation();
            animacija_gumba.From = shrani.ActualHeight;
            animacija_gumba.To = shrani.ActualHeight - 10;
            animacija_gumba.AutoReverse = true;
            animacija_gumba.Duration = new Duration(TimeSpan.FromSeconds(.5));
            shrani.BeginAnimation(Button.HeightProperty, animacija_gumba);

        }

        private void ABC_Selected(object sender, RoutedEventArgs e)
        {
            podatki.sortirajPrijateljeABC();
        }

        private void Stopnja_selected(object sender, RoutedEventArgs e)
        {
            podatki.sortirajPrijateljeStopnja();
        }
    }
}
