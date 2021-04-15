using System;

namespace gayshit
{
    public class Bullet : IGameObject
    {
        public  BulletType Type;

        public Bullet()
        {
            Type = BulletType.EnemyBullet;
        }
        public Bullet(BulletType type)
        {
            Type = type;
        }

        public ObjectCommand Act(int x, int y)
        {
            var comm = Type == BulletType.EnemyBullet
                ? new ObjectCommand() { DeltaX = 0, DeltaY = 1 }
                : new ObjectCommand() { DeltaX = 0, DeltaY = -1 };
            return comm;
                
        }

        public bool DeadInConflict(IGameObject conflictedObject)
        {
            if (conflictedObject is Enemy || conflictedObject is LevelBoss)
            {
                if (Type == BulletType.PlayerBullet && Player.CurrentBuff == BonusType.DoubleScore)
                    GameMap.Scores += 2 * GameMap.RegularScore;
                else if (Type == BulletType.PlayerBullet)
                    GameMap.Scores += GameMap.RegularScore;
                if (Type == BulletType.EnemyBullet)
                    return true;
            }
            return true;
        }

        public int GetDrawingPriority()
        {
            return 2;
        }

        public string GetImageFileName()
        {
            return Type == BulletType.EnemyBullet ? "EnemyBullet.png" : "PlayerBullet.png";
        }
    }
}
