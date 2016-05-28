using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Bulkes
{
    class Bulk : Unit
    {
        protected bool isMoved;
        protected float mass;
        protected Indicator indicator;
        public Bulk(float _x, float _y, float _radius, Color _color) : base (_x, _y, _radius, _color)
        {
            mass = (float)Math.PI * _radius * _radius;
            isMoved = false;
            indicator = new Indicator();
            animationRadius = radius;
        }
        public Bulk(float _x, float _y, float _radius) : this(_x, _y, _radius, Settings.BulkDefaultColor)
        { }

        public float getSpeedCoefficient()
        {
            return Math.Min(Settings.BulkBaseSize / radius, 2f);
        }


        public float getSpeedX()
        {
            return speedX * getSpeedCoefficient();
        }

        public float getSpeedY()
        {
            return speedY * getSpeedCoefficient();
        }

        public bool getIsMoved()
        {
            return isMoved;
        }

        public void setIsMoved(bool flag)
        {
            isMoved = flag;
        }

        public void addMass(float feed)
        {
            setMass(feed + mass);
        }

        public float getMass()
        {
            return mass;
        }

        public void setMass(float mass)
        {
            this.mass = mass;
            radius = (float)Math.Sqrt((double)mass / Math.PI) * Settings.UserScale;
            baseRadius = radius;
          //  if (this is User) {
               // Log.v("Mass: ", String.valueOf(mass));
              //  Log.v("Radius: ", String.valueOf(radius));
          //  }
        }

        public void updatePosition(Unit unit)//update location + radius
        {
            radius = (float)Math.Sqrt((double)mass / Math.PI) * Settings.UserScale;
            //baseRadius = radius;
            x = unit.getX() + ((baseX - unit.getX()) * Settings.UserScale);
            y = unit.getY() + ((baseY - unit.getY()) * Settings.UserScale);
         //   if (this is User) {
               // Log.v("User UPD", String.valueOf(radius));
         //   }
            if (!isOnMainScreen())
                animationRadius = radius;
        }


        public float getFeed()
        {
            return mass;
        }

        public Path getIndicator(float x_end, float y_end)
        {
            float coefficient = Math.Max(radius / Settings.BulkBaseSize, 2f);
            indicator.getParameters(x, y, getAnimationRadius() + Settings.IndicatorTopOffset * coefficient, x_end, y_end);
            return indicator.getTriangle(x, y, getAnimationRadius() + (Settings.IndicatorBaseOffset * coefficient), coefficient);
        }
    }
}
