using System;
namespace gayshit
{
    public class Bonus : IGameObject
    {
        public BonusType Type;

        public Bonus()
        {
            Type = BonusType.NoBonus;
        }

        public Bonus(BonusType type)
        {
            Type = type;
        }

        public ObjectCommand Act(int x, int y) => new ObjectCommand() { DeltaX = 0, DeltaY = 1 };

        public bool DeadInConflict(IGameObject conflictedObject)
        {
            if (conflictedObject is Player player)
            {
                player.Buff = Type;
            }
            return true;
        }

        public int GetDrawingPriority()
        {
            return 4;
        }

        public string GetImageFileName()
        {
            switch (Type)
            {
                case BonusType.Immortality:
                    return "ImmortalityBonus.png";
                case BonusType.TripleShot:
                    return "TripleShotBonus.png";
                case BonusType.DoubleScore:
                    return "DoubleScoreBonus.png";
                default:
                    return "Player.png";
            }
        }
    }
}