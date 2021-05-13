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
using Prism.Services.Dialogs;
using System.Windows.Controls.Primitives;

//< MenuItem Header = "Предварительный просмотр" ></ MenuItem >
//< MenuItem Header = "Параметры страницы" ></ MenuItem > 
//< Separator />
//< MenuItem Header = "Отправить" ></ MenuItem >
//< MenuItem Header = "Найти" ></ MenuItem >
//< MenuItem Header = "Заменить" ></ MenuItem >
// Command="ToggleBold "

namespace WordPad_for_akimioyama
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool mb_save = false; // переменна для првоерки сохранения файла 
        string path = @"";

        public MainWindow()
        {
            InitializeComponent();
            FillSizeComboBox(ComboBoxSize);
            FillFontComboBox(ComboBoxFont);
            

            double size = Convert.ToDouble(ComboBoxSize.SelectedItem) * 92.0 / 72.0;
            string fontName = (string)ComboBoxFont.SelectedItem;
            TextBlock.Selection.ApplyPropertyValue(System.Windows.Controls.RichTextBox.FontSizeProperty, size);
            TextBlock.Selection.ApplyPropertyValue(System.Windows.Controls.RichTextBox.FontFamilyProperty, fontName);
            leftt.IsChecked = true;
            TextBlock.Focus();

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


        //Создать, открыть, созранить как...............
        private void Create_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
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
                path = sfd.FileName.ToString();
            }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            //SaveFileDialog sfd = new SaveFileDialog(); // диологове окно
            //sfd.Filter = "Файл RTF (*.rtf)|*.rtf|Текстовый файлы (*.txt)|*.txt|XAML Файл (*.xaml)|*.xaml|Все файлы (*.*)|*.*";  // фильтр 

            if (mb_save == false && path != "")
            {
                TextRange doc = new TextRange(TextBlock.Document.ContentStart, TextBlock.Document.ContentEnd);
                FileStream fStream;
                if (Path.GetExtension(path) == ".rtf")
                {
                    fStream = new FileStream(path, FileMode.Create);
                    doc.Save(fStream, DataFormats.Rtf);
                }
                else if (Path.GetExtension(path) == ".txt")
                {
                    fStream = new FileStream(path, FileMode.Create);
                    doc.Save(fStream, DataFormats.Text);
                }
                else
                {
                    fStream = new FileStream(path, FileMode.Create);
                    doc.Save(fStream, DataFormats.Xaml);
                }
                mb_save = true;
            }
            else
            {

            }
        }
        //..............................................


        //Диалоговые окна..............................
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
                if (mb_save == false)
                {
                    var qwe = MessageBox.Show("Хотите сохранить файл?", "Сохранение", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                    if (qwe == MessageBoxResult.No)
                    {
                        Application.Current.Shutdown();
                    }
                    else
                    {
                        if (path == "")
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
                                Application.Current.Shutdown();
                            }
                        }
                        else
                        {
                            TextRange doc = new TextRange(TextBlock.Document.ContentStart, TextBlock.Document.ContentEnd);
                            FileStream fStream;
                            if (Path.GetExtension(path) == ".rtf")
                            {
                                fStream = new FileStream(path, FileMode.Create);
                                doc.Save(fStream, DataFormats.Rtf);
                            }
                            else if (Path.GetExtension(path) == ".txt")
                            {
                                fStream = new FileStream(path, FileMode.Create);
                                doc.Save(fStream, DataFormats.Text);
                            }
                            else
                            {
                                fStream = new FileStream(path, FileMode.Create);
                                doc.Save(fStream, DataFormats.Xaml);
                            }
                            mb_save = true;
                        }
                        Application.Current.Shutdown();
                    }
                }
                else
                {
                    Application.Current.Shutdown();
                }

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
            mb_save = false;
        }
        private void Color_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            
            if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SolidColorBrush col = new SolidColorBrush(Color.FromArgb(cd.Color.A, cd.Color.R, cd.Color.G, cd.Color.B));
                TextBlock.Selection.ApplyPropertyValue(System.Windows.Controls.RichTextBox.ForegroundProperty, col);
            }
            mb_save = false;
        }
        //..............................................



        //Отображение элементов.........................
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
                Thickness posithion = new Thickness(-12, -1, 0, 0);
                ToolBar2.Margin = posithion;
                qqq.IsChecked = false;
            }
            else
            {
                ToolBar1.Visibility = Visibility.Visible;
                Thickness posithion = new Thickness(-12, 32, 0, 0);
                ToolBar2.Margin = posithion;
                qqq.IsChecked = true;
            }
        }
        //..............................................



        private void Text_Chahged(object sender, TextChangedEventArgs e) //одна ошибка с перовым элиментом при выделение всего текста и нажатии кнопки
        {
            TextRange doc = new TextRange(TextBlock.Document.ContentStart, TextBlock.Document.ContentEnd);
            string fontName = (string)ComboBoxFont.SelectedItem;
            double size = Convert.ToDouble(ComboBoxSize.SelectedItem) * 92.0 / 72.0;

            if (doc.Text.Length != 0) 
            {
                

                if (fontName != null)
                {
                    
                    TextBlock.Selection.ApplyPropertyValue(System.Windows.Controls.RichTextBox.FontSizeProperty, size);
                    TextBlock.Selection.ApplyPropertyValue(System.Windows.Controls.RichTextBox.FontFamilyProperty, fontName);
                    TextBlock.Focus();
                }
            }
            else
            {
              
                
                TextBlock.Selection.ApplyPropertyValue(System.Windows.Controls.RichTextBox.FontSizeProperty, "12");
                TextBlock.Selection.ApplyPropertyValue(System.Windows.Controls.RichTextBox.FontFamilyProperty, fontName);
            }
            mb_save = false;
        }




        //Дата и время................
        private void Data_Time_Click(object sender, RoutedEventArgs e)
        {
            Window1 taskWindow = new Window1();
            taskWindow.Owner = this;
            taskWindow.ShowDialog();
            Print();
            mb_save = false;
        }
        private void Print()
        {
            string new_text = Data.deta_time;

            string fontName = (string)ComboBoxFont.SelectedItem;
            double size = Convert.ToDouble(ComboBoxSize.SelectedItem) * 92.0 / 72.0;
            TextBlock.Selection.ApplyPropertyValue(System.Windows.Controls.RichTextBox.FontSizeProperty, size);
            TextBlock.Selection.ApplyPropertyValue(System.Windows.Controls.RichTextBox.FontFamilyProperty, fontName);

            TextBlock.AppendText(new_text);
            TextBlock.Focus();
        }
        //............................



        //Поиск.......................
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            Window2 taskWindow = new Window2();
            taskWindow.Owner = this;
            taskWindow.ShowDialog();
            TextRange txtReturn = new TextRange(TextBlock.Document.ContentStart, TextBlock.Document.ContentEnd);
            string new_text = txtReturn.Text;

            foreach (string word in Data.all_text)
            {
                int startIndex = 0;
                while (startIndex < new_text.Length)
                {
                    int wordStartIndex = new_text.IndexOf(word);
                    if (wordStartIndex != -1)
                    {
                        var flowDocumet = TextBlock.Document;
                        TextPointer start = flowDocumet.ContentStart.GetPositionAtOffset(wordStartIndex);
                        TextPointer end = flowDocumet.ContentStart.GetPositionAtOffset(word.Length);

                        
                        TextBlock.Selection.Select(start, end);
                    }
                    else
                    {
                        break;
                    }
                    startIndex += wordStartIndex + word.Length;
                }
            }
        }
        //............................



        //Вырвавниванете..............
        TextAlignment currentAlign = TextAlignment.Left;
        private void Align_Clik(object sender, RoutedEventArgs e)
        {
            
            string name = (sender as FrameworkElement).Name;
            switch (name)
            {
                case "leftt": currentAlign = TextAlignment.Left; break;
                case "rightt": currentAlign = TextAlignment.Right; break;
                case "centerr": currentAlign = TextAlignment.Center; break;
            }
            if (doc == null) return;
            List<Block> blocks = doc.Blocks.Where(b => b.ContentStart.CompareTo(TextBlock.Selection.End) < 0 && b.ContentEnd.CompareTo(TextBlock.Selection.Start) > 0).ToList();
            if (blocks != null)
                foreach (Block b in blocks)
                    b.TextAlignment = currentAlign;
            mb_save = false;
        }
        private void textChanged(object sender, TextChangedEventArgs e)
        {
            string fontName = (string)ComboBoxFont.SelectedItem;
            double size = Convert.ToDouble(ComboBoxSize.SelectedItem) * 92.0 / 72.0;
            if (fontName != null)
            {
                TextBlock.Selection.ApplyPropertyValue(System.Windows.Controls.RichTextBox.FontSizeProperty, size);
                TextBlock.Selection.ApplyPropertyValue(System.Windows.Controls.RichTextBox.FontFamilyProperty, fontName);
                TextBlock.Focus();
            }
            mb_save = false;

            /*if (doc == null) return;
              TextBlock.CaretPosition.Paragraph.TextAlignment = currentAlign;*/
        }
        /*private void Align_left(object sender, RoutedEventArgs e)
        {
            if (leftt.IsChecked == true) // выключаем, но не можем
            {
                rightt.IsChecked = false;
                centerr.IsChecked = false;
                leftt.IsChecked = true;
                p1.TextAlignment = TextAlignment.Left;
            }
            else // включаем 
            {
                leftt.IsChecked = true;
                rightt.IsChecked = false;
                centerr.IsChecked = false;
                p1.TextAlignment = TextAlignment.Left;
            }
        }
        private void Align_Center(object sender, RoutedEventArgs e)
        {
            if (centerr.IsChecked == false) // выключаем
            {
                centerr.IsChecked = false;
                leftt.IsChecked = true;
                
                p1.TextAlignment = TextAlignment.Left;
            }
            else if (centerr.IsChecked == true) // включаем 
            {
                leftt.IsChecked = false;
                rightt.IsChecked = false;
                p1.TextAlignment = TextAlignment.Center;
                centerr.IsChecked = true;
            }
        }
        private void Align_Right(object sender, RoutedEventArgs e)
        {
            if (rightt.IsChecked == false) // выключаем
            {
                rightt.IsChecked = false;
                leftt.IsChecked = true;
                p1.TextAlignment = TextAlignment.Left;
            }
            else // включаем 
            {
                leftt.IsChecked = false;
                centerr.IsChecked = false;
                p1.TextAlignment = TextAlignment.Right;
                rightt.IsChecked = true;
            }
        }*/
        //............................



        //Маркеры.....................
        private void Markers_Click(object sender, RoutedEventArgs e)
        {
            TextMarkerStyle style = TextMarkerStyle.Square;
            switch ((sender as FrameworkElement).Name)
            {
                case "markerDigit": style = TextMarkerStyle.Decimal; break;
                case "markerDisk": style = TextMarkerStyle.Disc; break;
                case "markerRoman": style = TextMarkerStyle.UpperRoman; break;
            }

            List<Block> blocks = doc.Blocks.Where(b => b.ContentStart.CompareTo(TextBlock.Selection.End) < 0 && b.ContentEnd.CompareTo(TextBlock.Selection.Start) > 0).ToList();
            try
            {
                if (blocks != null && (blocks[0] is Paragraph par))
                {
                    Block upper = par.PreviousBlock;
                    Block next = par.NextBlock;

                    List list = new List(new ListItem(par));
                    list.MarkerStyle = style;
                    if (upper != null)
                        doc.Blocks.InsertAfter(upper, list);
                    else if (next != null)
                        doc.Blocks.InsertBefore(next, list);
                    else
                        doc.Blocks.Add(list);
                    TextBlock.CaretPosition = list.ContentEnd;
                }
            }
            catch
            {
                
            }
            mb_save = false;
        }
        //............................



        //Жирный, курсив и подяёркивание......
        private void Bold_Click(object sender, RoutedEventArgs e)
        {
            double size = (double)ComboBoxSize.SelectedItem;
            string nameFont = ComboBoxFont.SelectedItem.ToString();

            if (Boold.IsChecked == true)
            {
                TextBlock.Selection.ApplyPropertyValue(System.Windows.Controls.RichTextBox.FontSizeProperty, size);
                TextBlock.Selection.ApplyPropertyValue(System.Windows.Controls.RichTextBox.FontFamilyProperty, nameFont);
                TextBlock.Selection.ApplyPropertyValue(System.Windows.Controls.RichTextBox.FontWeightProperty, FontWeights.Bold);
                
                TextBlock.Focus();
            }
            else
            {
                TextBlock.Selection.ApplyPropertyValue(System.Windows.Controls.RichTextBox.FontSizeProperty, size);
                TextBlock.Selection.ApplyPropertyValue(System.Windows.Controls.RichTextBox.FontFamilyProperty, nameFont);
                TextBlock.Selection.ApplyPropertyValue(System.Windows.Controls.RichTextBox.FontWeightProperty, FontWeights.Normal);
                
                TextBlock.Focus();
            }
        }
        //.....................................





        //О программе...........................
        private void About_WordPad(object sender, RoutedEventArgs e)
        {
            Window3 taskWindow = new Window3();
            taskWindow.Owner = this;
            taskWindow.ShowDialog();
        }


        //.....................................
    }


}

/*            <xctk:DropDownButton Content="Цвета" Background="{x:Null}" BorderBrush="{x:Null}">
                <xctk:DropDownButton.DropDownContent>
                    <ListBox x:Name="ListBoxColor" SelectionChanged="qwqw">
                        <ListBoxItem>
                            <DockPanel>
                                <Image DockPanel.Dock="Left" Source="blackColor_img.png" Width="16" Height="16"/>
                                <Label DockPanel.Dock="Right" Content="Чёрный"></Label>
                            </DockPanel>
                        </ListBoxItem>
                        <ListBoxItem>
                            <DockPanel>
                                <Image DockPanel.Dock="Left" Source="blueColor_img.png" Width="16" Height="16"/>
                                <Label DockPanel.Dock="Right" Content="Синий"></Label>
                            </DockPanel>
                        </ListBoxItem>
                        <ListBoxItem>
                            <DockPanel>
                                <Image DockPanel.Dock="Left" Source="greenColor_img.png" Width="16" Height="16"/>
                                <Label DockPanel.Dock="Right" Content="Зелёный"></Label>
                            </DockPanel>
                        </ListBoxItem>
                        <ListBoxItem>
                            <DockPanel>
                                <Image DockPanel.Dock="Left" Source="yellowColor_img.png" Width="16" Height="16"/>
                                <Label DockPanel.Dock="Right" Content="Жёлтый"></Label>
                            </DockPanel>
                        </ListBoxItem>
                        <ListBoxItem>
                            <DockPanel>
                                <Image DockPanel.Dock="Left" Source="redColor_img.png" Width="16" Height="16"/>
                                <Label DockPanel.Dock="Right" Content="Красный"></Label>
                            </DockPanel>
                        </ListBoxItem>
                        <ListBoxItem>
                            <DockPanel>
                                <Image DockPanel.Dock="Left" Source="purpleColor_img.png" Width="16" Height="16"/>
                                <Label DockPanel.Dock="Right" Content="Фиолетовый"></Label>
                            </DockPanel>
                        </ListBoxItem>
                        <ListBoxItem>
                            <DockPanel>
                                <Image DockPanel.Dock="Left" Source="grayColor_img.png" Width="16" Height="16"/>
                                <Label DockPanel.Dock="Right" Content="Серый"></Label>
                            </DockPanel>
                        </ListBoxItem>
                        <ListBoxItem>
                            <DockPanel>
                                <Image DockPanel.Dock="Left" Source="whiteColor_img.png" Width="16" Height="16"/>
                                <Label DockPanel.Dock="Right" Content="Белый"></Label>
                            </DockPanel>
                        </ListBoxItem>
                    </ListBox>
                </xctk:DropDownButton.DropDownContent>
            </xctk:DropDownButton>*/