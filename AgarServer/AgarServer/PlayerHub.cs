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

        public static async Task RemovePlayer(IHubContext context, string id)
        {
            var player = GameEngine.Instance.RemovePlayer(id);
            if (player != null)
            {
                if (PlayerHub.broadcasters["groupid"].GetPlayerCount() == 0)
                {
                    PlayerHub.broadcasters["groupid"].Terminate();
                    PlayerHub.broadcasters.Remove("groupid");
                }
                else
                {
                    PlayerHub.broadcasters["groupid"].RemovePlayer(player);
                }

                context.Clients.All.removePlayer(player);
            }
        }

        public void UpdatePlayer(PlayerInput input)
        {
            if (PlayerHub.broadcasters.ContainsKey("groupid"))
            {
                PlayerHub.broadcasters["groupid"].ChangePlayerMousePosition(GameEngine.Instance.GetPlayer(input.Id), input.MousePosition);
            }
        }

        public void SplitPlayer(string id)
        {
            if (PlayerHub.broadcasters.ContainsKey("groupid"))
            {
                PlayerHub.broadcasters["groupid"].SplitPlayer(GameEngine.Instance.GetPlayer(id));
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
            if (!PlayerHub.broadcasters.ContainsKey("groupid"))
            {
                PlayerHub.broadcasters.Add("groupid", new Broadcaster(colliser, shapesColliser, new Position(0, 0)));
            }

            PlayerHub.broadcasters["groupid"].AddPlayer(player);
        }
    }
}