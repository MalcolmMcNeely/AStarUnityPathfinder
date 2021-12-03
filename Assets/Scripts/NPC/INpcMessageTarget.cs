using Grid;
using UnityEngine.EventSystems;

namespace NPC
{
    public interface INpcMessageTarget : IEventSystemHandler
    {
        void SetUpdateSpeed(float updateSpeed);
        void SetWorld(GridUnit[,] world, int worldSizeX, int worldSizeZ, float traversalMultiplier);
        void SetPathStart(int x, int z);
        void SetPathEnd(int x, int z);
        void StartPathfinding();
        void StopPathfinding();
    }
}