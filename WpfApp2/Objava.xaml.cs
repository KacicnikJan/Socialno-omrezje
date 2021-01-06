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
using System.IO;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using System.Windows.Media.Animation;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for Objava.xaml
    /// </summary>
    public partial class Objava : Window
    {
        bool default_text = true;
        bool se_predvaja = false;
        string opozorilo;
        string ime_slike = "NULL";
        string pot_videa = "NULL";
        Color text;
        Color back;
        string barva_ozadja;
        string barva_pisave;
        string zasebnost;
        public Objava()
        {
            seznam_prijateljev_objava = new ListBox();
            InitializeComponent();
            PlayPause.Visibility = Visibility.Hidden;
            //pridobimo seznam prijateljev
            DataContext = ((MainWindow)Application.Current.MainWindow).podatki;
            
        }

        //izbriše osnovni text, ki je v vnosnem polju
        private void RichTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (default_text == true) { 
                ((RichTextBox)sender).Document = new FlowDocument();
                default_text = false;
            }
        }

        //zapiranje okna
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //objavi objavo na zid
        private void Btn_Objavi(object sender, RoutedEventArgs e)
        {

            if (Lok.ToString().Length > 32 && zasebnost != null && back != null && text != null && barva_ozadja != null && barva_pisave != null) {
                MojGradnikObjava gradnik_obj = new MojGradnikObjava();

                gradnik_obj.dodaj_Lokacijo(Lok.ToString().Substring(33));
                gradnik_obj.dodaj_kdo(zasebnost);

                int id = ((MainWindow)Application.Current.MainWindow).podatki.dobi_id_zadnje() + 1;
                string ime_dat = "text_obj_" + id.ToString() + ".xaml";
                TextRange t = new TextRange(text_objave.Document.ContentStart,
                                    text_objave.Document.ContentEnd);
                FileStream file = new FileStream(ime_dat, FileMode.Create);
                t.Save(file, System.Windows.DataFormats.XamlPackage);
                file.Close();

                gradnik_obj.dodaj_dopis(text_objave, back, text);

                if (ime_slike != "NULL")
                {
                    gradnik_obj.dodaj_sliko(ime_slike);
                    Debug.WriteLine(ime_slike);
                }
                else if (pot_videa != "NULL") {
                    gradnik_obj.dodaj_video(pot_videa);
                }

                

                string cas = DateTime.Now.ToString();
                gradnik_obj.nastavi_cas(cas);
                ((MainWindow)Application.Current.MainWindow).objava_dodaj(new ObjavaNaZidu(id,ime_dat, Lok.ToString().Substring(33), zasebnost, barva_pisave, barva_ozadja, ime_slike, pot_videa,cas));
                gradnik_obj.Name = "objava" + id.ToString();
                DockPanel.SetDock(gradnik_obj, Dock.Bottom);
                ((MainWindow)Application.Current.MainWindow).zid_objav.Children.Add(gradnik_obj);
            }
            

        }

        //dodajanje prijateljev v objavo
        private void Btn_Prijatelj(object sender, RoutedEventArgs e)
        {
            if (seznam_prijateljev_objava.SelectedItem != null)
            {
                String prijatelj = "@" + seznam_prijateljev_objava.Items[seznam_prijateljev_objava.SelectedIndex].ToString().Substring(37); //brez System.Windows.Controls.ListBoxItem:
                text_objave.AppendText(prijatelj);
            }
        }

        private void Btn_Slika(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Title = "Izberite sliko";

            if (openFileDialog.ShowDialog() == true)
            {
                Dodana_slika.Source = new BitmapImage(new Uri(openFileDialog.FileName));

                ime_slike = openFileDialog.FileName;

                //dodana animacija
                PathFigure tocke_poti = new PathFigure();
                TranslateTransform premikanje = new TranslateTransform();
                PolyBezierSegment pbs = new PolyBezierSegment();
                PathGeometry gibanje = new PathGeometry();            

                Dodana_slika.RenderTransform = premikanje;

                pbs.Points.Add(new Point(-40, 0));
                pbs.Points.Add(new Point(-20, 0));
                pbs.Points.Add(new Point(0, 0));
                pbs.Points.Add(new Point(50, 0));

                tocke_poti.StartPoint = new Point(-80, 0);
                tocke_poti.Segments.Add(pbs);
                gibanje.Figures.Add(tocke_poti);

                DoubleAnimationUsingPath gibanje_slike = new DoubleAnimationUsingPath();
                gibanje_slike.PathGeometry = gibanje;
                gibanje_slike.Duration = TimeSpan.FromMilliseconds(400);

                this.RegisterName("animirano_dodajanje_slike", premikanje);

                Storyboard sb = new Storyboard();
                sb.Children.Add(gibanje_slike);
                Storyboard.SetTargetProperty(gibanje_slike, new PropertyPath(TranslateTransform.XProperty));
                Storyboard.SetTargetName(gibanje_slike, "animirano_dodajanje_slike");
                sb.Begin(this);
                this.UnregisterName("animirano_dodajanje_slike");
            }

        }

        private void Btn_Video(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Title = "Izberite video";

            if (openFileDialog.ShowDialog() == true)
            {
                Dodan_video.Source = new Uri(openFileDialog.FileName);
                Dodan_video.Play();
                se_predvaja = true;
                PlayPause.Visibility = Visibility.Visible;
                pot_videa = openFileDialog.FileName.ToString();
            }
        }

        private void Btn_Play(object sender, RoutedEventArgs e)
        {
            if (se_predvaja == true)
            {
                Dodan_video.Pause();
                se_predvaja = false;
            }
            else if (se_predvaja == false)
            {
                Dodan_video.Play();
                se_predvaja = true;
            }
        }

        private void Btn_wow(object sender, RoutedEventArgs e) {

            System.Drawing.Image tempImage = WpfApp2.Properties.Resources.wow_ss;
            Clipboard.SetDataObject(tempImage);
            text_objave.Paste();
        }

        private void haha_Click(object sender, RoutedEventArgs e)
        {
            System.Drawing.Image tempImage = WpfApp2.Properties.Resources.haha_rs;
            Clipboard.SetDataObject(tempImage);
            text_objave.Paste();
        }

        private void Btn_Angry(object sender, RoutedEventArgs e)
        {
            System.Drawing.Image tempImage = WpfApp2.Properties.Resources.angry_rs;
            Clipboard.SetDataObject(tempImage);
            text_objave.Paste();
        }

        private void Btn_Sad(object sender, RoutedEventArgs e)
        {
            System.Drawing.Image tempImage = WpfApp2.Properties.Resources.sad_rs;
            Clipboard.SetDataObject(tempImage);
            text_objave.Paste();
        }

        private void Btn_Heart(object sender, RoutedEventArgs e)
        {
            System.Drawing.Image tempImage = WpfApp2.Properties.Resources.heart_rs;
            Clipboard.SetDataObject(tempImage);
            text_objave.Paste();
        }

        void btn_prijatelj(object sender, MouseEventArgs e)
        {
            if (seznam_prijateljev_objava.SelectedItem != null)
            { Prijatelji pr = ((Prijatelji)seznam_prijateljev_objava.SelectedItem);
                String prijatelj = "@" + pr.Ime + " " + pr.Priimek; //brez System.Windows.Controls.ListBoxItem:
                text_objave.AppendText(prijatelj);
            }
        }

        private void Prijatelji_Selected(object sender, RoutedEventArgs e)
        {
            zasebnost = "prijatelji";
        }

        private void Vsi_Selected(object sender, RoutedEventArgs e)
        {
            zasebnost = "vsi";
        }

        private void Jaz_Selected(object sender, RoutedEventArgs e)
        {
            zasebnost = "samo jaz";
        }

        private void Crna_Selected(object sender, RoutedEventArgs e)
        {
            text_objave.Foreground = new SolidColorBrush(Colors.Black);
            text = Colors.Black;
            barva_pisave = "Črna";
        }

        private void Modra_Selected(object sender, RoutedEventArgs e)
        {
            text_objave.Foreground = new SolidColorBrush(Colors.Blue);
            text = Colors.Blue;
            barva_pisave = "Modra";
        }

        private void Rdeca_Selected(object sender, RoutedEventArgs e)
        {
            text_objave.Foreground = new SolidColorBrush(Colors.IndianRed);
            text = Colors.IndianRed;
            barva_pisave = "Rdeča";
        }

        private void Normalno_Selected(object sender, RoutedEventArgs e)
        {
            text_objave.Background = new SolidColorBrush(Colors.White);
            back = Colors.White;
            barva_ozadja = "Normalno";
        }

        private void Zeleno_Selected(object sender, RoutedEventArgs e)
        {
            text_objave.Background = new SolidColorBrush(Colors.LightGreen);
            back = Colors.LightGreen;
            barva_ozadja = "Zeleno";
        }

        private void Vijolicno_Selected(object sender, RoutedEventArgs e)
        {
            text_objave.Background = new SolidColorBrush(Colors.LightPink);
            back = Colors.LightPink;
            barva_ozadja = "Vijolično";
    }
    }
}

