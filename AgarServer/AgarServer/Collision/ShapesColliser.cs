using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgarServer.Collision
{
    public class ShapesColliser : IShapesColliser
    {
        public StaticShape CheckCollision(Player player)
        {
            foreach (var shape in GameEngine.Instance.ShapesGenerator.GetAllShapes())
            {
                    Position playerCenter = Position.GetCircleCenter(player.Position, player.Radius);
                    Position shapeCenter = Position.GetCircleCenter(shape.Position, shape.Radius);

                    double distanceBetweenPoints = (playerCenter.Left - shapeCenter.Left) * (playerCenter.Left - shapeCenter.Left) +
                    (playerCenter.Top - shapeCenter.Top) * (playerCenter.Top - shapeCenter.Top);
                    double sumRadius = (player.Radius + shape.Radius) * (player.Radius + shape.Radius);

                    if (distanceBetweenPoints <= sumRadius)
                    {
                        return shape;
                    }                
            }

            return null;
        }
    }
}