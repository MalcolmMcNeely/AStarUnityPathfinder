using System;
using System.Collections.Generic;
using Grid;

namespace Pathfinding
{
    public class PathNode
    {
        public GridUnit Node { get; set; }
        public PathNode Via { get; set; }
        public List<PathNode> Neighbours { get; } = new List<PathNode>();
        public int DistanceCost { get; private set; }
        public int TraversalCost { get; set; } = int.MaxValue;
        public int TotalCost => DistanceCost + TraversalCost;

        int _x;
        int _z;
        int _height;
        float _traversalMultiplier;

        public static PathNode Create(GridUnit node, int x, int z, float traversalMultiplier)
        {
            return new PathNode
            {
                Node = node,
                _height = (int)node.transform.position.y,
                _x = x,
                _z = z,
                _traversalMultiplier = traversalMultiplier
            };
        }

        public int CalculateTraversalCost(PathNode via)
        {
            return (int) (Math.Abs(_height + via.Node.transform.position.y) * _traversalMultiplier);
        }

        public void CalculateDistanceCost(int endX, int endZ)
        {
            DistanceCost = Math.Abs(_x - endX) + Math.Abs(_z - endZ);
        }

        public void AddNeighbours(IEnumerable<PathNode> neighbours)
        {
            Neighbours.AddRange(neighbours);
        }
    }
}