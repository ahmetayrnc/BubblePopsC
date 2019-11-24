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

            if (_contexts.game.hasShootingTrajectory)
            {
                if (!validTrajectory)
                {
                    _contexts.game.RemoveShootingTrajectory();
                    return;
                }

                _contexts.game.ReplaceShootingTrajectory(trajectory);
            }
            else
            {
                if (!validTrajectory) return;

                _contexts.game.SetShootingTrajectory(trajectory);
            }
        }
    }
}