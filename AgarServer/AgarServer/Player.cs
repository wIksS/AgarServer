using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace AgarServer
{
    public class Player
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("position")]
        public Position Position { get; set; }

        [JsonProperty("radius")]
        public double Radius { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonIgnore]
        public string ConnectionId { get; set; }
    }
}