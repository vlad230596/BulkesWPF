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
using System.Windows.Media.Animation;

namespace Bulkes
{
    /// <summary>
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        private GameManager gameManager;
        public GamePage()
        {
            InitializeComponent();
            mainBulk.Width = Settings.UserStartSize * 2;
            mainBulk.Height = Settings.UserStartSize * 2;
            mainBulk.Fill = new SolidColorBrush(Settings.UsersBulkColors[Settings.UserDefaultColorIndex]);            
        }

        private void gamePageLoaded(object sender, RoutedEventArgs e)
        {
            gameManager = new GameManager(canvas, Container, mainBulk, timeUI, pointUI, (float)this.ActualWidth, (float)this.ActualHeight);
            Window.GetWindow(this).WindowStyle = WindowStyle.None;
            this.PreviewKeyDown += handleKeyPress;
            this.Focusable = true;
            this.Focus();
        }

        private void handleKeyPress(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                EndGameDialog endGameDialog = new EndGameDialog();
                endGameDialog.Owner = Application.Current.MainWindow;
                if (endGameDialog.ShowDialog() == true)
                {
                    gameManager.stopGame();
                    this.NavigationService.GoBack();
                }
            }
         }



        private void canvasMouseDown(object sender, MouseButtonEventArgs e)
        {
            gameManager.setMousePositionPoint(e.GetPosition(this));
        }

    }
}
