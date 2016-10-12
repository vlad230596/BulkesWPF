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
using System.Windows.Media.Animation;

namespace Bulkes
{
    class GameManager
    {
        private DispatcherTimer dispatcherTimer;
        private Canvas canvas;
        private Ellipse mainBulk;
        private User user;
        private List<Bulk> bulkesMap;
        private LinkedList<Unit> unitMap;
        SectorHolder sectors;
        GameMap gameMap;
        private Point mousePositionPoint;
        private float actualPageWidth;
        private float actualPageHeight;
        float usersX;
        float usersY;
        float usersRadius;
        float dX;
        float dY;
        DateTime start;
        TextBlock timeUI;
        TextBlock pointUI;
        public GameManager(Canvas canvas, Canvas Container, Ellipse mainBulk, TextBlock timeUI, TextBlock pointUI, float actualPageWidth, float actualPageHeight)
        {
            this.canvas = canvas;
            this.mainBulk = mainBulk;
            this.actualPageWidth = actualPageWidth;
            this.actualPageHeight = actualPageHeight;
            this.timeUI = timeUI;
            this.pointUI = pointUI;
            mousePositionPoint = new Point();
            dispatcherTimer = new DispatcherTimer();
            start = DateTime.Now;

            Container.Background = new SolidColorBrush(Settings.GameFieldColor);
            dX = 0;
            dY = 0;
            usersRadius = Settings.UserStartSize;
            float centerX = actualPageWidth / 2;
            float centerY = actualPageHeight / 2;
            usersX = centerX - usersRadius / 2;
            usersY = centerY - usersRadius / 2;

            user = new User(centerX, centerY, Settings.UserStartSize);
            bulkesMap = new List<Bulk>();
            bulkesMap.Add(user);
            bulkesMap.Add(new Enemy(0, 200, 95));
            bulkesMap.Add(new Enemy(500, 0, 80));

            gameMap = new GameMap();
            gameMap.generateSmartMap(bulkesMap);
            unitMap = gameMap.getMap();
            gameMap.addUnit(user);
            gameMap.addUnit(bulkesMap[1]);
            gameMap.addUnit(bulkesMap[2]);

            sectors = new SectorHolder();
            sectors.setOffsets(-gameMap.getOffsetTopLeftX(), -gameMap.getOffsetTopLeftY());           
            startGame();
        }



        private void timerTick(object sender, EventArgs e)
        {
            draw();
        }
        public void draw()
        {
            canvas.Children.Clear();
            drawMap();
            timeUI.Text = (DateTime.Now - start).ToString().Substring(0, 8);
            pointUI.Text = ((uint)user.getMass()).ToString();
            
            foreach (Bulk bulk in bulkesMap) {
                if (!bulk.getIsDeleted() && bulk is Enemy)
                {
                    //drawBulk(bulk,canvas);
                    if(!((Enemy) bulk).getIsDeleted())
                        ((Enemy) bulk).updateState(gameMap, sectors);
                }
            }

        }
        private void drawMap()
        {
            sectors.restartChecking();
            int leftBorder = gameMap.getOffsetTopLeftX() + 1;
            int rightBorder = gameMap.getOffsetTopLeftX() + Settings.MapWidthP - 1;
            int upBorder = gameMap.getOffsetTopLeftY() + 1;
            int downBorder = gameMap.getOffsetTopLeftY() + Settings.MapHeightP - 1;
            foreach (Unit point in unitMap)
            {
                if (point.getIsDeleted())
                    continue;
                sectors.checkUnit(point);
                foreach (Bulk bulk in bulkesMap)
                {
                    if (point != bulk && !bulk.getIsDeleted() && bulk.isEaten(point))
                    {
                        if (bulk.getRadius() > point.getRadius())
                        {
                            bulk.addMass(point.getFeed());
                            if (point is User)
                            {
                                stopGame();
                                MessageBox.Show("End Game");                                
                            }
                            point.setIsDeleted(true);
                            if (bulk is Enemy && ((Enemy)bulk).isTarget(point))
                                ((Enemy)bulk).setTarget(null);
                        }
                        else
                            if (bulk is User)
                            {
                                user.setIsMoved(false);
                                bulk.setIsMoved(false);
                                stopGame();
                                MessageBox.Show("End Game");                                
                            }
                    }
                }
                if (!point.getIsDeleted() && !(point is User))
                {      
                    point.move(dX / 16, dY / 16);
                    if (point.getX() >= rightBorder)
                        point.setX(point.getX() - Settings.MapWidthP + 2);
                    else if (point.getX() <= leftBorder)
                        point.setX(point.getX() + Settings.MapWidthP - 2);
                    if (point.getY() >= downBorder)
                        point.setY(point.getY() - Settings.MapHeightP + 2);
                    else if (point.getY() <= upBorder)
                        point.setY(point.getY() + Settings.MapHeightP - 2);
                    if (point.isOnMainScreen())
                    {
                        Ellipse figure = new Ellipse();
                        figure.Width = point.getRadius() * 2;
                        figure.Height = point.getRadius() * 2;
                        figure.Fill = new SolidColorBrush(point.getColor());
                        figure.SetValue(Canvas.LeftProperty, (double)point.getX() - point.getRadius() / 2);
                        figure.SetValue(Canvas.TopProperty, (double)point.getY()  - point.getRadius() / 2);
                        canvas.Children.Add(figure);
                    }
                }
            }
            DoubleAnimation anim = new DoubleAnimation();
            anim.To = user.getRadius() * 2;
            anim.Duration = new Duration(TimeSpan.FromMilliseconds(2000));
            Storyboard sb = new Storyboard();
            sb.Duration = anim.Duration;

            DoubleAnimation anim2 = new DoubleAnimation();
            anim2.To = user.getRadius() * 2;
            anim2.Duration = new Duration(TimeSpan.FromMilliseconds(2000));
            sb.Children.Add(anim);
            sb.Children.Add(anim2);

            Storyboard.SetTarget(anim, mainBulk);
            Storyboard.SetTarget(anim2, mainBulk);
            Storyboard.SetTargetProperty(anim, new PropertyPath("(Width)"));
            Storyboard.SetTargetProperty(anim2, new PropertyPath("(Height)"));
            sb.Begin();
           // mainBulk.Width = user.getRadius() * 2;
            //mainBulk.Height = user.getRadius() * 2;
            if (mainBulk.Width >= Settings.ScreenWidthDefault || mainBulk.Height >= Settings.ScreenHeightDefault)
            {
                stopGame();
                MessageBox.Show("You win");
                
            }
        }

        public void startGame()
        {
            dispatcherTimer.Tick += new EventHandler(timerTick);
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(Settings.TimerInterval);
            dispatcherTimer.Start();
            
        }

        public void stopGame()
        {
            dispatcherTimer.Stop();
        }

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
                dX = mousePositionPoint.X < usersX ? (dX) : (-dX);
                dY = (mousePositionPoint.Y < usersY) ? (dY) : (-dY);
            }
            else
            {
                dX = (float)mousePositionPoint.X - usersX;
                dY = (float)mousePositionPoint.Y - usersY;
            }
        }
        public void setMousePositionPoint(Point mousePositionPoint)
        {
            this.mousePositionPoint.X = mousePositionPoint.X;
            this.mousePositionPoint.Y = mousePositionPoint.Y;
            calculateMovementOffset();
        }

    }
}
