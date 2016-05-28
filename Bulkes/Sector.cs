using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulkes
{
    class Sector
    {
        private int basePriority;
        private int finishPriority;
        private float sumFeed;
        private List<Bulk> bulkes;
        const int MAX_PRIORITY = 10000;
        const int EMPTY = 5;
        const int BIG_BULK = 10;
        public Sector()
        {
            bulkes = new List<Bulk>(Settings.CountBulkes + 1);//1 - for user
            sumFeed = 0f;
            basePriority = 0;
        }
        public void restart()
        {
            bulkes.Clear();
            sumFeed = 0f;
            basePriority = 0;
        }
        public void addFeed(float feed)
        {
            sumFeed += feed;
        }
        public void checkBulk(Bulk bulk)
        {
            bulkes.Add(bulk);
        }
        public void findPriority(Bulk current_bulk)
        {
            foreach (Bulk bulk in bulkes)
            {
                if (bulk != current_bulk)
                {
                    if (bulk.getRadius() < current_bulk.getRadius() - Settings.BulkOffsetRadius)
                        sumFeed += bulk.getFeed();
                    else
                        basePriority += BIG_BULK;
                }
            }
            basePriority += (int)Math.Min(Settings.MaxTotalFeed / sumFeed, EMPTY);
            //Log.v("Sum Feed ", String.valueOf(sumFeed) + " Priority " + String.valueOf(basePriority));
        }
        public int getPriority()
        {
            return finishPriority;
        }
        public int getBasePriority()
        {
            return basePriority;
        }
        public void updatePriority(int value)
        {
            finishPriority = basePriority + value;
        }
    }
}
