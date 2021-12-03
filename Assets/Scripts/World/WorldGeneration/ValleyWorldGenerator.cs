using System;
using Grid;
using UnityEngine;

namespace World.WorldGeneration
{
    public class ValleyWorldGenerator : IWorldGenerator
    {
        const float MinHeight = 0f;
        const float HeightMultiplier = 0.05f;
        float[] _dataX;
        int[] _dataOffsets;

        public void Generate(GridUnit[,] worldGrid, Vector3 gridUnitSize, int sizeX, int sizeZ, Func<Vector3, int, int, GridUnit> create)
        {
            _dataX = CreateHeightDataX(sizeX);
            _dataOffsets = CreateHeightDataOffset(sizeZ, sizeX);

            for (var z = 0; z < sizeZ; z++)
            {
                var heightsForRow = GetHeightsForRow(z, sizeX);

                for (var x = 0; x < sizeX; x++)
                {
                    var position = new Vector3(gridUnitSize.x + x * gridUnitSize.x, heightsForRow[x], gridUnitSize.z + z * gridUnitSize.z);

                    worldGrid[x, z] = create.Invoke(position, x, z);
                }
            }
        }

        int[] CreateHeightDataOffset(int sizeZ, int sizeX)
        {
            var data = new int[sizeZ];
            var secondPointX = (int)(sizeX * 0.5);

            for (var i = 0; i < sizeZ; i++)
            {
                data[i] = (int) Mathf.PingPong(i, secondPointX);
            }

            return data;
        }

        float[] CreateHeightDataX(int sizeX)
        {
            var data = new float[sizeX];
            var midpoint = (int)(sizeX * 0.5);
            var quarterPoint = (int)(midpoint * 0.5);
            var threeQuarterPoint = midpoint + quarterPoint;
            var maxHeight = sizeX * HeightMultiplier;

            for (var i = 0; i < sizeX; i++)
            {
                if (i <= quarterPoint)
                {
                    var normal = Normalise(i, quarterPoint, 0);
                    data[i] = Mathf.Lerp(MinHeight, maxHeight, normal);
                }
                else if (i <= midpoint)
                {
                    var normal = Normalise(i, midpoint, quarterPoint);
                    data[i] = Mathf.Lerp(maxHeight, MinHeight, normal);
                }
                else if (i <= threeQuarterPoint)
                {
                    var normal = Normalise(i, threeQuarterPoint, midpoint);
                    data[i] = Mathf.Lerp(MinHeight, maxHeight, normal);
                }
                else
                {
                    var normal = Normalise(i, sizeX, threeQuarterPoint);
                    data[i] = Mathf.Lerp(maxHeight, MinHeight, normal);
                }
            }

            return data;
        }

        float Normalise(float val, float max, float min)
        {
            return (val - min) / (max - min);
        }

        float[] GetHeightsForRow(int z, int sizeX)
        {
            var output = new float[sizeX];
            var offset = _dataOffsets[z];

            for (var i = 0; i < sizeX; i++)
            {
                output[i] = _dataX[WrapIndex(i + offset, sizeX)];
            }

            return output;
        }

        int WrapIndex(int index, int maxSize)
        {
            return Math.Abs((index + maxSize) % maxSize);
        }
    }
}