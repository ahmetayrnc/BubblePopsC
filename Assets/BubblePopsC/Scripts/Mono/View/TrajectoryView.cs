using System.Collections.Generic;
using UnityEngine;

namespace BubblePopsC.Scripts.Mono.View
{
    public class TrajectoryView : UniqueView, IAnyShootingTrajectoryListener, IAnyShootingTrajectoryRemovedListener
    {
        public LineRenderer lineRenderer;

        protected override void AddListeners()
        {
            var eventEntity = Contexts.sharedInstance.game.CreateEntity();
            eventEntity.AddAnyShootingTrajectoryListener(this);
            eventEntity.AddAnyShootingTrajectoryRemovedListener(this);
        }

        public void OnAnyShootingTrajectory(GameEntity entity, List<Vector3> points)
        {
            lineRenderer.enabled = true;
//            lineRenderer.SetPositions(new Vector3[0]);
            lineRenderer.positionCount = points.Count;
            for (var i = 0; i < points.Count; i++)
            {
                lineRenderer.SetPosition(i, points[i]);
            }

//            Debug.Log($"points: {points.Count}, lineRendererPositions: {lineRenderer.positionCount}");
        }

        public void OnAnyShootingTrajectoryRemoved(GameEntity entity)
        {
            lineRenderer.enabled = false;
        }
    }
}