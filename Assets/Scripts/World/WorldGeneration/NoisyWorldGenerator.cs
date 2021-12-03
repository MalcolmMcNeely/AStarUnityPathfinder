using System;
using Grid;
using UnityEngine;

namespace World.WorldGeneration
{
    public class NoisyWorldGenerator : IWorldGenerator
    {
        const float Width = 256;
        const float Height = 256;
        const float Scale = 400f;

        public void Generate(GridUnit[,] worldGrid, Vector3 gridUnitSize, int sizeX, int sizeZ, Func<Vector3, int, int, GridUnit> create)
        {
            for(var x = 0; x < sizeX; x++)
            {
                for (var z = 0; z < sizeZ; z++)
                {
                    var position = new Vector3(gridUnitSize.x + x * gridUnitSize.x, CalculateHeight(x, z), gridUnitSize.z + z * gridUnitSize.z);

                    worldGrid[x, z] = create.Invoke(position, x, z);
                }
            }
        }

        float CalculateHeight(int x, int y)
        {
            var xCoord = x / Width * Scale;
            var yCoord = y / Height * Scale;

            return Mathf.PerlinNoise(xCoord, yCoord);
        }
    }
}