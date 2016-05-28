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

namespace Bulkes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Page
    {
        private GamePage gamePage;

        public MainWindow()
        {
            InitializeComponent();
            double width = Width;
            double height = Height;
        }

        private void survivalButtonClick(object sender, RoutedEventArgs e)
        {
            gamePage = new GamePage();       
            this.NavigationService.Navigate(gamePage);
        }

        private void mainPageLoaded(object sender, RoutedEventArgs e)
        {
            this.ShowsNavigationUI = false;
        }
    }
}
