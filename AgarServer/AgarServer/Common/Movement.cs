using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgarServer
{
    public class Movement
    {
        private const int moveCount = 1;

         public static Position GetNewPosition(Position playerPosition, Position inputPosition)
        {
            if (playerPosition.Top == inputPosition.Top && playerPosition.Left == inputPosition.Left)
            {
                return inputPosition;
            }

            double speed = 700;
            double elapsed = 0.01f;

            double distance = Math.Sqrt(
                (inputPosition.Left - playerPosition.Left) * (inputPosition.Left - playerPosition.Left) +
                (inputPosition.Top - playerPosition.Top) * (inputPosition.Top - playerPosition.Top));
            double directionX = (inputPosition.Left - playerPosition.Left) / distance;
            double directionY = (inputPosition.Top - playerPosition.Top) / distance;

            Position newPosition = new Position(playerPosition.Top, playerPosition.Left);

            bool moving = true;

            
            if (moving == true)
            {
                newPosition.Left += directionX * speed * elapsed;
                newPosition.Top += directionY * speed * elapsed;
                if (Math.Sqrt((newPosition.Left - playerPosition.Left) * (newPosition.Left - playerPosition.Left)
                   + (newPosition.Top - playerPosition.Top) * (newPosition.Top - playerPosition.Top)) >= distance)
                {
                    newPosition.Left = inputPosition.Left;
                    newPosition.Top = inputPosition.Top;
                    moving = false;
                }
            }

            return newPosition;
        }

        //public static Position GetNewPosition(Player player, Position inputPosition)
        //{
        //    Position newPosition = new Position();
        //    if (inputPosition.Top < playerPosition.Top)
        //    {
        //        newPosition.Top = playerPosition.Top + (moveCount * -1);
        //    }
        //    else
        //    {
        //        newPosition.Top = playerPosition.Top + moveCount;
        //    }

        //    if (inputPosition.Left < playerPosition.Left)
        //    {
        //        newPosition.Left = playerPosition.Left + (moveCount * -1);
        //    }
        //    else
        //    {
        //        newPosition.Left = playerPosition.Left + moveCount;
        //    }

        //    return newPosition;
        //}
    }
}