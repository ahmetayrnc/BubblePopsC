using System;
using System.Collections.Generic;
using BubblePopsC.Scripts.Components;
using UnityEngine;

namespace BubblePopsC.Scripts.Services
{
    public static class TrajectoryCalculatorService
    {
        public static bool GetTrajectory(PlayAreaComponent playArea, Vector2 touchPos, out List<Vector3> trajectory)
        {
            var shooterPos = Contexts.sharedInstance.game.shooterPosition.Value;
            var trajectoryVector = touchPos - shooterPos;

            trajectory = new List<Vector3> {shooterPos};

            var doesIntersectLeftWall = GetLineIntersection(shooterPos, trajectoryVector * 20, playArea.LeftWallStart,
                playArea.LeftWallEnd, out var leftWallIntersection);
            var doesIntersectRightWall = GetLineIntersection(shooterPos, trajectoryVector * 20,
                playArea.RightWallStart,
                playArea.RightWallEnd, out var rightWallIntersection);
            var doesIntersectTopWall = GetLineIntersection(shooterPos, trajectoryVector * 20, playArea.LeftWallEnd,
                playArea.RightWallEnd, out var topWallIntersection);

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

            if (!doesIntersectLeftWall && !doesIntersectRightWall) return true;

            var newTrajectoryVector = trajectoryVector;
            newTrajectoryVector.x *= -1;
            var shouldAddThirdPoint = GetLineIntersection(trajectory[1], newTrajectoryVector * 20,
                playArea.LeftWallEnd,
                playArea.RightWallEnd, out var thirdPoint);

            if (!shouldAddThirdPoint) return false;

            trajectory.Add(thirdPoint);

            return true;
        }

        private static bool GetLineIntersection(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3,
            out Vector2 i)
        {
            i = Vector2.zero;

            var s1X = p1.x - p0.x;
            var s1Y = p1.y - p0.y;
            var s2X = p3.x - p2.x;
            var s2Y = p3.y - p2.y;

            var s = (-s1Y * (p0.x - p2.x) + s1X * (p0.y - p2.y)) / (-s2X * s1Y + s1X * s2Y);
            var t = (s2X * (p0.y - p2.y) - s2Y * (p0.x - p2.x)) / (-s2X * s1Y + s1X * s2Y);

            if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
            {
                // Collision detected
                i.x = p0.x + t * s1X;
                i.y = p0.y + t * s1Y;
                return true;
            }

            return false; // No collision
        }

        //Calculate the intersection point of two lines. Returns true if lines intersect, otherwise false.
        //Note that in 3d, two lines do not intersect most of the time. So if the two lines are not in the 
        //same plane, use ClosestPointsOnTwoLines() instead.
        private static bool LineLineIntersection(out Vector3 intersection, Vector3 linePoint1, Vector3 lineVec1,
            Vector3 linePoint2, Vector3 lineVec2)
        {
            var lineVec3 = linePoint2 - linePoint1;
            var crossVec1And2 = Vector3.Cross(lineVec1, lineVec2);
            var crossVec3And2 = Vector3.Cross(lineVec3, lineVec2);

            var planarFactor = Vector3.Dot(lineVec3, crossVec1And2);

            //is coplanar, and not parallel
            if (Mathf.Abs(planarFactor) < 0.0001f && crossVec1And2.sqrMagnitude > 0.0001f)
            {
                var s = Vector3.Dot(crossVec3And2, crossVec1And2) / crossVec1And2.sqrMagnitude;
                intersection = linePoint1 + (lineVec1 * s);
                return true;
            }

            intersection = Vector3.zero;
            return false;
        }

        private static bool OnSegment(Vector2 p, Vector2 q, Vector2 r)
        {
            return q.x <= Math.Max(p.x, r.x) && q.x >= Math.Min(p.x, r.x) &&
                   q.y <= Math.Max(p.y, r.y) && q.y >= Math.Min(p.y, r.y);
        }
    }
}