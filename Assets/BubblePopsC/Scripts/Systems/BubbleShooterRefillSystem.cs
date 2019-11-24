using System.Collections.Generic;
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
            throw new System.NotImplementedException();
        }

        protected override bool Filter(GameEntity entity)
        {
            throw new System.NotImplementedException();
        }

        protected override void Execute(List<GameEntity> entities)
        {
            throw new System.NotImplementedException();
        }
    }
}