using AgarServer.Common;
using AgarServer.StaticObjects;
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
        private Dictionary<string, KeyValuePair<Player, Position>> players;
        private IHubContext context;
        private CancellationTokenSource ts;
        private CancellationToken ct;
        private IPlayerColliser colliser;
        private IShapesColliser shapesCollsier;
        private List<MovingShape> MovingShapes;
        private static readonly object lockObject = new object();

        public Broadcaster(IPlayerColliser colliser, IShapesColliser shapeColliser, Position mousePosition)
        {
            this.MovingShapes = new List<MovingShape>();
            this.shapesCollsier = shapeColliser;
            this.colliser = colliser;
            this.players = new Dictionary<string, KeyValuePair<Player, Position>>();
            this.ts = new CancellationTokenSource();
            this.ct = ts.Token;
            context = GlobalHost.ConnectionManager.GetHubContext<PlayerHub>();
            Task.Run(() => UpdatePlayer(), ct);
        }

        public void UpdatePlayer()
        {
            while (true)
            {
                if (ct.IsCancellationRequested)
                {
                    return;
                }

                lock (lockObject)
                {
                    foreach (var playerObj in this.players)
                    {
                        var player = playerObj.Value.Key;
                        var position = Movement.GetNewPosition(
                        Position.GetCircleCenter(player.Position, player.Radius),
                        playerObj.Value.Value);

                        // TODO: This operation is not needed refactor later to remove it
                        position.Top -= player.Radius;
                        position.Left -= player.Radius;
                        player.Position = position;

                        context.Clients.All.updatePlayer(player);

                        MoveMovingShapes();
                        PerformCollision(player);
                        PerformShapesCollision(player);
                    }
                }

                Thread.Sleep(1000 / GlobalConstants.UpdatesPerSecond);
            }
        }

        private void MoveMovingShapes()
        {
            for (int i = 0; i < MovingShapes.Count; i++)
            {
                var position = Movement.GetNewPosition(
                        Position.GetCircleCenter(MovingShapes[i].Position, MovingShapes[i].Radius),
                        MovingShapes[i].Velocity.Direction,
                        MovingShapes[i].Velocity.Speed);

                position.Top -= MovingShapes[i].Radius;
                position.Left -= MovingShapes[i].Radius;

                MovingShapes[i].Velocity.Speed -= GlobalConstants.MovingShapeGravity;
                if (MovingShapes[i].Velocity.Speed < 0)
                {
                    MovingShapes.Remove(MovingShapes[i]);
                    continue;
                }

                MovingShapes[i].Position = position;
                this.context.Clients.All.updateShape(MovingShapes[i]);
            }
        }

        public void AddPlayer(Player player)
        {
            lock (lockObject)
            {
                this.players.Add(player.Id, new KeyValuePair<Player, Position>(player, new Position(0, 0)));
            }
        }

        public void ChangePlayerMousePosition(Player player, Position position)
        {
            lock (lockObject)
            {
                this.players[player.Id] = new KeyValuePair<Player, Position>(player, position);
            }
        }

        public void SplitPlayer(Player player)
        {
            if (this.players.ContainsKey(player.Id) && player.Radius > 20)
            {
                player.Radius -= GlobalConstants.MovingShapesRadius / GlobalConstants.UpdateShapesRadius;
                context.Clients.All.changePlayerRadius(player);

                var shape = new MovingShape(-1, new Position(0, 0), GlobalConstants.MovingShapesRadius, player.Color);
                shape = GameEngine.Instance.ShapesGenerator.AddShape(shape) as MovingShape;
                shape.Velocity = new Velocity(
                   Movement.GetNewPosition(Position.GetCircleCenter(player.Position, player.Radius),
                   this.players[player.Id].Value,
                   50000 / 0.01, false));

                var center = Position.GetCircleCenter(player.Position, player.Radius);
                center = new Position(center.Top - 18, center.Left - 18);
                shape.Position = Movement.GetNewPosition(
                    center,
                    this.players[player.Id].Value,
                    (player.Radius + 10) / 0.01, false);

                this.MovingShapes.Add(shape);
                this.context.Clients.All.spawnShape(shape);
            }
        }

        public void RemovePlayer(Player player)
        {
            lock (lockObject)
            {
                this.players.Remove(player.Id);
            }
        }

        public int GetPlayerCount()
        {
            return this.players.Count;
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
                    Task.Run(() => PlayerHub.RemovePlayer(context, player.Id));
                    collisedPlayer.Radius += player.Radius / GlobalConstants.UpdateShapesRadius;
                    context.Clients.All.changePlayerRadius(collisedPlayer);
                }
                else
                {
                    Task.Run(() => PlayerHub.RemovePlayer(context, collisedPlayer.Id));
                    player.Radius += collisedPlayer.Radius / GlobalConstants.UpdateShapesRadius;
                    context.Clients.All.changePlayerRadius(player);
                }
            }
        }

        private void PerformShapesCollision(Player player)
        {
            StaticShape collisedShape = shapesCollsier.CheckCollision(player);
            if (collisedShape != null)
            {
                player.Radius += collisedShape.Radius / GlobalConstants.UpdateRadius;
                GameEngine.Instance.ShapesGenerator.RemoveShape(collisedShape.Id);
                context.Clients.All.changePlayerRadius(player);
                context.Clients.All.removeShape(collisedShape.Id);
                var newShape = GameEngine.Instance.ShapesGenerator.AddShape();
                context.Clients.All.spawnShape(newShape);
            }
        }
    }
}