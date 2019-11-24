using Entitas;

namespace BubblePopsC.Scripts.Systems.Input
{
    public class CleanupInputSystem : ICleanupSystem
    {
        private readonly IGroup<InputEntity> _touchDownGroup;
        private readonly IGroup<InputEntity> _touchUpGroup;
        private readonly IGroup<InputEntity> _touchPositionGroup;

        public CleanupInputSystem(Contexts contexts)
        {
            _touchDownGroup = contexts.input.GetGroup(InputMatcher.TouchDown);
            _touchUpGroup = contexts.input.GetGroup(InputMatcher.TouchUp);
            _touchPositionGroup = contexts.input.GetGroup(InputMatcher.TouchPosition);
        }

        public void Cleanup()
        {
            if (_touchDownGroup.count > 0)
            {
                foreach (var touchDown in _touchDownGroup.GetEntities())
                {
                    touchDown.Destroy();
                }
            }

            if (_touchUpGroup.count > 0)
            {
                foreach (var touchUp in _touchUpGroup.GetEntities())
                {
                    touchUp.Destroy();
                }

                Contexts.sharedInstance.input.RemoveTouchPosition();
            }
        }
    }
}