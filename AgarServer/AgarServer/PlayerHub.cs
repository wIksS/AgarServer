using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace AgarServer
{
    public class PlayerHub : Hub
    {
        private static Dictionary<string, Broadcaster> broadcasters = new Dictionary<string, Broadcaster>();

        public override Task OnDisconnected(bool stopCalled)
        {
            PlayerHub.RemovePlayer(
                GlobalHost.ConnectionManager.GetHubContext<PlayerHub>(),
                Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }

        public static void RemovePlayer(IHubContext context, string id)
        {
            var player = GameEngine.Instance.RemovePlayer(id);
            if (player != null)
            {
                PlayerHub.broadcasters[player.Id].Terminate();
                PlayerHub.broadcasters.Remove(player.Id);

                context.Clients.All.removePlayer(player);
            }
        }

        public void UpdatePlayer(PlayerInput input)
        {
            if (PlayerHub.broadcasters.ContainsKey(input.Id))
            {
                PlayerHub.broadcasters[input.Id].MousePosition = input.MousePosition;
            }
        }

        public void SpawnPlayer()
        {
            Player player = GameEngine.Instance.CreatePlayer(Context.ConnectionId);
            player.ConnectionId = Context.ConnectionId;
            Clients.AllExcept(Context.ConnectionId).spawnNewPlayer(player);
            Clients.Caller.spawnCurrentPlayer(player);
            Clients.Caller.spawnAllShapes(GameEngine.Instance.ShapesGenerator.GetAllShapes());
            var otherPlayers = GameEngine.Instance.GetOtherPlayers(player.Id);
            Clients.Caller.spawnOtherPlayers(otherPlayers);

            IPlayerColliser colliser = NinjectObjectFactory.GetObject<IPlayerColliser>();
            IShapesColliser shapesColliser = NinjectObjectFactory.GetObject<IShapesColliser>();
            PlayerHub.broadcasters.Add(player.Id, new Broadcaster(colliser, shapesColliser,player, new Position(0, 0)));
        }
    }
}