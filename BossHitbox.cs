using System;
namespace gayshit
{
    public class BossHitbox : IGameObject
    {
        public static int Health;

        public ObjectCommand Act(int x, int y)
        {
            //if (GameMap.Map[x, y] != null)
                //return new ObjectCommand { DeltaX = 0, DeltaY = 0 };
            return GameMap.PartitialMovement;
        }

        public bool DeadInConflict(IGameObject conflictedObject)
        {
            GameMap.Scores += 20;
            return false;
        }

        public int GetDrawingPriority()
        {
            return 1;
        }

        public string GetImageFileName()
        {
            return "Hitbox.png";
        }
    }
}
