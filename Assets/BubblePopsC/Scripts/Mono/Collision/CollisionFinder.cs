using System.Collections.Generic;
using UnityEngine;

namespace BubblePopsC.Scripts.Mono.Collision
{
    public static class CollisionFinder
    {
        public static bool GetTrajectory(Vector2 shooterPos, Vector2 touchPos, out List<Vector3> trajectory)
        {
            const float distance = 200f;
            const string layerName = "VisualCollision";
            const string bubbleTag = "Bubble";
            var origin = shooterPos;
            var direction = (touchPos - shooterPos).normalized;

            trajectory = new List<Vector3> {shooterPos};

            for (var i = 0; i < 4; i++)
            {
                var rayHit = Physics2D.Raycast(origin, direction, distance, LayerMask.GetMask(layerName));

                if (rayHit.collider == null) break;

                trajectory.Add(rayHit.point);
                direction.x *= -1f;
                origin = rayHit.point + direction * 0.001f;

                if (rayHit.collider.CompareTag(bubbleTag)) break;
            }

            return trajectory.Count > 1 && trajectory.Count < 4;
        }

        public static bool GetRealCollisionPoint(Vector2 shooterPos, Vector2 touchPos, out Vector3 endCollisionPoint)
        {
            const float distance = 200f;
            const string layerName = "RealCollision";
            const string bubbleTag = "Bubble";
            var origin = shooterPos;
            var direction = (touchPos - shooterPos).normalized;

            endCollisionPoint = shooterPos;

            for (var i = 0; i < 4; i++)
            {
                var rayHit = Physics2D.Raycast(origin, direction, distance, LayerMask.GetMask(layerName));

                if (rayHit.collider == null) break;

                direction.x *= -1f;
                origin = rayHit.point + direction * 0.001f;

                if (!rayHit.collider.CompareTag(bubbleTag)) continue;

                endCollisionPoint = rayHit.point + direction * 0.001f;
                return true;
            }

            return false;
        }
    }
}