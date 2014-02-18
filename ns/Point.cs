using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ns
{
    class Point
    {
        public Point(float p1, float p2)
        {
            // TODO: Complete member initialization
            this.Latitude  = p1;
            this.Longitude = p2;
        }
        public float Latitude { get; set; }
        public float Longitude { get; set; }

        public override string ToString()
        {
            return "Latitude is " + Latitude.ToString("0.##########", CultureInfo.GetCultureInfo("en-GB")) + " and Longitude is " + Longitude.ToString("0.##########", CultureInfo.GetCultureInfo("en-GB"));
        }
    }
}
