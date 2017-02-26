using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgarServer
{
    public interface IShapesGenerator<T> 
    {
        void GenerateShapes();

        T AddShape();

        void RemoveShape(int id);

        IEnumerable<T> GetAllShapes();
    }
}