using System;
namespace gayshit
{
    public interface IGameObject
    {
        string GetImageFileName();
        int GetDrawingPriority();
        ObjectCommand Act(int x, int y);
        bool DeadInConflict(IGameObject conflictedObject);
    }
}
