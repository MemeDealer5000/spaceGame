using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace gayshit
{
    public class GameState
    {
        public const int ElementSize = 32;
        public List<CreatureAnimation> Animations = new List<CreatureAnimation>();
        private static bool doesSpawn = false;

        public void BeginAction()
        {
            Animations.Clear();
            for (var x = 0; x < GameMap.MapWidth; x++)
                for (var y = 0; y < GameMap.MapHeight; y++)
                {
                    var creature = GameMap.Map[x, y];
                    if (creature == null) continue;
                    if (creature is Player player)
                        if(player.IsShooting)
                        {
                            player.IsShooting = false;
                            GameMap.Map[x, y - 1] = new Bullet(BulletType.PlayerBullet);
                            var bullet = GameMap.Map[x, y - 1];
                            var comm = bullet.Act(x, y - 1);
                            if (x + comm.DeltaX < 0 || x + comm.DeltaX >= GameMap.MapWidth || y - 1 + comm.DeltaY < 0 ||
                                y - 1 + comm.DeltaY >= GameMap.MapHeight)
                            {
                                GameMap.Map[x, y - 1] = null;
                                continue;
                            }

                            Animations.Add(
                                new CreatureAnimation
                                {
                                    Command = comm,
                                    Creature = bullet,
                                    Location = new Point(x * ElementSize, y * ElementSize),
                                    TargetLogicalLocation = new Point(x + comm.DeltaX, y + comm.DeltaY)
                                });
                        }

                    var command = creature.Act(x, y);

                    if (x + command.DeltaX < 0 || x + command.DeltaX >= GameMap.MapWidth || y + command.DeltaY < 0 ||
                        y + command.DeltaY >= GameMap.MapHeight)
                    {
                        GameMap.Map[x, y] = null;
                        continue;
                    }
                            
                    Animations.Add(
                        new CreatureAnimation
                        {
                            Command = command,
                            Creature = creature,
                            Location = new Point(x * ElementSize, y * ElementSize),
                            TargetLogicalLocation = new Point(x + command.DeltaX, y + command.DeltaY)
                        });
                }

            Animations = Animations.OrderByDescending(z => z.Creature.GetDrawingPriority()).ToList();
        }

        public void EndActions()
        {
            if (GameMap.IsGameOver)
                Application.Restart();
            var creaturesPerLocation = GetObjectsInCell();
            for (var x = 0; x < GameMap.MapWidth; x++)
                for (var y = 0; y < GameMap.MapHeight; y++)
                {
                    GameMap.Map[x, y] = SelectCellWinner(creaturesPerLocation, x, y);
                    if (doesSpawn)
                    {
                        GameMap.Map[0, y] = new Enemy();
                        GameMap.EnemyCounter -= 1;
                        doesSpawn = false;
                    }
                }
            if (GameMap.EnemyCounter < 0)
                PrepareMapForBoss();
            if (GameMap.IsOver)
            {
                GameMap.CreateMap();
                GameMap.IsOver = false;
            }
        }

        private static IGameObject SelectCellWinner(List<IGameObject>[,] creatures, int x, int y)
        {
            var candidates = creatures[x, y];
            var aliveCandidates = candidates.ToList();
            IGameObject removedCandidate = null;
            foreach (var candidate in candidates)
                foreach (var rival in candidates)
                    if (rival != candidate && candidate.DeadInConflict(rival))
                    {
                        aliveCandidates.Remove(candidate);
                        if (candidate is Enemy && x != 0)
                        {
                            removedCandidate = candidate;
                            GameMap.Map[0, y] = new Enemy();
                            GameMap.EnemyCounter -= 1;
                        }
                        else doesSpawn |= (candidate is Enemy && x == 0);
                    }
            if (GameMap.EnemyCounter > 0 && removedCandidate is Enemy)
            { 
                var rnd = new Random();
                var bonusType = rnd.Next(0, 4);
                var bonus = new Bonus((BonusType)bonusType);
                GameMap.Map[x, y + 1] = bonus;
                creatures[x, y + 1] = new List<IGameObject>() { bonus };
            }
            if (aliveCandidates.Count > 1)
                    throw new Exception(
                    $"Creatures {aliveCandidates[0].GetType().Name} and {aliveCandidates[1].GetType().Name} claimed the same map cell");

            return aliveCandidates.FirstOrDefault();
        }

        private static void PrepareMapForBoss()
        {
            for (var x = 0; x < GameMap.MapWidth; x++)
                for (var j = 0; j < GameMap.MapHeight; j++)
                {
                    if (!(GameMap.Map[x, j] is Player))
                        GameMap.Map[x, j] = null;
                }
            GameMap.Map[GameMap.MapWidth / 2, 4] = new LevelBoss("Zalupa");
            GameMap.EnemyCounter = 50;
        }

        private List<IGameObject>[,] GetObjectsInCell()
        {
            var creatures = new List<IGameObject>[GameMap.MapWidth, GameMap.MapHeight];
            for (var x = 0; x < GameMap.MapWidth; x++)
                for (var y = 0; y < GameMap.MapHeight; y++)
                    creatures[x, y] = new List<IGameObject>();
            foreach (var e in Animations)
            {
                var x = e.TargetLogicalLocation.X;
                var y = e.TargetLogicalLocation.Y;
                var nextCreature = e.Command.TransformTo ?? e.Creature;
                creatures[x, y].Add(nextCreature);
            }

            return creatures;
        }
    }
}