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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bulkes
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {

        private int lastSelectedColorIndex;
        private double zoomUp;
        private double zoomDown;

        public SettingsPage()
        {
            InitializeComponent();
            zoomUp = 1.3;
            zoomDown = 1.0;
        }

        private void exitSettingButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (checkIsBlackBackground.IsChecked == true)
                Settings.GameFieldColor = Colors.Black;
            else
                Settings.GameFieldColor = Colors.White;
            Settings.UserDefaultColorIndex = lastSelectedColorIndex;
            this.NavigationService.GoBack();
        }

        private void settingsPageLoaded(object sender, RoutedEventArgs e)
        {
            createAndAddColorsList();
            if (Settings.GameFieldColor == Colors.Black)
                checkIsBlackBackground.IsChecked = true;
        }

        private void createAndAddColorsList()
        {
            int currentColorIndex = Settings.UserDefaultColorIndex;
            lastSelectedColorIndex = currentColorIndex;
            for (int i = 0; i < Settings.UsersBulkColors.Length; i++)
            {
                Border colorsItem = new Border();
                colorsItem.Height = 50;
                colorsItem.Width = 50;
                colorsItem.Margin = new Thickness(5);
                colorsItem.CornerRadius = new CornerRadius(50);
                colorsItem.Background = new SolidColorBrush(Settings.UsersBulkColors[i]);
                Image selectedIcon = new Image()
                {
                    Source = new BitmapImage(new Uri("SettingsSelectColor.png", UriKind.Relative)),
                    Height = 40.0,
                    Width = 40.0,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                if(i != currentColorIndex)
                {
                    selectedIcon.Visibility = Visibility.Hidden;
                }
                colorsItem.Child = selectedIcon;
                colorsItem.MouseLeftButtonDown += colorItemMouseClick;
                colorsItem.MouseEnter += colorItemMouseOver;
                colorsItem.MouseLeave += colorItemMouseLeave;
                colorListPanel.Children.Add(colorsItem);
            }
        }

        private void colorItemMouseOver(object sender, RoutedEventArgs e)
        {
            Border colorItem = sender as Border;
            animationInitialisation(colorItem, zoomUp, zoomDown, 400);
        }

        private void colorItemMouseLeave(object sender, RoutedEventArgs e)
        {
            Border colorItem = sender as Border;
            animationInitialisation(colorItem, zoomDown, zoomUp, 500);
        }

        private void animationInitialisation(Border colorItem, double newZoomValue, double lastZoomValue, int durationInMillis)
        {
            Storyboard sbZoomUp = new Storyboard();
            ScaleTransform scale = new ScaleTransform(lastZoomValue, lastZoomValue);
            colorItem.RenderTransformOrigin = new Point(0.5, 0.5);
            colorItem.RenderTransform = scale;
            DoubleAnimation animationX = new DoubleAnimation();
            animationX.To = newZoomValue;
            animationX.Duration = new Duration(TimeSpan.FromMilliseconds(durationInMillis));
            DoubleAnimation animationY = new DoubleAnimation();
            animationY.To = newZoomValue;
            animationY.Duration = new Duration(TimeSpan.FromMilliseconds(durationInMillis));
            sbZoomUp.Children.Add(animationX);
            sbZoomUp.Children.Add(animationY);
            Storyboard.SetTargetProperty(animationX, new PropertyPath("RenderTransform.ScaleX"));
            Storyboard.SetTargetProperty(animationY, new PropertyPath("RenderTransform.ScaleY"));
            Storyboard.SetTarget(animationX, colorItem);
            Storyboard.SetTarget(animationY, colorItem);
            sbZoomUp.Begin();
        }

        private void colorItemMouseClick(object sender, RoutedEventArgs e)
        {
            Border colorItem = sender as Border;
            Image selector = colorItem.Child as Image;
            int clickedItemIndex = colorListPanel.Children.IndexOf(colorItem);
            if (clickedItemIndex != lastSelectedColorIndex)
            {
                selector.Visibility = Visibility.Visible;
                Image lastItemSelector = (colorListPanel.Children[lastSelectedColorIndex] as Border).Child as Image;
                lastItemSelector.Visibility = Visibility.Hidden;
                lastSelectedColorIndex = clickedItemIndex;
            }
        }
    }
}
