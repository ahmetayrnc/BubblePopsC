using System.Collections.Generic;
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
            if (_contexts.game.hasShootingTrajectory) ShootBall();
        }

        private void ShootBall()
        {
            _contexts.game.RemoveShootingTrajectory();
            //Debug.Log("Shoot");
        }
    }
}