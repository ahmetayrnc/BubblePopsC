using System.Collections.Generic;
using BubblePopsC.Scripts.Services;
using Entitas;
using UnityEngine;

namespace BubblePopsC.Scripts.Systems.Input
{
    public class ProcessTouchUpSystem : ReactiveSystem<InputEntity>
    {
        private readonly Contexts _contexts;

        public ProcessTouchUpSystem(Contexts contexts) : base(contexts.input)
        {
            _contexts = contexts;
        }

        protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
        {
            return context.CreateCollector(InputMatcher.TouchUp);
        }

        protected override bool Filter(InputEntity entity)
        {
            return entity.hasTouchUp;
        }

        protected override void Execute(List<InputEntity> entities)
        {
            var ghostBubble = _contexts.game.GetGroup(GameMatcher.Ghost).GetSingleEntity();

            if (ghostBubble == null)
            {
                return;
            }

            var trajectory = _contexts.game.shootingTrajectory.Points;
            ShootBall(trajectory, ghostBubble);
        }

        private void ShootBall(List<Vector3> trajectory, GameEntity ghostBubble)
        {
            var finalAxialPos = ghostBubble.axialCoord;
            var finalPos = HexHelperService.HexToPoint(finalAxialPos.Q, finalAxialPos.R);
            trajectory[trajectory.Count - 1] = finalPos;

            var willBeShoutNextBubble = _contexts.game.GetGroup(GameMatcher.WillBeShotNext).GetSingleEntity();
            var bubbleId = willBeShoutNextBubble.id.Value;
            willBeShoutNextBubble.AddShot(trajectory.ToArray(), () =>
            {
                Debug.Log("Done");

                var bubble = _contexts.game.GetEntityWithId(bubbleId);
                if (bubble == null) return;

                bubble.RemoveShot();
                bubble.isWillBeShotNext = false;
                bubble.RemovePosition();
                bubble.AddAxialCoord(finalAxialPos.Q, finalAxialPos.R);
            });

            ghostBubble.isDestroyed = true;
            _contexts.game.RemoveShootingTrajectory();
        }
    }
}