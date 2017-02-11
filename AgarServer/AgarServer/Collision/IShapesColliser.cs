using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgarServer
{
    public interface IShapesColliser 
    {
        StaticShape CheckCollision(Player player);
    }
}