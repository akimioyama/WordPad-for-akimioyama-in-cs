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
using Microsoft.Win32;
using System.IO;
using Path = System.IO.Path;

namespace WordPad_for_akimioyama
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool mb_save = false; // переменна для првоерки сохранения файла 
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Create_Click(object sender, RoutedEventArgs e)
        {
            TextBlock.Document.Blocks.Clear();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myDialog = new OpenFileDialog(); // диологове окно
            myDialog.Filter = "Файлы RTF (*.rtf)|*.rtf|Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*"; // фильтр 

            if (myDialog.ShowDialog() == true)  // отображаем
            {
                //Тут нужно будет спрашивать про сохранить или нет

                // Открываем докумен
                TextRange doc = new TextRange(TextBlock.Document.ContentStart, TextBlock.Document.ContentEnd); //раници открытия. От ночала и до конца
                using (FileStream fs = new FileStream(myDialog.FileName, FileMode.Open)) //  операции чтения и записи.
                {
                    if (Path.GetExtension(myDialog.FileName).ToLower() == ".rtf")
                        doc.Load(fs, DataFormats.Rtf);
                    else if (Path.GetExtension(myDialog.FileName).ToLower() == ".txt")
                        doc.Load(fs, DataFormats.Text);
                    else
                        doc.Load(fs, DataFormats.Xaml);
                }
            }
        }

        private void Save_As_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog(); // диологове окно
            sfd.Filter = "Файл RTF (*.rtf)|*.rtf|Текстовый файлы (*.txt)|*.txt|XAML Файл (*.xaml)|*.xaml|Все файлы (*.*)|*.*";  // фильтр 

            if (sfd.ShowDialog() == true)
            {
                TextRange doc = new TextRange(TextBlock.Document.ContentStart, TextBlock.Document.ContentEnd);
                using (FileStream fs = File.Create(sfd.FileName))
                {
                    if (Path.GetExtension(sfd.FileName).ToLower() == ".rtf")
                        doc.Save(fs, DataFormats.Rtf);
                    else if (Path.GetExtension(sfd.FileName).ToLower() == ".txt")
                        doc.Save(fs, DataFormats.Text);
                    else
                        doc.Save(fs, DataFormats.Xaml);
                }
                mb_save = true;
            }

        }
        private void Print_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            pd.PageRangeSelection = PageRangeSelection.AllPages;
            pd.UserPageRangeEnabled = true;
            

            if (pd.ShowDialog() == true)
            {
                pd.PrintVisual(TextBlock, "Привет");
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            var response = MessageBox.Show("Do you really want to exit?", "Exiting...",
                MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (response == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
                // првоерка на сохранеин ДОБАВИТЬ!
            }


        }

     
    }
}
