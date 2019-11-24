using System.Collections.Generic;
using BubblePopsC.Scripts.Services;
using Entitas;

namespace BubblePopsC.Scripts.Systems.Input
{
    public class ProcessTouchDownSystem : ReactiveSystem<InputEntity>
    {
        private readonly Contexts _contexts;

        public ProcessTouchDownSystem(Contexts contexts) : base(contexts.input)
        {
            _contexts = contexts;
        }

        protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
        {
            return context.CreateCollector(InputMatcher.TouchDown);
        }

        protected override bool Filter(InputEntity entity)
        {
            return entity.hasTouchDown;
        }

        protected override void Execute(List<InputEntity> entities)
        {
//            var inputEntity = entities.SingleEntity();
//            var input = inputEntity.touchDown;
//
//            var playArea = _contexts.game.playArea;
//            var validTrajectory = TrajectoryCalculatorService.GetTrajectory(playArea, input.Value, out var trajectory);
//
//            if (!validTrajectory) return;
//
//            _contexts.game.SetShootingTrajectory(trajectory);
        }
    }
}