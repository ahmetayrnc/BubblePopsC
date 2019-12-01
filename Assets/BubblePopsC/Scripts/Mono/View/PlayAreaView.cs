using UnityEngine;

namespace BubblePopsC.Scripts.Mono.View
{
    public class PlayAreaView : View
    {
        public SpriteRenderer spriteRenderer;
        public Transform colliderParent;

        protected override void AddListeners(GameEntity entity)
        {
            spriteRenderer.sortingLayerName = PlayAreaLayer;
        }

        protected override void InitializeView(GameEntity entity)
        {
            spriteRenderer.sortingLayerName = PlayAreaLayer;

            var playArea = entity.playArea;
            transform.position = playArea.Center;
            spriteRenderer.size = playArea.Size;
            colliderParent.localScale = playArea.Size;
        }
    }
}