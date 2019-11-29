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

        public static Vector2Int PointToHex(Vector2 point)
        {
            var size = 0.5f;
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

//            Debug.Log($"angle: {angle}, index: {directionIndex}");
            var neighbourDirection = axialDirections[directionIndex];
            var neighbourAxialCoord = new Vector2Int(q, r)
            {
                x = neighbourDirection.x += q,
                y = neighbourDirection.y += r
            };

            return neighbourAxialCoord;
        }

        private static double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        private static Vector2Int HexRound(Vector2 hex)
        {
            var axial = CubeToAxial(CubeRound(AxialToCube(hex)));
            return new Vector2Int((int) axial.x, (int) axial.y);
        }

        private static Vector2 CubeToAxial(Vector3 cube)
        {
            var q = cube.x;
            var r = cube.z;
            return new Vector2(q, r);
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
            int rx = Convert.ToInt32(cube.x);
            var ry = Convert.ToInt32(cube.y);
            var rz = Convert.ToInt32(cube.z);

            var xDiff = Math.Abs(rx - cube.x);
            var yDiff = Math.Abs(ry - cube.y);
            var zDiff = Math.Abs(rz - cube.z);

            if (xDiff > yDiff && xDiff > zDiff)
            {
                rx = -ry - rz;
            }
            else if (yDiff > zDiff)
            {
                ry = -rx - rz;
            }
            else
            {
                rz = -rx - ry;
            }

            return new Vector3Int(rx, ry, rz);
        }
    }
}