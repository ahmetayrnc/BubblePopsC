using System.Collections.Generic;
using BubblePopsC.Scripts.Components.Bubble;
using BubblePopsC.Scripts.Services;
using Entitas;

namespace BubblePopsC.Scripts.Systems
{
    public class BubbleShooterRefillSystem : ReactiveSystem<GameEntity>
    {
        private Contexts _contexts;

        public BubbleShooterRefillSystem(Contexts contexts) : base(contexts.game)
        {
            _contexts = contexts;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.WillBeShotNext.Removed());
        }

        protected override bool Filter(GameEntity entity)
        {
            return !entity.isWillBeShotNext;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            var spareBubble = _contexts.game.GetGroup(GameMatcher.SpareBubble).GetSingleEntity();
            spareBubble.isSpareBubble = false;
            spareBubble.isWillBeShotNext = true;
            spareBubble.isMoveToShooter = true;
            BubbleCreatorService.CreateSpareBubble();
        }
    }
}