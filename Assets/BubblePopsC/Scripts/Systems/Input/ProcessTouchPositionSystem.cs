using System.Collections.Generic;
using System.Linq;
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
            var input = inputEntity.touchPosition;

            var playArea = _contexts.game.playArea;
            var validTrajectory = TrajectoryCalculatorService.GetTrajectory(playArea, input.Value, out var trajectory);
            UpdateTrajectory(validTrajectory, trajectory);
            UpdateGhostBubble(validTrajectory, trajectory);
        }

        private void UpdateGhostBubble(bool validTrajectory, List<Vector3> trajectory)
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

            var hex = HexHelperService.PointToHex(trajectory[trajectory.Count - 1]);
            ghostBubble.ReplaceAxialCoord(hex.x, hex.y);
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