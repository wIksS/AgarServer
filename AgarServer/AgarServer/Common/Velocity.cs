using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgarServer.Common
{
    public class Velocity
    {
        public Velocity(Position direction)
        {
            this.Speed = GlobalConstants.InitialMovingShapeSpeed;
            this.Direction = direction;
        }

        public double Speed { get; set; }

        public Position Direction { get; set; }
    }
}