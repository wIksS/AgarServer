using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgarServer
{
    public interface IPlayerColliser 
    {
        Player CheckCollision(IEnumerable<Player> players, Player currentPlayer);
    }
}