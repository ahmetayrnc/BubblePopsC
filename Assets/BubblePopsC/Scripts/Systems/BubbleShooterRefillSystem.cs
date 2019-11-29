using System.Collections.Generic;
using BubblePopsC.Scripts.Components.Bubble;
using BubblePopsC.Scripts.Services;
using Entitas;

namespace BubblePopsC.Scripts.Systems
{
    public class BubbleShooterRefillSystem : ReactiveSystem<GameEntity>
    {
        public BubbleShooterRefillSystem(Contexts contexts) : base(contexts.game)
        {
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
            BubbleCreatorService.CreateWillBeShotNextBubble();
        }
    }
}