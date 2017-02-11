using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgarServer
{
    public class CircleColliser : IPlayerColliser
    {
        public Player CheckCollision(IEnumerable<Player> players, Player currentPlayer)
        {
            foreach (var player in players)
            {
                if (player.Id != currentPlayer.Id)
                {
                    Position playerCenter = Position.GetCircleCenter(player.Position, player.Radius);
                    Position currentPlayerCenter = Position.GetCircleCenter(currentPlayer.Position, currentPlayer.Radius);

                    double distanceBetweenPoints = (playerCenter.Left - currentPlayerCenter.Left) * (playerCenter.Left - currentPlayerCenter.Left) +
                    (playerCenter.Top - currentPlayerCenter.Top)*(playerCenter.Top - currentPlayerCenter.Top);
                    double sumRadius = (player.Radius + currentPlayer.Radius) * (player.Radius + currentPlayer.Radius);

                    if (distanceBetweenPoints <= sumRadius)
                    {
                        return player;
                    }
                }
            }

            return null;
        }
    }
}