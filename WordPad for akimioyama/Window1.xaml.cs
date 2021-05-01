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

namespace WordPad_for_akimioyama
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();

            DateTime localDate = DateTime.Now;
            String[] cultureNames = { "en-US", "en-GB", "fr-FR", "ru-RU" };

            foreach (var cultureName in cultureNames)
            {
                var culture = new System.Globalization.CultureInfo(cultureName);
                DataBox.Items.Add(localDate.ToString(culture));
            }
            DataBox.Items.Add(localDate.ToString("dd MMMM yyyy"));
            DataBox.Items.Add(localDate.ToString("HH:mm:ss"));
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DataBox.SelectedIndex != -1)
            {
                string item = (string)DataBox.SelectedItem;
                Data.deta_time = item;
                Window2.Close();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Window2.Close();
        }
    }
}
