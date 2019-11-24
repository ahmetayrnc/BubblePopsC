using UnityEngine;

namespace BubblePopsC.Scripts.Mono.View
{
    public class PlayAreaView : View
    {
        public SpriteRenderer spriteRenderer;

        protected override void AddListeners(GameEntity entity)
        {
            spriteRenderer.sortingLayerName = PlayAreaLayer;
        }

        protected override void InitializeView(GameEntity entity)
        {
            spriteRenderer.sortingLayerName = PlayAreaLayer;

            var playArea = entity.playArea;
            var transform1 = transform;
            transform1.position = playArea.Center;
            transform1.localScale = playArea.Size;
        }
    }
}