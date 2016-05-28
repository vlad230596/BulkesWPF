using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Bulkes
{
    class User : Bulk
    {
        public User(float _x, float _y, float _radius) : this(_x, _y, _radius, Settings.UserDefaultColor)
        { }

        public User(float _x, float _y, float _radius, Color _color) : base(_x, _y, _radius, _color)
        {
            speedX = 0;//user is always in center
            speedY = 0;
        }
    }
}
