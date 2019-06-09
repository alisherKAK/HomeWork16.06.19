using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace HomeWork16_06_19
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> fonts = new List<string>();
        private Timer timer;
        private bool isSaved = false;
        private string savedFilePath = string.Empty;

        public MainWindow()
        {
            InitializeComponent();

            foreach (System.Drawing.FontFamily font in System.Drawing.FontFamily.Families)
            {
                fonts.Add(font.Name);
            }
            fonts.Remove(string.Empty);

            fontFamilyComboBox.ItemsSource = fonts;

            timer = new Timer(IntermediateSaveFile, null, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(10));
        }

        private void FontSizeTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void FontSizeTextBoxKeyUp(object sender, KeyEventArgs e)
        {
            if(!string.IsNullOrEmpty(fontSizeTextBox.Text))
                mainRichTextBox.FontSize = double.Parse(fontSizeTextBox.Text);
        }

        private void BoldFormatCheckboxSelected(object sender, RoutedEventArgs e)
        {
            mainRichTextBox.FontWeight = FontWeights.Bold;
        }

        private void ItalicFormatCheckboxSelected(object sender, RoutedEventArgs e)
        {
            mainRichTextBox.FontStyle = FontStyles.Italic;
        }

        private void UnderlineCheckboxSelected(object sender, RoutedEventArgs e)
        {
            richTextBoxsRun.TextDecorations = TextDecorations.Underline;
        }

        private void BoldFormatCheckboxUnselected(object sender, RoutedEventArgs e)
        {
            mainRichTextBox.FontWeight = FontWeights.Normal;
        }

        private void ItalicFormatCheckboxUnselected(object sender, RoutedEventArgs e)
        {
            mainRichTextBox.FontStyle = FontStyles.Normal;
        }

        private void UnderlineCheckboxUnselected(object sender, RoutedEventArgs e)
        {
            richTextBoxsRun.TextDecorations = null;
        }

        private void FontFamilyComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mainRichTextBox.FontFamily = new FontFamilyConverter().ConvertFromString(fonts[fontFamilyComboBox.SelectedIndex]) as FontFamily;
        }

        private void OpenButtonClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "txt files (*.txt)|*.txt";
            if(openFileDialog.ShowDialog() == true)
            {
                using(var streamReader = new StreamReader(openFileDialog.FileName))
                {
                    richTextBoxsRun.Text = streamReader.ReadToEnd();
                }
            }
        }

        private void SaveAsButtonClick(object sender, RoutedEventArgs e)
        {
            SaveTextToFile();
        }

        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            if (isSaved)
            {
                this.Close();
            }
            else
            {
                SaveTextToFile();
            }
        }

        private void IntermediateSaveFile(object state)
        {
            string filePath = $@"C:\Users\{Environment.UserName}\tmp";

            using(var stream = File.Open(filePath, FileMode.Create))
            {
                byte[] dataBytes = Encoding.UTF8.GetBytes(new TextRange(mainRichTextBox.Document.ContentStart, mainRichTextBox.Document.ContentEnd).Text);
                stream.Write(dataBytes, 0, dataBytes.Length);
            }
            //using(var streamWriter = new StreamWriter(filePath))
            //{
            //    streamWriter.Write(richTextBoxsRun.Text);
            //}
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            if(isSaved)
            {
                using (var streamWriter = File.CreateText(savedFilePath + ".txt"))
                {
                    streamWriter.Write(richTextBoxsRun.Text);
                }
            }
            else
            {
                SaveTextToFile();
            }
        }

        private void SaveTextToFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                using (var streamWriter = File.CreateText(saveFileDialog.FileName + ".txt"))
                {
                    streamWriter.Write(richTextBoxsRun.Text);
                }

                savedFilePath = saveFileDialog.FileName;
                isSaved = true;
            }
        }
    }
}
