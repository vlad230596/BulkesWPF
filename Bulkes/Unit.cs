﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Bulkes
{
    class Unit
    {
        protected float x;
        protected float y;
        protected float baseX;
        protected float baseY;
        protected float baseRadius;
        protected float radius;
        protected float animationRadius;
        protected bool isDeleted;
        protected Color color;
        protected Unit target;

        public float getSpeedX()
        {
            return speedX;
        }

        public float getSpeedY()
        {
            return speedY;
        }

        protected float speedX;
        protected float speedY;
        protected float step;

        //constructors
        public Unit():this(0f, 0f, 0f)
        { }
        public Unit(float _x, float _y, float _radius) : this(_x, _y, _radius, Colors.Gray)
        { }
        public Unit(float _x, float _y, float _radius, Color _color)
        {
            x = _x;
            y = _y;
            baseX = x;
            baseY = y;
            radius = _radius * Settings.UserScale;
            baseRadius = _radius;
            color = _color;
            isDeleted = false;
            animationRadius = 0f;
        }

        //setters
        public void setX(float x)
        {
            this.x = x;
            baseX = x;
        }
        public void setY(float y)
        {
            this.y = y;
            baseY = y;
        }
        public void setRadius(float radius)
        {
            this.baseRadius = radius;
            this.radius = radius * Settings.UserScale;
        }
        //overrided in Bulk
        public void updatePosition(Unit unit)//update location + radius
        {
            radius = baseRadius * Settings.UserScale;
            // x = unit.x + ((baseX - unit.x) * Settings.UserScale);
            // y = unit.y + ((baseY - unit.y) * Settings.UserScale);

            if (!isOnMainScreen())
                animationRadius = radius;
        }
        public void setSpeed(float _speedX, float _speedY)
        {
            speedX = _speedX;
            speedY = _speedY;
        }
        public void setColor(Color color)
        {
            this.color = color;
        }
        public void setPosition(float _x, float _y)
        {
            x = _x;
            y = _y;
            baseX = x;//update here
            baseY = y;
        }
        public void setIsDeleted(bool isDeleted, Unit whatBulk)
        {
            target = whatBulk;
            this.isDeleted = isDeleted;
        }
        public void setAnimationRadius(float animationRadius)
        {
            this.animationRadius = animationRadius;
        }

        //getters
        public float getX()
        {
            return x;
        }
        public float getY()
        {
            return y;
        }
        public float getRadius()
        {
            return radius;
        }
        public float getAnimationRadius()
        {
            if (animationRadius < radius)
            {
                animationRadius += Settings.StepRadius * radius;
                if (animationRadius <= radius)
                    return animationRadius;
                else
                    animationRadius = radius;
            }
            else
            if (animationRadius > radius)
            {
                animationRadius -= Settings.StepRadius * radius;
                if (animationRadius >= radius)
                    return animationRadius;
                else
                    animationRadius = radius;
            }
            return radius;
        }
        public float getFeed()//this method must override in all class
        {
            return 0f;
        }
        public bool getIsDeleted()
        {
            return isDeleted;
        }
        public Color getColor()
        {
            return color;
        }

        //public methods
        public void move(float dx, float dy)
        {
            x += dx;
            y += dy;
            baseX += dx;
            baseY += dy;
        }
        public bool isOverlapped(Unit unit)
        {
            //if( unit.radius > radius )
            //  return unit.isOverlapped(this);
            if (unit.getX() < x - (radius + unit.radius)
                    || unit.getX() > x + (radius + unit.radius)
                    || unit.getY() < y - (radius + unit.radius)
                    || unit.getY() > y + (radius + unit.radius))
                return false;

            Indicator pointOut = new Indicator();//point on radius external circle
            pointOut.getParameters(unit.getX(), unit.getY(), unit.getRadius(), x, y);//(x;y) - center current unit
            float dx;
            float dy;
            dx = pointOut.getX() - x;
            dy = pointOut.getY() - y;
            return (dx * dx + dy * dy) < (radius * radius);
        }
        public bool isEaten(Unit unit)
        {
            if (unit.radius > radius)
                return unit.isEaten(this);
            float dx;
            float dy;
            dx = unit.getX() - x;
            dy = unit.getY() - y;
            return (dx * dx + dy * dy) < (radius * radius);
        }

        //protected
        protected bool isOnMainScreen()
        {
            if (x + radius < 0f || x - radius > Settings.ScreenWidthDefault)
                return false;
            if (y + radius < 0f || y - radius > Settings.ScreenHeightDefault)
                return false;
            return true;
        }
        protected bool catchTarget()
        {
            if (isOnMainScreen())
            {
                speedX = Math.Max(Settings.MinFoodSpeed, target.getSpeedX() * Settings.UnitToTargetCoefficient);
                speedY = Math.Max(Settings.MinFoodSpeed, target.getSpeedY() * Settings.UnitToTargetCoefficient);
               // Log.v("Alarm", String.valueOf(speedX) + " " + String.valueOf(speedY));
                return moveToTarget();
            }
            else
                return true;
        }
        protected bool moveToTarget()
        {
            float dx;
            float dy;
            float newX;
            float newY;
            dx = target.getX() - x;
            dy = target.getY() - y;
            if (dx * dx + dy * dy < (target.getRadius() - radius) * (target.getRadius() - radius))
                return true;
            //catchTarget();
            if (Math.Abs(dx) > Math.Abs(dy))
            {
                if (dx > 0)
                    newX = x + getSpeedX();
                else
                    newX = x - getSpeedX();
                setPosition(newX, solveY(newX));
            }
            else
            {
                if (dy > 0)
                    newY = y + getSpeedY();
                else
                    newY = y - getSpeedY();
                setPosition(solveX(newY), newY);
            }
            return false;
        }

        //private
        private float solveY(float _x)//x  - increment; y = f(x)
        {
            float k;
            k = (target.getY() - getY()) / (target.getX() - getX());
            return k * _x - k * getX() + getY();
        }
        private float solveX(float _y)//y  - increment; x = f(y)
        {
            float k;
            if (Math.Abs(target.getX() - getX()) < 0.001f)
                return getX();
            else
            {
                k = (target.getY() - getY()) / (target.getX() - getX());
                return (_y - getY()) / k + getX();
            }
        }
    }
}
