using System;
using BubblePopsC.Scripts.Components.Position;
using UnityEngine;

namespace BubblePopsC.Scripts.Services
{
    public static class HexHelperService
    {
        public static Vector2 HexToPoint(int q, int r)
        {
            const float size = 0.5f;
            var x = size * (Mathf.Sqrt(3f) * q + Mathf.Sqrt(3f) / 2f * r);
            var y = size * (3f / 2 * r);
            return new Vector2(x, y);
        }

        public static AxialCoordComponent PointToHex(Vector2 point)
        {
            const float size = 0.5f;
            var q = (Mathf.Sqrt(3f) / 3f * point.x - 1f / 3 * point.y) / size;
            var r = (2f / 3 * point.y) / size;
            return HexRound(new Vector2(q, r));
        }

        public static Vector2Int FindNearestNeighbour(int q, int r, Vector2 point)
        {
            var axialDirections = new[]
            {
                new Vector2Int {x = 0, y = 1},
                new Vector2Int {x = +1, y = 0},
                new Vector2Int {x = +1, y = -1},
                new Vector2Int {x = 0, y = -1},
                new Vector2Int {x = -1, y = 0},
                new Vector2Int {x = -1, y = 1},
            };

            var line = (point - HexToPoint(q, r)).normalized;
            var angle = Vector2.SignedAngle(Vector2.down, line);
            angle += -180f;
            angle *= -1f;
            var directionIndex = (int) angle / 60;

            var neighbourDirection = axialDirections[directionIndex];
            var neighbourAxialCoord = new Vector2Int(q, r)
            {
                x = neighbourDirection.x += q,
                y = neighbourDirection.y += r
            };

            return neighbourAxialCoord;
        }

        private static AxialCoordComponent HexRound(Vector2 hex)
        {
            var cubeCoordinates = AxialToCube(hex);
            var roundedCubeCoordinates = CubeRound(cubeCoordinates);
            var roundedAxialCoordinates = CubeToAxial(roundedCubeCoordinates);
            return roundedAxialCoordinates;
        }

        private static AxialCoordComponent CubeToAxial(Vector3Int cube)
        {
            var q = cube.x;
            var r = cube.z;
            return new AxialCoordComponent {Q = q, R = r};
        }

        private static Vector3 AxialToCube(Vector2 hex)
        {
            var x = hex.x;
            var z = hex.y;

            var y = -x - z;
            return new Vector3(x, y, z);
        }

        private static Vector3Int CubeRound(Vector3 cube)
        {
            var q = (int) Math.Round(cube.x);
            var r = (int) Math.Round(cube.y);
            var s = (int) Math.Round(cube.z);

            var qDiff = Math.Abs(q - cube.x);
            var rDiff = Math.Abs(r - cube.y);
            var sDiff = Math.Abs(s - cube.z);

            if (qDiff > rDiff && qDiff > sDiff)
            {
                q = -r - s;
            }
            else if (rDiff > sDiff)
            {
                r = -q - s;
            }
            else
            {
                s = -q - r;
            }

            return new Vector3Int(q, r, s);
        }
    }
}