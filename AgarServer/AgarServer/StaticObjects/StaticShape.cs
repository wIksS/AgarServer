using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgarServer
{
    public class StaticShape
    {
        public StaticShape(int id, Position position, int radius,string color)
        {
            this.Id = id;
            this.Position = position;
            this.Radius = radius;
            this.Color = color;
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("position")]
        public Position Position { get; set; }

        [JsonProperty("radius")]
        public double Radius { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }
    }
}