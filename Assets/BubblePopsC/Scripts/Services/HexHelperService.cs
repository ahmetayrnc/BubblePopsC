using System;
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