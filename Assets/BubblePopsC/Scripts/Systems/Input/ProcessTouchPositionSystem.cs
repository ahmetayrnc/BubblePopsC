using System.Collections.Generic;
using BubblePopsC.Scripts.Mono.Collision;
using BubblePopsC.Scripts.Services;
using Entitas;
using UnityEngine;

namespace BubblePopsC.Scripts.Systems.Input
{
    public class ProcessTouchPositionSystem : ReactiveSystem<InputEntity>
    {
        private readonly Contexts _contexts;

        public ProcessTouchPositionSystem(Contexts contexts) : base(contexts.input)
        {
            _contexts = contexts;
        }

        protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
        {
            return context.CreateCollector(InputMatcher.TouchPosition);
        }

        protected override bool Filter(InputEntity entity)
        {
            return entity.hasTouchPosition;
        }

        protected override void Execute(List<InputEntity> entities)
        {
            var inputEntity = entities.SingleEntity();
            var touchPos = inputEntity.touchPosition.Value;
            var shooterPos = _contexts.game.shooterPosition.Value;

            var validTrajectory = CollisionFinder.GetTrajectory(shooterPos, touchPos, out var trajectory);
            var validEndPoint = CollisionFinder.GetRealCollisionPoint(shooterPos, touchPos, out var endPoint);

            UpdateTrajectory(validTrajectory, trajectory);
            UpdateGhostBubble(validEndPoint, endPoint);
        }

        private void UpdateGhostBubble(bool validTrajectory, Vector3 endPoint)
        {
            var ghostBubble = _contexts.game.GetGroup(GameMatcher.Ghost).GetSingleEntity();

            if (ghostBubble == null)
            {
                ghostBubble = BubbleCreatorService.CreateGhostBubble();
                ghostBubble.AddAxialCoord(0, 0);
            }

            if (!validTrajectory)
            {
                ghostBubble.isDestroyed = true;
                return;
            }

            var hex = HexHelperService.PointToHex(endPoint);
            var neighbourAxialCoord =
                HexHelperService.FindNearestNeighbour(hex.x, hex.y, endPoint);

            ghostBubble.ReplaceAxialCoord(neighbourAxialCoord.x, neighbourAxialCoord.y);
        }

        private void UpdateTrajectory(bool valid, List<Vector3> trajectory)
        {
            if (!valid)
            {
                if (_contexts.game.hasShootingTrajectory)
                {
                    _contexts.game.RemoveShootingTrajectory();
                }

                return;
            }

            if (_contexts.game.hasShootingTrajectory)
            {
                _contexts.game.ReplaceShootingTrajectory(trajectory);
            }
            else
            {
                _contexts.game.SetShootingTrajectory(trajectory);
            }
        }
    }
}