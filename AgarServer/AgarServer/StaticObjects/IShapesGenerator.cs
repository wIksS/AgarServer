using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgarServer
{
    public interface IShapesGenerator 
    {
        void GenerateShapes();

        void AddShape();

        void RemoveShape(int id);

        IEnumerable<StaticShape> GetAllShapes();
    }
}