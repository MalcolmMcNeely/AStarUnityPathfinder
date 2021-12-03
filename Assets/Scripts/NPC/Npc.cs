using Grid;
using Pathfinding;
using UnityEngine;

namespace NPC
{
    public class Npc : MonoBehaviour, INpcMessageTarget
    {
        readonly AStarPathfinder _pathfinder = new AStarPathfinder();
        float _updateSpeed;
        float _updateTimer;

        public void SetUpdateSpeed(float updateSpeed)
        {
            _updateSpeed = updateSpeed;
        }

        public void SetWorld(GridUnit[,] world, int worldSizeX, int worldSizeZ, float traversalMultiplier)
        {
            _pathfinder.SetWorld(world, worldSizeX, worldSizeZ, traversalMultiplier);
        }

        public void SetPathStart(int x, int z)
        {
            _pathfinder.SetStart(x, z);
        }

        public void SetPathEnd(int x, int z)
        {
            _pathfinder.SetEnd(x, z);
        }

        public void StartPathfinding()
        {
            _pathfinder.Start();
        }

        public void StopPathfinding()
        {
            _pathfinder.Stop();
        }

        void Update()
        {
            var timeDelta = Time.deltaTime;

            if ((_updateTimer += timeDelta) >= _updateSpeed)
            {
                _pathfinder.Update();

                _updateTimer = 0;
            }
        }
    }
}