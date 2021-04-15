using System;
namespace gayshit
{
    public class Enemy : IGameObject
    {
        public readonly string Type;
        public bool IsShooting = false;
        private int playerIndex;
        private int shotCounter = 2;

        public Enemy()
        {
            Type = "nigger";
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
            if (GameMap.Map[x + dX, y] != null)
                return new ObjectCommand() { DeltaX = 0, DeltaY = 0 };
            return new ObjectCommand() { DeltaX = dX, DeltaY = 0 };
        }

        public bool DeadInConflict(IGameObject conflictedObject)
        {
            return conflictedObject is Bullet bullet && bullet.Type != BulletType.EnemyBullet;
        }

        public int GetDrawingPriority()
        {
            return 3;
        }

        private bool IsPlayerAlive()
        { 
            for (var i =0; i< GameMap.MapWidth; i++)
            {
                if (GameMap.Map[i, GameMap.MapHeight - 1] is Player)
                {
                    playerIndex = i;
                    return true;
                }
            }
            return false;
        }

        public string GetImageFileName()
        {
            var type = string.Empty;
            switch (Type)
            {
                case "regular":
                    type = "Regular.png";
                    break;
                case "heavy":
                    type = "Heavy.png";
                    break;
                case "fast":
                    type = "Fast.png";
                    break;
                case "nigger":
                    type = "Nigger.png";
                    break;
            }
            return type;
        }
    }
}
