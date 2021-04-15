using System;
namespace gayshit
{
    public class Terrain : IGameObject
    {

        public ObjectCommand Act(int x, int y)
        {
            return new ObjectCommand { DeltaX = 0, DeltaY = 0 };
        }

        public bool DeadInConflict(IGameObject conflictedObject)
        {
            return true;
        }

        public int GetDrawingPriority()
        {
            return 7;
        }

        public string GetImageFileName()
        {
            return "Terrain.png";
        }
    }
}
