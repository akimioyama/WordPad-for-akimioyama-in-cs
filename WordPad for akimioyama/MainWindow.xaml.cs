using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
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
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using DataFormats = System.Windows.DataFormats;
using PrintDialog = System.Windows.Controls.PrintDialog;
using MessageBox = System.Windows.MessageBox;
using Application = System.Windows.Application;
using System.Drawing.Text;
using ComboBox = System.Windows.Controls.ComboBox;

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
            FillSizeComboBox(ComboBoxSize);
            FillFontComboBox(ComboBoxFont);
            ComboBoxSize.SelectedIndex = 4;
            TextBlock.FontSize = Convert.ToDouble(ComboBoxSize.SelectedItem) * 92.0 / 72.0;
            TextBlock.FontFamily = new FontFamily(ComboBoxFont.SelectedIndex.ToString());

        }
        //Шапка с тестом.................................
        public void FillSizeComboBox(ComboBox comboBoxSize)
        {
            int qwe = 15;
            int[] sizee = new int[16] {8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            int j = 0;
            for (int i = 0; i <= qwe; i++)
            {
                comboBoxSize.Items.Add(sizee[j]);
                j += 1;
            }
            comboBoxSize.SelectedIndex = 4;
        }
        public void FillFontComboBox(ComboBox ComboBoxFont)
        {
            foreach (FontFamily fontFamily in Fonts.SystemFontFamilies)
            {
                ComboBoxFont.Items.Add(fontFamily.Source);
            }
            ComboBoxFont.SelectedIndex = 2;
        }
        private void TextBlock_Selection(object sender, RoutedEventArgs e)
        {
            
        }
        private void SizeSelection(object sender, SelectionChangedEventArgs e)
        {
            double size = Convert.ToDouble(ComboBoxSize.SelectedItem) * 92.0 / 72.0;
            TextBlock.Selection.ApplyPropertyValue(System.Windows.Controls.RichTextBox.FontSizeProperty, size);
            TextBlock.Focus();
        }
        private void FontSelection(object sender, SelectionChangedEventArgs e)
        {
            string fontName = (string)ComboBoxFont.SelectedItem;

            if (fontName != null)
            {
                TextBlock.Selection.ApplyPropertyValue(System.Windows.Controls.RichTextBox.FontFamilyProperty, fontName);
                TextBlock.Focus();
            }
        }
        //..............................................
        private void NewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TextBlock.Document.Blocks.Clear();
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
        private void Save_Click(object sender, RoutedEventArgs e)
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
            var response = MessageBox.Show("Вы точно хотите выйти?", "Выход...",
                MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (response == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
                // првоерка на сохранеин ДОБАВИТЬ!
            }
        }
        private void Font_Click(object sender, RoutedEventArgs e)
        {
            FontDialog fd = new FontDialog();

            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {  
                TextBlock.FontFamily = new FontFamily(fd.Font.Name);
                TextBlock.FontSize = fd.Font.Size * 92.0 / 72.0;
                TextBlock.FontWeight = fd.Font.Bold ? FontWeights.Bold : FontWeights.Regular;
                TextBlock.FontStyle = fd.Font.Italic ? FontStyles.Italic : FontStyles.Normal;
            }
        }

        private void Color_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TextBlock.Foreground = new SolidColorBrush(Color.FromArgb(cd.Color.A, cd.Color.R, cd.Color.G, cd.Color.B));
            }
        }

        private void rrrr(object sender, RoutedEventArgs e)
        {
            if (ToolBar2.Visibility == Visibility.Visible)
            {
                ToolBar2.Visibility = Visibility.Hidden;
                rrr.IsChecked = false;
            }
            else
            {
                ToolBar2.Visibility = Visibility.Visible;
                rrr.IsChecked = true;
            }
        }
        private void qqqq(object sender, RoutedEventArgs e)
        { 
            if (ToolBar1.Visibility == Visibility.Visible)
            {
                ToolBar1.Visibility = Visibility.Hidden;
                qqq.IsChecked = false;
            }
            else
            {
                ToolBar1.Visibility = Visibility.Visible;
                qqq.IsChecked = true;
            }
        }

        private void Text_Chahged(object sender, TextChangedEventArgs e) //одна ошибка с перовым элиментом при выделение всего текста и нажатии кнопки
        {
            string fontName = (string)ComboBoxFont.SelectedItem;
        
            if (fontName != null)
            {
                double size = Convert.ToDouble(ComboBoxSize.SelectedItem) * 92.0 / 72.0;
                TextBlock.Selection.ApplyPropertyValue(System.Windows.Controls.RichTextBox.FontSizeProperty, size);
                TextBlock.Selection.ApplyPropertyValue(System.Windows.Controls.RichTextBox.FontFamilyProperty, fontName);
                TextBlock.Focus();
            }
        }
    }
}
