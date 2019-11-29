using System.Collections.Generic;
using Entitas;

namespace BubblePopsC.Scripts.Systems
{
    public class MergeSystem : ReactiveSystem<GameEntity>
    {
        public MergeSystem(Contexts contexts) : base(contexts.game)
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
            var lastPutBubble = entities[0];
        }
    }
}