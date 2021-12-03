using System;
using Grid;
using UnityEngine;

namespace World.WorldGeneration
{
    public class FlatWorldGenerator : IWorldGenerator
    {
        public void Generate(GridUnit[,] worldGrid, Vector3 gridUnitSize, int sizeX, int sizeZ, Func<Vector3, int, int, GridUnit> create)
        {
            for(var x = 0; x < sizeX; x++)
            {
                for (var z = 0; z < sizeZ; z++)
                {
                    var position = new Vector3(gridUnitSize.x + x * gridUnitSize.x, 0, gridUnitSize.z + z * gridUnitSize.z);

                    worldGrid[x, z] = create.Invoke(position, x, z);
                }
            }
        }
    }
}