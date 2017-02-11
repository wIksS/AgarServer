using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgarServer
{
    public struct Position
    {
        public Position(double top, double left)
        {
            this.Top = top;
            this.Left = left;
        }

        [JsonProperty("top")]
        public double Top { get; set; }

        [JsonProperty("left")]
        public double Left { get; set; }

        public static Position GetCircleCenter(Position circle, double radius)
        {
            return new Position(circle.Top + radius, circle.Left + radius);
        }

        public static Position GetRandomPosition(Random random, int height, int width)
        {
            return new Position(random.Next(0, height),
                random.Next(0, width));
        }
    }
}