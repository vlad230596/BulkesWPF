using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Bulkes
{
    class GameManager
    {
        private DispatcherTimer dispatcherTimer;
        private Canvas canvas;

        private Point mousePositionPoint;

        private float actualPageWidth;
        private float actualPageHeight;

        private bool runFlag;


        Ellipse user;
        float usersX;
        float usersY;
        float usersRadius;

        float dX;
        float dY;

        Dispatcher dispatcher;


        public GameManager(Canvas canvas, float actualPageWidth, float actualPageHeight)
        {
            this.canvas = canvas;
            this.actualPageWidth = actualPageWidth;
            this.actualPageHeight = actualPageHeight;
            mousePositionPoint = new Point();
            dispatcherTimer = new DispatcherTimer();

            dispatcher = Dispatcher.CurrentDispatcher;

            runFlag = true;
            dX = 0;
            dY = 0;

            user = new Ellipse();

            usersRadius = 100;
            float centerX = actualPageWidth / 2;
            float centerY = actualPageHeight / 2;
            usersX = centerX - usersRadius / 2;
            usersY = centerY - usersRadius / 2;


            user.Fill = new SolidColorBrush() { Color = Color.FromArgb(255, 255, 255, 0) };
            user.StrokeThickness = 2;
            user.Stroke = Brushes.Black;
            user.Width = usersRadius;
            user.Height = usersRadius;

            user.SetValue(Canvas.LeftProperty, (double)usersX);
            user.SetValue(Canvas.TopProperty, (double)usersY);
            canvas.Children.Add(user);

            startGame();
        }



        private void timerTick(object sender, EventArgs e)
        {
            usersX += dX;
            usersY += dY;
            user.SetValue(Canvas.LeftProperty, (double)usersX);
            user.SetValue(Canvas.TopProperty, (double)usersY);
        }

        public void startGame()
        {
           // new Thread(startCalculatingProcess).Start();
            dispatcherTimer.Tick += new EventHandler(timerTick);
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(4);
            dispatcherTimer.Start();
            
        }

        public void stopGame()
        {
            dispatcherTimer.Stop();
            runFlag = false;
        }
        /*
               private async void startCalculatingProcess()
               {
                   while(runFlag)
                   {
                       usersX += dX;
                       usersY += dY;
                     //  await drawField();
                   }
               }

               private async Task drawField()
               {
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                   {
                       user.SetValue(Canvas.LeftProperty, (double)usersX);
                       user.SetValue(Canvas.TopProperty, (double)usersY);
                   }));
               }
               */

        private void calculateMovementOffset()
        {
            float k;
            if (Math.Sqrt(Math.Pow((mousePositionPoint.X - usersX), 2) + Math.Pow((mousePositionPoint.Y - usersY), 2)) > usersRadius)
            {
                if ((mousePositionPoint.X - usersX) != 0)
                {
                    k = ((float)mousePositionPoint.Y - usersY) / ((float)mousePositionPoint.X - usersX);
                    dX = (float)Math.Sqrt(Math.Pow(usersRadius, 2) / (Math.Pow(k, 2) + 1f));
                    dY = (float)Math.Sqrt((Math.Pow(usersRadius, 2) * Math.Pow(k, 2)) / (1f + Math.Pow(k, 2)));
                }
                else
                {
                    dX = 0;
                    dY = usersRadius;
                }
                dX = mousePositionPoint.X < usersX ? (-dX) : (dX);
                dY = (mousePositionPoint.Y < usersY) ? (-dY) : (dY);
            }
            else
            {
                dX = (float)mousePositionPoint.X - usersX;
                dY = (float)mousePositionPoint.Y - usersY;
            }
            dX *= 0.005f;
            dY *= 0.005f;
        }


        public void setWindowSize(float actualWidth, float actualHeight)
        {
            actualPageWidth = actualWidth;
            actualPageHeight = actualHeight;
        }


        public void setMousePositionPoint(Point mousePositionPoint)
        {
            this.mousePositionPoint.X = mousePositionPoint.X;
            this.mousePositionPoint.Y = mousePositionPoint.Y;
            calculateMovementOffset();
        }

    }
}
