using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Bulkes
{
    class Indicator
    {
        private float x;
        private float y;
        private float alpha;//radians

        public void getParameters(float x0, float y0, float R, float x1, float y1)
        {
            float k;
            if (Math.Abs(x1 - x0) < 0.001f)
            {
                x = x0;
                if (y1 < y0)
                {
                    y = -R + y0;
                    alpha = (float)-Math.PI / 2.0f;//  -Pi/2
                }
                else
                {
                    y = R + y0;
                    alpha = (float)Math.PI / 2.0f;//  Pi/2
                }
            }
            else
            {
                k = (y1 - y0) / (x1 - x0);
                if (x1 - x0 < 0)
                    x = (float)Math.Sqrt(1.0f / (1f + k * k)) * (-R) + x0;
                else
                    x = (float)Math.Sqrt(1.0f / (1f + k * k)) * R + x0;

                y = k * x - k * x0 + y0;
                if (y1 - y0 < 0)
                    alpha = -(float)Math.Acos((x - x0) / R);
                else
                    alpha = (float)Math.Acos((x - x0) / R);
            }

        }

        public float getAlpha()
        {
            return alpha;
        }

        public float getX()
        {
            return x;
        }

        public float getY()
        {
            return y;
        }        
    }
}
