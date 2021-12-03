using System;
using Grid;
using UnityEngine;

namespace World.WorldGeneration
{
    public interface IWorldGenerator
    {
        void Generate(GridUnit[,] worldGrid, Vector3 gridUnitSize, int sizeX, int sizeZ, Func<Vector3, int, int, GridUnit> create);
    }
}