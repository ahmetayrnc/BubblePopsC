using System;
using System.Collections.Generic;
using BubblePopsC.Scripts.Components;
using BubblePopsC.Scripts.Components.Position;
using UnityEngine;

namespace BubblePopsC.Scripts.Services
{
    public static class TrajectoryCalculatorService
    {
        public static bool GetTrajectory(PlayAreaComponent playArea, Vector2 touchPos, out List<Vector3> trajectory)
        {
            var bubbles = Contexts.sharedInstance.game.GetGroup(
                GameMatcher.AllOf(GameMatcher.Bubble, GameMatcher.AxialCoord)).GetEntities();

            var shooterPos = Contexts.sharedInstance.game.shooterPosition.Value;
            trajectory = new List<Vector3> {shooterPos};

            var trajectoryVector = touchPos - shooterPos;
            var doesIntersectLeftWall = LineLineIntersection(shooterPos, trajectoryVector * 20,
                playArea.LeftWallStart, playArea.LeftWallEnd, out var leftWallIntersection);
            var doesIntersectRightWall = LineLineIntersection(shooterPos, trajectoryVector * 20,
                playArea.RightWallStart, playArea.RightWallEnd, out var rightWallIntersection);
            var doesIntersectTopWall = LineLineIntersection(shooterPos, trajectoryVector * 20,
                playArea.LeftWallEnd, playArea.RightWallEnd, out var topWallIntersection);

            if (doesIntersectRightWall)
            {
                trajectory.Add(rightWallIntersection);
            }
            else if (doesIntersectLeftWall)
            {
                trajectory.Add(leftWallIntersection);
            }
            else if (doesIntersectTopWall)
            {
                trajectory.Add(topWallIntersection);
            }

            if (trajectory.Count <= 1) return false;

            // 1 line
            foreach (var bubble in bubbles)
            {
                var bubbleAxialCoord = bubble.axialCoord;
            }


            if (!doesIntersectLeftWall && !doesIntersectRightWall) return true;

            var newTrajectoryVector = trajectoryVector;
            newTrajectoryVector.x *= -1;
            var shouldAddThirdPoint = LineLineIntersection(trajectory[1], newTrajectoryVector * 20,
                playArea.LeftWallEnd,
                playArea.RightWallEnd, out var thirdPoint);

            if (!shouldAddThirdPoint) return false;

            trajectory.Add(thirdPoint);

            return true;
        }

        private static bool LineLineIntersection(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, out Vector2 i)
        {
            i = Vector2.zero;

            var s1 = p1 - p0;
            var s2 = p3 - p2;

            var s = (-s1.y * (p0.x - p2.x) + s1.x * (p0.y - p2.y)) / (-s2.x * s1.y + s1.x * s2.y);
            var t = (s2.x * (p0.y - p2.y) - s2.y * (p0.x - p2.x)) / (-s2.x * s1.y + s1.x * s2.y);

            if (!(s >= 0) || !(s <= 1) || !(t >= 0) || !(t <= 1)) return false;

            i.x = p0.x + t * s1.x;
            i.y = p0.y + t * s1.y;
            return true;
        }

        public static bool LineCircleIntersection(Vector2 c, float radius, Vector2 lineStart, Vector2 lineEnd,
            out Vector2 intersection)
        {
            var intersections = FindLineCircleIntersections(c, radius, lineStart, lineEnd,
                out var intersection1, out var intersection2);

            if (intersections == 1)
            {
                intersection = intersection1;
                return true;
            }

            if (intersections == 2)
            {
                double dist1 = Vector2.Distance(intersection1, lineStart);
                double dist2 = Vector2.Distance(intersection2, lineStart);

                intersection = dist1 < dist2 ? intersection1 : intersection2;
                return true;
            }

            intersection = Vector2.zero;
            return false;
        }

        private static int FindLineCircleIntersections(Vector2 center, float radius,
            Vector2 point1, Vector2 point2,
            out Vector2 intersection1, out Vector2 intersection2)
        {
            float t;

            var dx = point2.x - point1.x;
            var dy = point2.y - point1.y;

            var a = dx * dx + dy * dy;
            var b = 2 * (dx * (point1.x - center.x) + dy * (point1.y - center.y));
            var c = (point1.x - center.x) * (point1.x - center.x) + (point1.y - center.y) * (point1.y - center.y) -
                    radius * radius;

            var det = b * b - 4 * a * c;
            if ((a <= 0.0000001) || (det < 0))
            {
                // No real solutions.
                intersection1 = new Vector2(float.NaN, float.NaN);
                intersection2 = new Vector2(float.NaN, float.NaN);
                return 0;
            }

            if (Math.Abs(det) < 0.001f)
            {
                // One solution.
                t = -b / (2 * a);
                intersection1 = new Vector2(point1.x + t * dx, point1.y + t * dy);
                intersection2 = new Vector2(float.NaN, float.NaN);
                return 1;
            }

            // Two solutions.
            t = (float) ((-b + Math.Sqrt(det)) / (2 * a));
            intersection1 = new Vector2(point1.x + t * dx, point1.y + t * dy);
            t = (float) ((-b - Math.Sqrt(det)) / (2 * a));
            intersection2 = new Vector2(point1.x + t * dx, point1.y + t * dy);
            return 2;
        }
    }
}