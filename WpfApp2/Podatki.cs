using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace WpfApp2
{
    public class Podatki : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
                Debug.WriteLine("Zaznana sprememba");
            }
        }


        private ObservableCollection<ObjavaNaZidu> objave;
        public ObservableCollection<ObjavaNaZidu> VseObjave {
            get
            {
                return objave;
            }
        }
        public void DodajObjavo(ObjavaNaZidu oz)
        {
            objave.Add(oz);
        }

        public int dobi_id_zadnje() {
            if(objave.Count != 0)
            return objave.Last().ID;
            else
                return 0;
        }

        public void OdstraniObjavo(int id) {
            foreach (ObjavaNaZidu o in objave) {
                if (o.ID == id)
                {
                    objave.Remove(o);
                    System.IO.File.Delete(o.Text_Objave);
                    break;
                }
            }
        }

        private ObservableCollection<Prijatelji> seznam_prijateljev;

        public void sortirajPrijateljeABC() {
            seznam_prijateljev = new ObservableCollection<Prijatelji>(seznam_prijateljev.OrderBy(x => x.Ime));
            NotifyPropertyChanged("VsiPrijatelji");
        }
        public void sortirajPrijateljeStopnja()
        {
            seznam_prijateljev = new ObservableCollection<Prijatelji>(seznam_prijateljev.OrderBy(x => x.Stopnja));
            NotifyPropertyChanged("VsiPrijatelji");
        }
        public ObservableCollection<Prijatelji> VsiPrijatelji
        {
            get
            {
                return seznam_prijateljev;
            }
        }
        public void DodajPrijatelja(Prijatelji p)
        {
            seznam_prijateljev.Add(p);
        }
        public void OdstraniPrijatelja(Prijatelji p) {
            seznam_prijateljev.Remove(p);
        }
        

        private string Ime_osebe;
        public string ImeOsebe
        {
            set
            {
                if (Ime_osebe != value)
                {
                    Ime_osebe = value;
                    NotifyPropertyChanged("ImeOsebe");
                    this.PolnoIme = Ime_osebe + " " + Priimek_osebe;
                }
            }

            get
            {
                return Ime_osebe;
            }
        }
        private string Priimek_osebe;
        public string PriimekOsebe
        {
            set
            {
                if (Priimek_osebe != value)
                {
                    Priimek_osebe = value;
                    NotifyPropertyChanged("PriimekOsebe");
                    this.PolnoIme = Ime_osebe + " " + Priimek_osebe;
                }
            }

            get
            {
                return Priimek_osebe;
            }
        }


        private string Polno_ime;
        public string PolnoIme
        {
            set
            {
                if (Polno_ime != value) { 
                    Polno_ime = Ime_osebe + " " + Priimek_osebe;
                    NotifyPropertyChanged("PolnoIme");
                }
            }

            get
            {
                return Polno_ime;
            }
        }


        private string Datum_Rojstva;
        public string DatumRojstva
        {
            set
            {
                if (Datum_Rojstva != value)
                {
                    Datum_Rojstva = value;
                    NotifyPropertyChanged("DatumRojstva");
                }
            }

            get
            {
                return Datum_Rojstva;
            }
        }

        private string Sola_;
        public string Sola
        {
            set
            {
                if (Sola_ != value)
                {
                    Sola_ = value;
                    NotifyPropertyChanged("Sola");
                }
            }
            get
            {
                return Sola_;
            }
        }

        private string Spol_;
        public string Spol
        {
            set
            {
                if (Spol_ != value)
                {
                    Spol_ = value;
                    NotifyPropertyChanged("Spol");
                }
            }

            get
            {
                return Spol_;
            }
        }

        private string Delo_;
        public string Delo
        {
            set
            {
                if (Delo_ != value) { 
                    Delo_ = value;
                    NotifyPropertyChanged("Delo");
                }
            }
            get
            {
                return Delo_;
            }
        }

        private string Naslovna_;
        public string Naslovna {
            set
            {
                if (Naslovna_ != value) {
                    Naslovna_ = value;
                    NotifyPropertyChanged("Naslovna");
                }
            }
            get {
                return Naslovna_;
            }
        }

        private string Prikazna_;
        public string Prikazna {
            set
            {
                if (Prikazna_ != value) {
                    Prikazna_ = value;
                    NotifyPropertyChanged("Prikazna");
                }
            }
            get {
                return Prikazna_;
            }
        }


        public Podatki()
        {
            Ime_osebe = "Janez";
            Priimek_osebe = "Novak";
            Datum_Rojstva = "1.1.1995";
            Sola_ = "FERI";
            Spol_ = "Moški";
            Delo_ = "Študent";
            Naslovna_ = @"Resources\Naslovna_b.jpg";
            Prikazna_ = @"Resources\profilan_s.jpg";
            objave = new ObservableCollection<ObjavaNaZidu>();
            seznam_prijateljev = new ObservableCollection<Prijatelji>();
            Polno_ime = Ime_osebe + " " + Priimek_osebe;
        }

        public void Init() {
            objave.Add(new ObjavaNaZidu());
            seznam_prijateljev.Add(new Prijatelji());
        }
    }


    public class ObjavaNaZidu
    {
        public string Text_Objave { set; get; }
        public string Lokacija { set; get; }
        public string Zasebnost { set; get; }
        public string Barva_pisave { set; get; }
        public string Barva_ozadja { set; get; }
        public string Slika { set; get; }
        public string Video { set; get; }
        public int ID { set;  get; }
        public string cas { set; get; }
        
        public ObjavaNaZidu()
        {
            ID = 0;
            Text_Objave = "Objava";
            Lokacija = "Maribor";
            Zasebnost = "Prijatelji";
            Barva_pisave = "Crna";
            Barva_ozadja = "Bela";
            cas = DateTime.Now.ToString();
            Slika = @"C:\Users\rajh\Desktop\Vmesniki\WpfApp2\Resources\images.jpg";
            Video = "NULL";
        }

        public ObjavaNaZidu(int id, string to, string l, string z, string bp, string bo, string s, string v, string c)
        {
            ID = id;
            Text_Objave = to;
            Lokacija = l;
            Zasebnost = z;
            Barva_pisave = bp;
            Barva_ozadja = bo;
            Slika = s;
            Video = v;
            cas = c;
        }
    }

    public class Prijatelji {
        public string Ime { set; get; }
        public string Priimek { set; get; }
        public string Lokacija { set; get; }

        public string Stopnja { set; get; }

        public const string d_p = "DoberPrijatelj";
        public const string z = "Znanec";
        public const string n = "zNeznanec";

        public Prijatelji()
        {
            Ime = "Mirko";
            Priimek = "Novak";
            Lokacija = "Ljubljana";
            Stopnja = d_p;
        }

        public Prijatelji(string i, string p, string l, string s)
        {
            Ime = i;
            Priimek =p;
            Lokacija = l;
            Stopnja = s;
        }

        public override string ToString()
        {
            return Ime + " " + Priimek + ", " + Lokacija;
        }
    }
}

