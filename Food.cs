using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Bulkes
{
    class Food : Unit
    {
        private float feed;
        
        public override float getFeed()
        {
            return feed;
        }

        public void setFeed(float feed)
        {
            this.feed = feed;
        }

        public Food(float feed)
        {
            this.feed = feed;
        }

        public Food(float _x, float _y, float _radius, float _feed) : this(_x, _y, _radius, Settings.ColorList[new Random().Next(Settings.getCountColors())], _feed)
        { }

        public Food(float _x, float _y, float _radius, Color _color, float feed) : base(_x, _y, _radius, _color)
        {
            this.feed = feed;
            if (!isOnMainScreen())
                animationRadius = radius;
        }
    }
}
