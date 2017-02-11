using AgarServer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgarServer
{
    public class StaticShapesGenerator : IShapesGenerator
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

        public void AddShape()
        {
            this.staticShapes.Add(idCounter, new StaticShape(idCounter,
                Position.GetRandomPosition(random, GlobalConstants.GameHeight, GlobalConstants.GameWidth),
                GlobalConstants.InitialStaticCirclesRadius, colorGenerator.GetColor()));
            idCounter++;
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