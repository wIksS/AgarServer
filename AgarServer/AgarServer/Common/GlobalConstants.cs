using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgarServer.Common
{
    public static class GlobalConstants
    {
        public static readonly int GameHeight = 1600;

        public static readonly int GameWidth= 2300;

        public static readonly int InitialRadius = 100;

        public static readonly int InitialStaticCirclesRadius = 8;

        public static readonly int InitialShapesCount = 200;

        public static readonly int UpdatesPerSecond = 15;

        public static readonly int UpdateRadius = 15;

        public static readonly int UpdateShapesRadius = 8;

        public static readonly int MovingShapesRadius = 20;

        public static readonly double InitialMovingShapeSpeed = 2600;

        public static readonly int MovingShapeGravity = 135;
    }
}