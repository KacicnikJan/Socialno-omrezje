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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MojGradnikObjava.xaml
    /// </summary>
    public partial class MojGradnikObjava : UserControl
    {
        bool se_predvaja = false;
        public MojGradnikObjava()
        {
            InitializeComponent();
            PlayPause.Visibility = Visibility.Hidden;
        }

        public void dodaj_Lokacijo(string lok)
        {
            Lokacija.Text = Lokacija.Text.ToString() + lok;
        }

        public void dodaj_Ime(string ime)
        {
            Oseba.Text = ime + Oseba.Text.ToString();
        }

        public void dodaj_kdo(string k)
        {
            Kdo.Text = Kdo.Text.ToString() + k;
        }
        public void dodaj_sliko(string pot) {
            slika.Source = new BitmapImage(new Uri(@""+pot));
        }

        public void dodaj_video(string pot) {
            video.Source = new Uri(@""+pot);
            PlayPause.Visibility = Visibility.Visible;
            video.Play();
            se_predvaja = true;
        }

        public void nastavi_cas(string c)
        {
            cas.Text = c;
        }
        private void Btn_Play(object sender, RoutedEventArgs e)
        {
            if (se_predvaja == true)
            {
                video.Pause();
                se_predvaja = false;
            }
            else if (se_predvaja == false)
            {
                video.Play();
                se_predvaja = true;
            }
        }

        public void dodaj_dopis(RichTextBox rtb, Color ozadje, Color pisava) {
            rtb.SelectAll();
            rtb.Copy();
            vsebina.Background = new SolidColorBrush(ozadje);
            vsebina.Foreground = new SolidColorBrush(pisava);
            vsebina.IsReadOnly = true;
            vsebina.Paste();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RichTextBox rtb = new RichTextBox();
            rtb.Margin = new Thickness(10);
            Komentarij.SelectAll();
            Komentarij.Copy();
            rtb.Paste();
            rtb.IsReadOnly = true;
            DockPanel.SetDock(rtb, Dock.Top);
            Kom.Children.Add(rtb);
        }

        private void btn_odstrani(object sender, RoutedEventArgs e)
        {
            string name = this.Name;
            int id = Int32.Parse(name.Substring(6));
            Debug.WriteLine(id);
            ((MainWindow)Application.Current.MainWindow).zid_objav.Children.Remove(this);
            ((MainWindow)Application.Current.MainWindow).podatki.OdstraniObjavo(id);
        }

        private void Btn_wow(object sender, RoutedEventArgs e)
        {

            System.Drawing.Image tempImage = WpfApp2.Properties.Resources.wow_ss;
            Clipboard.SetDataObject(tempImage);
            Komentarij.Paste();
        }

        private void haha_Click(object sender, RoutedEventArgs e)
        {
            System.Drawing.Image tempImage = WpfApp2.Properties.Resources.haha_rs;
            Clipboard.SetDataObject(tempImage);
            Komentarij.Paste();
        }

        private void Btn_Angry(object sender, RoutedEventArgs e)
        {
            System.Drawing.Image tempImage = WpfApp2.Properties.Resources.angry_rs;
            Clipboard.SetDataObject(tempImage);
            Komentarij.Paste();
        }

        private void Btn_Sad(object sender, RoutedEventArgs e)
        {
            System.Drawing.Image tempImage = WpfApp2.Properties.Resources.sad_rs;
            Clipboard.SetDataObject(tempImage);
            Komentarij.Paste();
        }

        private void Btn_Heart(object sender, RoutedEventArgs e)
        {
            System.Drawing.Image tempImage = WpfApp2.Properties.Resources.heart_rs;
            Clipboard.SetDataObject(tempImage);
            Komentarij.Paste();
        }
    }
}
