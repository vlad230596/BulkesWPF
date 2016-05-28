using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Bulkes
{
    class GameMap
    {
        private LinkedList<Unit> map;
        private Random random;
        private int offsetTopLeftX;
        private int offsetTopLeftY;
        private float diffSectorX;
        private float diffSectorY;
        private int delFoodCount;
        private int maxFoodCountOnMap;
        private int minFoodCountOnMap;
        private int countFoodToDraw;

        public GameMap()
        {
            random = new Random();
            map = new LinkedList<Unit>();
            maxFoodCountOnMap = Settings.CountSectorX * Settings.MapSizeX * Settings.CountSectorY * Settings.MapSizeY * Settings.MaxFoodInSector;
            minFoodCountOnMap = (int)Math.Round(maxFoodCountOnMap / 4.0);
            offsetTopLeftX = (Settings.MapSizeX % 2 == 0) ? (int)(-0.5f * Settings.ScreenWidthDefault * (Settings.MapSizeX - 1)) : (-Settings.MapSizeX / 2 * Settings.ScreenWidthDefault);
            offsetTopLeftY = (Settings.MapSizeY % 2 == 0) ? (int)(-0.5f * Settings.ScreenHeightDefault * (Settings.MapSizeY - 1)) : (-Settings.MapSizeY / 2 * Settings.ScreenHeightDefault);
            delFoodCount = 0;
        }

        public void fillFood(List<Bulk> bulkesMap)
        {
            generateSmartMap(bulkesMap);
        }

        public void checkForFoodAdd()
        {
            int iteration = 0;
            int mapSize = map.Count;
            double scaleValue = random.NextDouble();
            if (mapSize >= minFoodCountOnMap && mapSize <= maxFoodCountOnMap)
                countFoodToDraw = (int)(delFoodCount * scaleValue);
            else if (mapSize < minFoodCountOnMap)
                countFoodToDraw = delFoodCount;
            else if (mapSize > maxFoodCountOnMap)
                countFoodToDraw = 0;

            if(countFoodToDraw!=0)
            {
                foreach(Unit unit in map)
                {
                    if (iteration < countFoodToDraw)
                    {
                        if (unit.getIsDeleted())
                        {
                            unit.setIsDeleted(false);
                            delFoodCount--;
                        }
                        iteration++;
                    }
                }
            }
        }

        public void generateSmartMap(List<Bulk> bulkesMap)
        {
            Unit unit;
            float startSectorX;
            float startSectorY;
            LinkedList<Unit> sectorMap = new LinkedList<Unit>();
            diffSectorX = Settings.ScreenWidthDefault / Settings.CountSectorX;
            diffSectorY = Settings.ScreenHeightDefault / Settings.CountSectorY;
            startSectorY = offsetTopLeftY;
            while (startSectorY < (offsetTopLeftY + Settings.MapHeightP))
            {
                startSectorX = offsetTopLeftX;
                while (startSectorX < (offsetTopLeftX + Settings.MapWidthP))
                {
                    int foodInGroup = random.Next(Settings.MaxFoodInSector - Settings.MinFoodInSector) + Settings.MinFoodInSector;
                    sectorMap.Clear();
                    for (int i = 0; i < foodInGroup; i++)
                    {
                        float radius = getRandomRadius();
                        unit = new Food(
                            getRandomX((int)startSectorX, (int)diffSectorX, radius),
                            getRandomY((int)startSectorY, (int)diffSectorY, radius),
                            radius,
                            getColor(),
                            Settings.FoodFeedForRadius * radius);
                        bool flagCorrect;
                        do
                        {
                            flagCorrect = true;
                            foreach (Unit temp in sectorMap)
                            {
                                if (temp.isOverlapped(unit))
                                {
                                    radius = getRandomRadius();
                                    unit.setX(getRandomX((int)startSectorX, (int)diffSectorX, radius));
                                    unit.setY(getRandomY((int)startSectorY, (int)diffSectorY, radius));
                                    unit.setRadius(radius);
                                    flagCorrect = false;
                                }
                            }
                        } while (flagCorrect == false);
                        sectorMap.AddLast(unit);
                    }
                    foreach (Unit food in sectorMap)//loop for adding
                    {
                        bool canAdd = true;
                        foreach (Bulk bulk in bulkesMap)
                        if (bulk.isOverlapped(food))
                        {
                            canAdd = false;
                            break;
                        }
                        if (canAdd)
                            map.AddLast(food);
                    }
                    startSectorX += diffSectorX;
                }
                startSectorY += diffSectorY;
            }
        }

        private Color getColor()
        {
            return Settings.ColorList[random.Next(Settings.getCountColors())];
        }
        private float getRandomX(int startSectorX, int diffSectorX, float radius)
        {
            return startSectorX + radius + random.Next(diffSectorX - (int)radius * 2);//2 for right side
        }
        private float getRandomY(int startSectorY, int diffSectorY, float radius)
        {
            return startSectorY + radius + random.Next(diffSectorY - (int)radius * 2);//2 for bottom side
        }
        private float getRandomRadius()
        {
            return (float)random.Next(Settings.MaxFoodSize - Settings.MinFoodSize) + Settings.MinFoodSize;
        }

        public void incDeletedFood()
        {
            delFoodCount++;
        }

        public LinkedList<Unit> getMap()
        {
            return map;
        }
        public void addUnit(Unit unit)//update check here
        {
            map.AddLast(unit);
        }

        public int getOffsetTopLeftX()
        { return offsetTopLeftX; }
        public int getOffsetTopLeftY()
        { return offsetTopLeftY; }

        public Unit getAnyUnit()
        {
            if (map.Count!=0)
               return map.First();
           return null;
        }
    }
}
