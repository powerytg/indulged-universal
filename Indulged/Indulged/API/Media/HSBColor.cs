using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Indulged.API.Media
{
    /// <summary>
    /// HSB / HSV color model and utils
    /// </summary>
    /// 
    /// <remarks> 
    /// I didn't much write this. Great thanks to the original authors and their knowledge to color spaces!
    /// 
    /// I did fixed several bugs (like setters) and made some changes to the HSB color converter, knowing that the Philips Hue uses Hue value from 
    /// 0 - 65535 where as the usual color model uses 0 - 360. 
    /// 
    /// Source: 
    /// http://www.codeproject.com/Articles/11340/Use-both-RGB-and-HSB-color-schemas-in-your-NET-app 
    /// https://github.com/qJake/SharpHue/blob/master/SharpHue/HSBColor.cs
    /// </remarks>
    public class HSBColor
    {
        private float h;
        private float s;
        private float b;

        public HSBColor(float h, float s, float b)
        {
            this.h = Math.Min(Math.Max(h, 0), 65535);
            this.s = Math.Min(Math.Max(s, 0), 255);
            this.b = Math.Min(Math.Max(b, 0), 255);
        }

        public float H
        {
            get { return h; }
            set { h = value; }
        }

        public float S
        {
            get { return s; }
            set { s = value; }
        }

        public float B
        {
            get { return b; }
            set { b = value; }
        }

        public static Color FromHSB(int h, int s, int b)
        {
            return FromHSB(new HSBColor(h, s, b));
        }

        public static Color FromHSB(HSBColor hsbColor)
        {
            float r = hsbColor.b;
            float g = hsbColor.b;
            float b = hsbColor.b;
            if (hsbColor.s != 0)
            {
                float max = hsbColor.b;
                float dif = hsbColor.b * hsbColor.s / 255f;
                float min = hsbColor.b - dif;

                float h = hsbColor.h * 360f / 65535f;

                if (h < 60f)
                {
                    r = max;
                    g = h * dif / 60f + min;
                    b = min;
                }
                else if (h < 120f)
                {
                    r = -(h - 120f) * dif / 60f + min;
                    g = max;
                    b = min;
                }
                else if (h < 180f)
                {
                    r = min;
                    g = max;
                    b = (h - 120f) * dif / 60f + min;
                }
                else if (h < 240f)
                {
                    r = min;
                    g = -(h - 240f) * dif / 60f + min;
                    b = max;
                }
                else if (h < 300f)
                {
                    r = (h - 240f) * dif / 60f + min;
                    g = min;
                    b = max;
                }
                else if (h <= 360f)
                {
                    r = max;
                    g = min;
                    b = -(h - 360f) * dif / 60 + min;
                }
                else
                {
                    r = 0;
                    g = 0;
                    b = 0;
                }
            }

            return Color.FromArgb
                (
                    (byte)0xff,
                    (byte)Math.Round(Math.Min(Math.Max(r, 0), 255)),
                    (byte)Math.Round(Math.Min(Math.Max(g, 0), 255)),
                    (byte)Math.Round(Math.Min(Math.Max(b, 0), 255))
                    );
        }

        public static HSBColor FromColor(Color color)
        {
            HSBColor ret = new HSBColor(0f, 0f, 0f);

            float r = color.R;
            float g = color.G;
            float b = color.B;

            float max = Math.Max(r, Math.Max(g, b));

            if (max <= 0)
            {
                return ret;
            }

            float min = Math.Min(r, Math.Min(g, b));
            float dif = max - min;

            if (max > min)
            {
                if (g == max)
                {
                    ret.h = (b - r) / dif * 60f + 120f;
                }
                else if (b == max)
                {
                    ret.h = (r - g) / dif * 60f + 240f;
                }
                else if (b > g)
                {
                    ret.h = (g - b) / dif * 60f + 360f;
                }
                else
                {
                    ret.h = (g - b) / dif * 60f;
                }
                if (ret.h < 0)
                {
                    ret.h = ret.h + 360f;
                }
            }
            else
            {
                ret.h = 0;
            }

            ret.h *= 65535f / 360f;
            ret.s = (dif / max) * 255f;
            ret.b = max;

            return ret;
        }

        public HSBColor Clone()
        {
            return new HSBColor(h, s, b);
        }

        public string ToRGBString()
        {
            Color color = FromHSB(this);
            return string.Format("#{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);
        }

        public bool IsEqualToColor(HSBColor other)
        {
            return ((h == other.H) && (s == other.S) && (b == other.B));
        }
    }
}
