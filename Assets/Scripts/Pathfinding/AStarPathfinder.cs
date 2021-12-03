using System.Collections.Generic;
using System.Diagnostics;
using Grid;
using NPC;
using Utilities;
using Debug = UnityEngine.Debug;

namespace Pathfinding
{
    public class AStarPathfinder
    {
        PathNode[,] _worldGrid;
        int _worldX;
        int _worldZ;

        int? _startX;
        int? _startZ;
        int? _endX;
        int? _endZ;

        RunningState _runningState = RunningState.Stopped;
        PriorityQueue<PathNode> _openList;
        readonly List<PathNode> _closedList = new List<PathNode>();

        bool IsStarted => _runningState == RunningState.Started;
        bool IsStopped => _runningState == RunningState.Stopped;
        bool IsFinished => _runningState == RunningState.Finished;
        bool CanStart => _startX != null && _startZ != null && _endX != null && _endZ != null && !IsFinished;

        public List<PathNode> Path { get; } = new List<PathNode>();

        public void Update()
        {
#if DEBUG
            var stopwatch = new Stopwatch();
            stopwatch.Start();
#endif

            if (IsStopped || IsFinished)
            {
                return;
            }

            var current = _openList.Pop();

            if (current is null)
            {
                Debug.Log("Can not find path");
                _runningState = RunningState.Finished;
                return;
            }

            if (IsEndNode(current))
            {
                Finish(current);
            }

            CloseNode(current);

            var neighbours = current.Neighbours;

            foreach (var neighbour in neighbours)
            {
                if (_closedList.Contains(neighbour))
                {
                    continue;
                }

                var tentativeTraversalCost = neighbour.CalculateTraversalCost(current);
                if (tentativeTraversalCost < neighbour.TraversalCost)
                {
                    neighbour.Via = current;
                    neighbour.TraversalCost = tentativeTraversalCost;
                }

                if (!_openList.Contains(neighbour))
                {
                    OpenNode(neighbour);
                }
            }

#if DEBUG
            stopwatch.Stop();
            Debug.Log($"update time: {stopwatch.Elapsed.Ticks.ToString()}");
#endif
        }

        void OpenNode(PathNode node)
        {
            _openList.Push(node, node.TotalCost);
            node.Node.SetState(GridUnitState.BeingInspected);
        }

        void CloseNode(PathNode node)
        {
            _closedList.Add(node);
            node.Node.SetState(GridUnitState.FinishedInspected);
        }

        bool IsEndNode(PathNode current)
        {
            return current.Node.X == _endX && current.Node.Z == _endZ;
        }

        void Finish(PathNode finishNode)
        {
            _runningState = RunningState.Finished;

            var currentNode = finishNode;

            while (currentNode != null)
            {
                currentNode.Node.SetState(GridUnitState.Selected);
                Path.Add(currentNode);
                currentNode = currentNode.Via;
            }

            Path.Reverse();
        }

        public void SetStart(int x, int z)
        {
            _startX = x;
            _startZ = z;
            _worldGrid[x, z].Node.SetState(GridUnitState.BeingInspected);
        }

        public void SetEnd(int x, int z)
        {
            _endX = x;
            _endZ = z;
            _worldGrid[x, z].Node.SetState(GridUnitState.Selected);
            InitialiseNodes();
        }

        public void Start()
        {
            if (CanStart)
            {
                _openList = new PriorityQueue<PathNode>();
                _openList.Push(_worldGrid[_startX!.Value, _startZ!.Value], 0);
                _runningState = RunningState.Started;
            }
        }

        public void Stop()
        {
            if (IsStarted)
            {
                _runningState = RunningState.Stopped;
            }
        }

        public void SetWorld(GridUnit[,] world, int x, int z, float traversalMultiplier)
        {
            _worldGrid = new PathNode[x, z];
            _worldX = x;
            _worldZ = z;

            for (var i = 0; i < _worldX; i++)
            {
                for (var j = 0; j < _worldZ; j++)
                {
                    _worldGrid[i, j] = PathNode.Create(world[i, j], i, j, traversalMultiplier);
                }
            }
        }

        void InitialiseNodes()
        {
            for (var i = 0; i < _worldX; i++)
            {
                for (var j = 0; j < _worldZ; j++)
                {
                    var node = _worldGrid[i, j];
                    node.CalculateDistanceCost(_endX!.Value, _endZ!.Value);
                    node.AddNeighbours(GetNeighbours(i, j));
                }
            }
        }

        IEnumerable<PathNode> GetNeighbours(int x, int z)
        {
            if (CanMoveUp())
            {
                yield return _worldGrid[x, z + 1];

                if (CanMoveLeft())
                {
                    yield return _worldGrid[x - 1, z + 1];
                }

                if (CanMoveRight())
                {
                    yield return _worldGrid[x + 1, z + 1];
                }
            }

            if (CanMoveDown())
            {
                yield return _worldGrid[x, z - 1];

                if (CanMoveLeft())
                {
                    yield return _worldGrid[x - 1, z - 1];
                }

                if (CanMoveRight())
                {
                    yield return _worldGrid[x + 1, z - 1];
                }
            }

            if (CanMoveLeft())
            {
                yield return _worldGrid[x - 1, z];
            }

            if (CanMoveRight())
            {
                yield return _worldGrid[x + 1, z];
            }

            bool CanMoveUp()
            {
                return z + 1 < _worldZ;
            }

            bool CanMoveDown()
            {
                return z - 1 >= 0;
            }

            bool CanMoveRight()
            {
                return x + 1 < _worldX;
            }

            bool CanMoveLeft()
            {
                return x - 1 >= 0;
            }
        }
    }
}