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
        private SettingsPage settingsPage;
        Window parentWindow;

        public MainWindow()
        {
            InitializeComponent();

        }

        private void survivalButtonClick(object sender, RoutedEventArgs e)
        {
            gamePage = new GamePage();       
            this.NavigationService.Navigate(gamePage);
        }

        private void mainPageLoaded(object sender, RoutedEventArgs e)
        {
            this.ShowsNavigationUI = false;
            parentWindow = Window.GetWindow(this);
            parentWindow.ResizeMode = ResizeMode.NoResize;
            parentWindow.WindowStyle = WindowStyle.SingleBorderWindow;
        }


        private void mainMenuSettingsButtonUp(object sender, MouseButtonEventArgs e)
        {
            settingsPage = new SettingsPage();
            this.NavigationService.Navigate(settingsPage);
        }

        private void mainMenuAboutButtonUp(object sender, MouseButtonEventArgs e)
        {
            AboutDialog aboutDialog = new AboutDialog();
            aboutDialog.Owner = Application.Current.MainWindow;
            aboutDialog.ShowDialog();
            // MessageBox.Show("Разработали студенты группы СПн-13\nМохначёв Владислав\nЕлагин Игорь\nВерсия игры 1.0", "Об игре", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
