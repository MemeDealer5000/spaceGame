using System;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace gayshit
{
    public class Player : IGameObject
    {
        public int Lives;
        public int Health = 100;
        public BonusType Buff;
        public static BonusType CurrentBuff;
        public bool IsShooting = false;

        public Player()
        {
            CurrentBuff = BonusType.NoBonus;
            Lives = GameMap.Lives;
        }

        public ObjectCommand Act(int x, int y)
        {
            if (GameMap.KeyPressed == Keys.Left && x - 1 >= 0 && x - 1 < GameMap.MapWidth)
            {
                return new ObjectCommand { DeltaX = -1, DeltaY = 0 };
            }

            if (GameMap.KeyPressed == Keys.Right && x + 1 >= 0 && x + 1 < GameMap.MapWidth)
            {
                return new ObjectCommand { DeltaX = 1, DeltaY = 0 };
            }

            if (GameMap.KeyPressed == Keys.Space)
            {
                IsShooting = true;
            }
            return new ObjectCommand();
        }
        
        public bool DeadInConflict(IGameObject conflictedObject)
        {
            if (conflictedObject is Bullet)
            {
                Health -= 10;
            }
            if (Health <= 0)
            {
                Lives -= 1;
                GameMap.Lives -= 1;
                Health = 100;
            }
            if (conflictedObject is Terrain)
                return false;
            if (conflictedObject is Bonus bonus)
            {
                var type = bonus.Type;
                switch (type)
                {
                    case BonusType.TripleShot:
                        Buff = BonusType.TripleShot;
                        break;
                    case BonusType.Immortality:
                        return false;
                }
            }
            var isDead = Lives <= 0 && conflictedObject is Bullet;
            if (isDead)
            {
                GameMap.IsGameOver = true;
            }
            return isDead;
        }

        public int GetDrawingPriority()
        {
            return 0;
        }

        public string GetImageFileName()
        {
            return "Player.png";
        }
    }
}
