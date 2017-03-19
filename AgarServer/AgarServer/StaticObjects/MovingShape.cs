using AgarServer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgarServer.StaticObjects
{
    public class MovingShape : StaticShape
    {
        public MovingShape(int id, Position position, int radius, string color)
            : base(id, position, radius, color)
        {
        }

        public Velocity Velocity{ get; set; }
    }
}