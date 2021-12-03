using UnityEngine;
using UnityEngine.EventSystems;
using World;

namespace Grid
{
    [RequireComponent(typeof(MeshFilter))]
    public class GridUnit : MonoBehaviour
    {
        Color _defaultColour;
        public Color beingInspectedColour = Color.blue;
        public Color finishedInspectedColour = Color.green;
        public Color selectedColor = Color.red;
        public Color pathEndColor = Color.magenta;

        public GridUnitState nodeState = GridUnitState.Default;
        GridUnitState _previousState = GridUnitState.Default;

        GameObject _worldController;
        public int X { get; set; }
        public int Z { get; set; }

        public void SetWorld(GameObject world)
        {
            _worldController = world;
        }

        //public void Initialise(GameObject worldController)
        //{
            // var neighbourList = neighbours.ToList();
            //
            // for (var i = 0; i < neighbourList.Count; i++)
            // {
            //     var neighbour = neighbourList[i];
            //
            //     if (neighbour is null)
            //     {
            //         Neighbours[i] = new GridUnitNeighbour { Neighbour = null, TraversalHeuristic = 0};
            //     }
            //     else
            //     {
            //         var heightDifference = (int)Math.Abs(transform.position.y - neighbourList[i].transform.position.y);
            //         Neighbours[i] = new GridUnitNeighbour { Neighbour = neighbourList[i],  TraversalHeuristic = heightDifference };
            //     }
            // }

            //_worldController = worldController;
        //}

        public void SetState(GridUnitState newState)
        {
            nodeState = newState;
        }

        void Start()
        {
            _defaultColour = GetComponent<MeshRenderer>().material.color;
        }

        void Update()
        {
            if (StateChanged())
            {
                var nodeColor = GetNodeColour();

                GetComponent<MeshRenderer>().material.color = nodeColor;

                _previousState = nodeState;
            }
        }

        void OnMouseDown()
        {
            if (_worldController is null)
            {
                Debug.Log("WorldController on GridUnit is null");
                return;
            }

            ExecuteEvents.Execute<IWorldMessageTarget>(_worldController, null, (x, y) => x.OnGridUnitClicked(this));
        }

        bool StateChanged()
        {
            return _previousState != nodeState;
        }

        Color GetNodeColour()
        {
            var nodeColor = nodeState switch
            {
                GridUnitState.BeingInspected => beingInspectedColour,
                GridUnitState.FinishedInspected => finishedInspectedColour,
                GridUnitState.Selected => selectedColor,
                GridUnitState.PathEnd => pathEndColor,
                _ => _defaultColour
            };

            return nodeColor;
        }
    }
}
