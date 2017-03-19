using AgarServer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgarServer
{
    public class StaticShapesGenerator : IShapesGenerator<StaticShape>
    {
        private Dictionary<int, StaticShape> staticShapes;
        private int idCounter = 0;
        private Random random;
        private IColorGenerator colorGenerator;

        public StaticShapesGenerator(IColorGenerator colorGenerator)
        {
            this.colorGenerator = colorGenerator;
            this.random = new Random();
            this.staticShapes = new Dictionary<int, StaticShape>();
        }

        public void GenerateShapes()
        {
            for (int i = 0; i < GlobalConstants.InitialShapesCount; i++)
            {
                this.AddShape();
            }
        }

        public StaticShape AddShape()
        {
            var shape = new StaticShape(idCounter,
                Position.GetRandomPosition(random, GlobalConstants.GameHeight, GlobalConstants.GameWidth),
                GlobalConstants.InitialStaticCirclesRadius, colorGenerator.GetColor());

            this.staticShapes.Add(idCounter, shape);
            idCounter++;
            return shape;
        }

        public StaticShape AddShape(StaticShape shape)
        {
            shape.Id = idCounter;

            this.staticShapes.Add(idCounter, shape);
            idCounter++;
            return shape;
        }

        public void RemoveShape(int id)
        {
            if (this.staticShapes.ContainsKey(id))
            {
                this.staticShapes.Remove(id);
            }
        }

        public IEnumerable<StaticShape> GetAllShapes()
        {
            return this.staticShapes.Values.ToList();
        }
    }
}