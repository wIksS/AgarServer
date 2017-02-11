using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace AgarServer
{
    public class Broadcaster
    {
        private Player player;
        private IHubContext context;
        private CancellationTokenSource ts;
        private CancellationToken ct;
        private IPlayerColliser colliser;
        private IShapesColliser shapesCollsier;

        public Broadcaster(IPlayerColliser colliser, IShapesColliser shapeColliser, Player player, Position mousePosition)
        {
            this.shapesCollsier = shapeColliser;
            this.colliser = colliser;
            this.MousePosition = mousePosition;
            this.player = player;
            this.ts = new CancellationTokenSource();
            this.ct = ts.Token;
            context = GlobalHost.ConnectionManager.GetHubContext<PlayerHub>();
            Task.Run(() => UpdatePlayer(), ct);

        }

        public Position MousePosition { get; set; }

        public void UpdatePlayer()
        {
            while (true)
            {
                if (ct.IsCancellationRequested)
                {
                    return;
                }

                var position = Movement.GetNewPosition(
                    Position.GetCircleCenter(player.Position, player.Radius),
                    MousePosition);

                // TODO: This operation is not needed refactor later to remove it
                position.Top -= player.Radius; 
                position.Left -= player.Radius;
                player.Position = position;

                context.Clients.All.updatePlayer(player);

                PerformCollision(player);
                PerformShapesCollision(player);

                Thread.Sleep(50);
            }
        }

        public void Terminate()
        {
            this.ts.Cancel();
        }

        private void PerformCollision(Player player)
        {
            Player collisedPlayer = colliser.CheckCollision(GameEngine.Instance.Players.Values, player);
            if (collisedPlayer != null)
            {
                if (collisedPlayer.Radius > player.Radius)
                {
                    PlayerHub.RemovePlayer(context, player.Id);
                    collisedPlayer.Radius += player.Radius;
                    context.Clients.All.changePlayerRadius(collisedPlayer);
                }
                else
                {
                    PlayerHub.RemovePlayer(context, collisedPlayer.Id);
                    player.Radius += collisedPlayer.Radius;
                    context.Clients.All.changePlayerRadius(player);
                }
            }
        }

        private void PerformShapesCollision(Player player)
        {
            StaticShape collisedShape = shapesCollsier.CheckCollision(player);
            if (collisedShape != null)
            {
                player.Radius += collisedShape.Radius;
                GameEngine.Instance.ShapesGenerator.RemoveShape(collisedShape.Id);
                context.Clients.All.changePlayerRadius(player);
                context.Clients.All.removeShape(collisedShape.Id);
            }
        }
    }
}