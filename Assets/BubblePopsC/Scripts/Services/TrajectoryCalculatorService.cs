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
            if (!GetRoughTrajectory(playArea, touchPos, out trajectory))
            {
                return false;
            }

            var bubbles = Contexts.sharedInstance.game.GetGroup(
                GameMatcher.AllOf(GameMatcher.Bubble, GameMatcher.AxialCoord)).GetEntities();

            for (var i = 0; i < trajectory.Count - 1; i++)
            {
                if (!GetClosesLineCircleIntersection(out var closestIntersection, bubbles, trajectory[i],
                    trajectory[i + 1])) continue;
                trajectory[i + 1] = closestIntersection;
                var newTrajectory = new List<Vector3>();
                for (var j = 0; j <= i + 1; j++)
                {
                    newTrajectory.Add(trajectory[j]);
                }

                trajectory = newTrajectory;
                return true;
            }

            return true;

//            // check if the first line intersects any bubbles.
//            // If it intersects change trajectory and return.
//            if (GetClosesLineCircleIntersection(out var closestIntersection, bubbles, trajectory[0], trajectory[1]))
//            {
//                trajectory[1] = closestIntersection;
//                return true;
//            }

//            const float lineLength = 20f;
//            var bubbles = Contexts.sharedInstance.game.GetGroup(
//                GameMatcher.AllOf(GameMatcher.Bubble, GameMatcher.AxialCoord)).GetEntities();
//
//            var shooterPos = Contexts.sharedInstance.game.shooterPosition.Value;
//            trajectory = new List<Vector3> {shooterPos};
//
//            //check if a the line intersect any walls. If not return false
//            if (!GetFirstTrajectoryLine(touchPos, shooterPos, playArea, out var firstIntersection,
//                out var intersectionWall))
//            {
//                return false;
//            }
//
//            trajectory.Add(firstIntersection);
//
//            // check if the first line intersects any bubbles.
//            // If it intersects change trajectory and return.
//            if (GetClosesLineCircleIntersection(out var closestIntersection, bubbles, trajectory[0], trajectory[1]))
//            {
//                trajectory[1] = closestIntersection;
//                return true;
//            }
//
//            // if the line intersects the top wall stop there.
//            if (intersectionWall == IntersectionWall.Top) return true;
//
//            var trajectoryVector = touchPos - shooterPos;
//            trajectoryVector.x *= -1;
//            var hasTopIntersection = LineLineIntersection(trajectory[1], trajectoryVector * lineLength,
//                playArea.LeftWallEnd,
//                playArea.RightWallEnd, out var topIntersection);
//
//            // if the line is in such an angle that it does not intersect the top wall
//            // stop there.
//            if (!hasTopIntersection) return false;
//
//            trajectory.Add(topIntersection);
//
//            // check if the second line intersects any bubbles.
//            // If it intersects change trajectory and return.
//            if (GetClosesLineCircleIntersection(out var closestIntersection2, bubbles, trajectory[1], trajectory[2]))
//            {
//                trajectory[2] = closestIntersection2;
//            }
//
//            return true;
        }

        public static bool GetRoughTrajectory(PlayAreaComponent playArea, Vector2 touchPos,
            out List<Vector3> trajectory)
        {
            const float lineLength = 20f;

            var shooterPos = Contexts.sharedInstance.game.shooterPosition.Value;
            trajectory = new List<Vector3> {shooterPos};

            //check if a the line intersect any walls. If not return false
            if (!GetFirstTrajectoryLine(touchPos, shooterPos, playArea, out var firstIntersection,
                out var intersectionWall))
            {
                return false;
            }

            trajectory.Add(firstIntersection);

            // if the line intersects the top wall stop there.
            if (intersectionWall == IntersectionWall.Top) return true;

            var trajectoryVector = touchPos - shooterPos;
            trajectoryVector.x *= -1;
            var hasTopIntersection = LineLineIntersection(trajectory[1], trajectoryVector * lineLength,
                playArea.LeftWallEnd,
                playArea.RightWallEnd, out var topIntersection);

            // if the line is in such an angle that it does not intersect the top wall
            // stop there.
            if (!hasTopIntersection) return false;

            trajectory.Add(topIntersection);

            return true;
        }

        private enum IntersectionWall
        {
            Left,
            Right,
            Top
        }

        private static bool GetFirstTrajectoryLine(Vector2 touchPos, Vector2 shooterPos, PlayAreaComponent playArea,
            out Vector2 intersection, out IntersectionWall intersectionWall)
        {
            var trajectoryVector = touchPos - shooterPos;
            var doesIntersectLeftWall = LineLineIntersection(shooterPos, trajectoryVector * 20,
                playArea.LeftWallStart, playArea.LeftWallEnd, out var leftWallIntersection);
            var doesIntersectRightWall = LineLineIntersection(shooterPos, trajectoryVector * 20,
                playArea.RightWallStart, playArea.RightWallEnd, out var rightWallIntersection);
            var doesIntersectTopWall = LineLineIntersection(shooterPos, trajectoryVector * 20,
                playArea.LeftWallEnd, playArea.RightWallEnd, out var topWallIntersection);

            if (doesIntersectRightWall)
            {
                intersection = rightWallIntersection;
                intersectionWall = IntersectionWall.Right;
                return true;
            }

            if (doesIntersectLeftWall)
            {
                intersection = leftWallIntersection;
                intersectionWall = IntersectionWall.Left;
                return true;
            }

            if (doesIntersectTopWall)
            {
                intersection = topWallIntersection;
                intersectionWall = IntersectionWall.Top;
                return true;
            }

            intersection = Vector2.down;
            intersectionWall = IntersectionWall.Right;
            return false;
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

        private static bool GetClosesLineCircleIntersection(out Vector2 closestIntersection,
            IEnumerable<GameEntity> bubbles, Vector2 lp1, Vector2 lp2)
        {
            closestIntersection = lp2;
            var found = false;
            foreach (var bubble in bubbles)
            {
                var bubbleAxialCoord = bubble.axialCoord;
                var bubbleRegularCoord = HexHelperService.HexToPoint(bubbleAxialCoord.Q, bubbleAxialCoord.R);

                if (!LineCircleIntersection(bubbleRegularCoord, 0.4f, lp1, lp2,
                    out var intersection)) continue;

                if (!(Vector2.Distance(intersection, lp1) < Vector2.Distance(closestIntersection, lp1)))
                    continue;

                found = true;
                closestIntersection = intersection;
            }

            return found;
        }

        private static bool LineCircleIntersection(Vector2 c, float radius, Vector2 lineStart, Vector2 lineEnd,
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