using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace DotNetProContest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _allText;

        public MainWindow()
        {
            InitializeComponent();

            RefreshRecentFiles();
        }

        private void RefreshRecentFiles()
        {
            miRecentFiles.Items.Clear();

            if (Properties.Settings.Default.RecentFiles.Count == 0)
            {
                miRecentFiles.Visibility = Visibility.Collapsed;
            }
            else
            {
                miRecentFiles.Visibility = Visibility.Visible;

                foreach (var filePath in Properties.Settings.Default.RecentFiles)
                {
                    string fileName = Path.GetFileName(filePath);
                    MenuItem menuItem = new MenuItem();
                    menuItem.Header = fileName;
                    menuItem.Tag = filePath;
                    menuItem.Click += (s, e) =>
                    {
                        OpenFile(menuItem.Tag.ToString());
                    };

                    miRecentFiles.Items.Add(menuItem);
                }

                miRecentFiles.Items.Add(new Separator());

                MenuItem menuItemClearList = new MenuItem();
                menuItemClearList.Header = "Clear list";
                menuItemClearList.Click += (s, e) =>
                {
                    ClearRecentFiles();
                };
                miRecentFiles.Items.Add(menuItemClearList);
            }
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text files (*.txt)|*.txt";
            if (ofd.ShowDialog() == true)
            {
                OpenFile(ofd.FileName);
            }
        }

        private void OpenFile(string filePath)
        {
            AddToRecentFiles(filePath);

            ClearRichTextBox();

            using (TextReader textReader = File.OpenText(filePath))
            {
                _allText = textReader.ReadToEnd();
            }

            ProcessInputFile();
        }

        private void AddToRecentFiles(string filePath)
        {
            string fileName = Path.GetFileName(filePath);

            if (Properties.Settings.Default.RecentFiles.Contains(filePath))
                Properties.Settings.Default.RecentFiles.Remove(filePath);
            Properties.Settings.Default.RecentFiles.Insert(0, filePath);

            while (Properties.Settings.Default.RecentFiles.Count > 10)
                Properties.Settings.Default.RecentFiles.RemoveAt(10);
            Properties.Settings.Default.Save();

            RefreshRecentFiles();
        }

        private void ClearRichTextBox()
        {
            _allText = string.Empty;
            rtb.Document.Blocks.Clear();
        }

        private void ProcessInputFile()
        {
            if (string.IsNullOrEmpty(_allText))
                return;

            rtb.Document.Blocks.Add(new Paragraph(new Run(_allText)));
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ClearRecentFiles()
        {
            Properties.Settings.Default.RecentFiles.Clear();
            Properties.Settings.Default.Save();

            RefreshRecentFiles();
        }
    }
}
