using System;
namespace gayshit
{
    public class LevelBoss : IGameObject
    {
        public string BossName;
        private int playerIndex;
        private int shotCounter = 1;
        public int HealthPoints;

        public LevelBoss(string name)
        {
            BossName = name;
            HealthPoints = 100;
        }

        public ObjectCommand Act(int x, int y)
        {
            var dX = 0;
            if (IsPlayerAlive())
            {
                dX = playerIndex < x ? -1 : playerIndex > x ? 1 : 0;
            }
            if (x == playerIndex && shotCounter % 2 == 0)
                GameMap.Map[x, y + 1] = new Bullet(BulletType.EnemyBullet);
            shotCounter += 1;
            var finalCommand = new ObjectCommand() { DeltaX = dX, DeltaY = 0 };
            GameMap.PartitialMovement = finalCommand;
            return finalCommand;
        }

        public bool DeadInConflict(IGameObject conflictedObject)
        {
            if (conflictedObject is Bullet)
                HealthPoints -= 10;
            if (HealthPoints <= 0)
                GameMap.IsOver = true;
            return HealthPoints <= 0;
        }

        public int GetDrawingPriority()
        {
            return 5;
        }

        public string GetImageFileName()
        {
            return BossName + ".png";
        }

        private bool IsPlayerAlive()
        {
            for (var i = 0; i < GameMap.MapWidth; i++)
            {
                if (GameMap.Map[i, GameMap.MapHeight - 1] is Player)
                {
                    playerIndex = i;
                    return true;
                }
            }
            return false;
        }
    }
}
