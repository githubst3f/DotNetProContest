using System.Windows;

namespace DotNetProContest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModelMainWindow _viewModelMainWindow;
        public ViewModelMainWindow ViewModelMainWindow
        {
            get
            {
                return _viewModelMainWindow ?? 
                    (_viewModelMainWindow = new ViewModelMainWindow());
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
