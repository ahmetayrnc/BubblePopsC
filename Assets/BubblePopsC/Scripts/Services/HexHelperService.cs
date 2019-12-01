using System;
using System.Collections.Generic;
using System.Linq;
using BubblePopsC.Scripts.Components.Position;
using UnityEngine;

namespace BubblePopsC.Scripts.Services
{
    public static class HexHelperService
    {
        private static readonly AxialCoord[] AxialDirections =
        {
            new AxialCoord {Q = 0, R = 1},
            new AxialCoord {Q = +1, R = 0},
            new AxialCoord {Q = +1, R = -1},
            new AxialCoord {Q = 0, R = -1},
            new AxialCoord {Q = -1, R = 0},
            new AxialCoord {Q = -1, R = 1},
        };

        public static Vector2 HexToPoint(AxialCoord hex)
        {
            var offset = Contexts.sharedInstance.game.boardOffset.Value;
            const float size = 0.5f;
            var x = size * (Mathf.Sqrt(3f) * hex.Q + Mathf.Sqrt(3f) / 2f * hex.R);
            var y = size * (3f / 2 * hex.R) + offset;
            return new Vector2(x, y);
        }

        public static AxialCoord PointToHex(Vector2 point)
        {
            var offset = Contexts.sharedInstance.game.boardOffset.Value;
            point.y -= offset;

            const float size = 0.5f;
            var q = (Mathf.Sqrt(3f) / 3f * point.x - 1f / 3 * point.y) / size;
            var r = (2f / 3 * point.y) / size;
            return HexRound(new Vector2(q, r));
        }

        public static AxialCoord FindNearestNeighbour(AxialCoord hex, Vector2 point)
        {
            var line = (point - HexToPoint(hex)).normalized;
            var angle = Vector2.SignedAngle(Vector2.down, line);
            angle += -180f;
            angle *= -1f;
            var directionIndex = (int) angle / 60;

            var neighbourDirection = AxialDirections[directionIndex];
            var neighbourAxialCoord = new AxialCoord
            {
                Q = neighbourDirection.Q + hex.Q,
                R = neighbourDirection.R + hex.R
            };

            return neighbourAxialCoord;
        }

        public static List<AxialCoord> GetNeighbours(AxialCoord hex)
        {
            return AxialDirections.Select(axialDirection =>
                new AxialCoord {Q = axialDirection.Q + hex.Q, R = axialDirection.R + hex.R}).ToList();
        }

        public static Vector2Int GetArrayIndices(AxialCoord axialCoord)
        {
            return new Vector2Int(axialCoord.Q + Mathf.FloorToInt(axialCoord.R / 2f), axialCoord.R);
        }

        #region internalMethods

        private static AxialCoord HexRound(Vector2 hex)
        {
            var cubeCoordinates = AxialToCube(hex);
            var roundedCubeCoordinates = CubeRound(cubeCoordinates);
            var roundedAxialCoordinates = CubeToAxial(roundedCubeCoordinates);
            return roundedAxialCoordinates;
        }

        private static AxialCoord CubeToAxial(Vector3Int cube)
        {
            var q = cube.x;
            var r = cube.z;
            return new AxialCoord {Q = q, R = r};
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

        #endregion
    }
}