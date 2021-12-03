using System;
using System.Collections.Generic;
using Grid;
using NPC;
using UnityEngine;
using UnityEngine.EventSystems;
using World.WorldGeneration;

namespace World
{
    public class World : MonoBehaviour, IWorldMessageTarget
    {
        public GameObject gridUnitPrefab;
        public GameObject npcPrefab;

        [Range(1, 10000)]
        public int gridSizeX;

        [Range(1, 10000)]
        public int gridSizeZ;

        [Range(1, 10)]
        public float traversalMultiplier = 10;

        public WorldGenerationType generationType = WorldGenerationType.Flat;
        public WorldSelectionBehaviour selectionBehaviour = WorldSelectionBehaviour.DebugGridUnits;

        public float updateChangeRate = 0.05f;
        public float updateSpeed = 0.016f;

        GridUnit[,] _worldGrid;
        GameObject _npc;

        void Start()
        {
            CreateWorld(gridUnitPrefab, gridSizeX, gridSizeZ);
        }

        void Update()
        {
            ProcessWorldInput();
        }

        void CreateWorld(GameObject prefab, int sizeX, int sizeZ)
        {
            _worldGrid = new GridUnit[sizeX, sizeZ];

            IWorldGenerator worldGenerator = GetWorldGenerator(generationType);

            var gridUnitMeshFilter = prefab.GetComponent<MeshFilter>();
            var gridUnitSize = gridUnitMeshFilter.sharedMesh.bounds.size;

            worldGenerator.Generate(_worldGrid, gridUnitSize, sizeX, sizeZ, CreateGridUnit);

            Debug.Log($"{generationType} World created");
        }

        GridUnit CreateGridUnit(Vector3 position, int x, int z)
        {
            var gridUnit = Instantiate(gridUnitPrefab, position, Quaternion.identity).GetComponent<GridUnit>();
            gridUnit.X = x;
            gridUnit.Z = z;
            gridUnit.SetWorld(gameObject);

            return gridUnit;
        }

        IWorldGenerator GetWorldGenerator(WorldGenerationType generationType)
        {
            return generationType switch
            {
                WorldGenerationType.Flat => new FlatWorldGenerator(),
                WorldGenerationType.Valley => new ValleyWorldGenerator(),
                WorldGenerationType.Noisy => new NoisyWorldGenerator(),
                _ => throw new ArgumentOutOfRangeException(nameof(generationType), generationType, null)
            };
        }

        void ProcessWorldInput()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                ExecuteEvents.Execute<INpcMessageTarget>(_npc, null, (x, y) => x.StartPathfinding());
                Debug.Log("Pathfinding started");
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                ExecuteEvents.Execute<INpcMessageTarget>(_npc, null, (x, y) => x.StopPathfinding());
                Debug.Log("Pathfinding stopped");
            }

            if (Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                updateSpeed += updateChangeRate;
                ExecuteEvents.Execute<INpcMessageTarget>(_npc, null, (x, y) => x.SetUpdateSpeed(updateSpeed));
                Debug.Log($"Updated speed incremented to {updateSpeed}");
            }

            if (Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                updateSpeed -= updateChangeRate;
                ExecuteEvents.Execute<INpcMessageTarget>(_npc, null, (x, y) => x.SetUpdateSpeed(updateSpeed));
                Debug.Log($"Updated speed decremented to {updateSpeed}");
            }
        }

        public void OnGridUnitClicked(GridUnit gridUnit)
        {
            switch (selectionBehaviour)
            {
                case WorldSelectionBehaviour.Normal:
                    if (_npc is null)
                    {
                        SpawnNpc(gridUnit);
                    }
                    else
                    {
                        SetPathEnd(gridUnit);
                    }
                    break;
                case WorldSelectionBehaviour.DebugGridUnits:
                    SetGridUnitAndNeighboursAsSelected(gridUnit);
                    break;
            }
        }

        void SpawnNpc(GridUnit gridUnit)
        {
            _npc = Instantiate(npcPrefab, gridUnit.transform.position, Quaternion.identity);

            ExecuteEvents.Execute<INpcMessageTarget>(_npc, null, (x, y) => x.SetWorld(_worldGrid, gridSizeX, gridSizeZ, traversalMultiplier));
            ExecuteEvents.Execute<INpcMessageTarget>(_npc, null, (x, y) => x.SetPathStart(gridUnit.X, gridUnit.Z));
            ExecuteEvents.Execute<INpcMessageTarget>(_npc, null, (x, y) => x.SetUpdateSpeed(updateSpeed));

            Debug.Log("NPC Spawned");
        }

        void SetPathEnd(GridUnit gridUnit)
        {
            ExecuteEvents.Execute<INpcMessageTarget>(_npc, null, (x, y) => x.SetPathEnd(gridUnit.X, gridUnit.Z));
            Debug.Log("Path End Set");
        }

        void SetGridUnitAndNeighboursAsSelected(GridUnit gridUnit)
        {
            gridUnit.SetState(GridUnitState.Selected);

            for (var x = 0; x < gridSizeX; x++)
            {
                for (var z = 0; z < gridSizeZ; z++)
                {
                    if (_worldGrid[x, z].Equals(gridUnit))
                    {
                        var neighbours = GetNeighbours(x, z);
                        foreach (var neighbour in neighbours)
                        {
                            neighbour.SetState(GridUnitState.Selected);
                        }

                        return;
                    }
                }
            }

            Debug.Log("GridUnit not found in world");
        }

        IEnumerable<GridUnit> GetNeighbours(int x, int z)
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
                return z + 1 < gridSizeZ;
            }

            bool CanMoveDown()
            {
                return z - 1 >= 0;
            }

            bool CanMoveRight()
            {
                return x + 1 < gridSizeX;
            }

            bool CanMoveLeft()
            {
                return x - 1 >= 0;
            }
        }
    }
}