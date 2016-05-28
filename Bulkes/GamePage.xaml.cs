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
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        private GameManager gameManager;
        public GamePage()
        {
            InitializeComponent();
        }

        private void gamePageLoaded(object sender, RoutedEventArgs e)
        {
            gameManager = new GameManager(canvas, (float)this.ActualWidth, (float)this.ActualHeight);


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


        private void gamePageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(gameManager != null)
                gameManager.setWindowSize((float)e.NewSize.Width, (float)e.NewSize.Height);
        }



        private void canvasMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                gameManager.setMousePositionPoint(e.GetPosition(this));
        }

        private void canvasMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                gameManager.setMousePositionPoint(e.GetPosition(this));
        }
    }
}
