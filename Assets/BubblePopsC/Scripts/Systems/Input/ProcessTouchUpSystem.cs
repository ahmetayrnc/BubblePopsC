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
            var shotBubble = _contexts.game.GetGroup(GameMatcher.Shot).GetSingleEntity();
            if (shotBubble != null) return;

            var finalAxialPos = ghostBubble.axialCoord.Value;
            var finalPos = HexHelperService.HexToPoint(finalAxialPos);
            trajectory[trajectory.Count - 1] = finalPos;

            var shooterBubble = _contexts.game.GetGroup(GameMatcher.WillBeShotNext).GetSingleEntity();
            var bubbleId = shooterBubble.id.Value;
            shooterBubble.AddShot(trajectory.ToArray(), () =>
            {
                var bubble = _contexts.game.GetEntityWithId(bubbleId);
                if (bubble == null) return;

                bubble.RemoveShot();
                bubble.isWillBeShotNext = false;
                bubble.RemovePosition();
                bubble.AddAxialCoord(finalAxialPos);
                bubble.isMergeDirty = true;
            });

            ghostBubble.isDestroyed = true;
            _contexts.game.RemoveShootingTrajectory();
        }
    }
}