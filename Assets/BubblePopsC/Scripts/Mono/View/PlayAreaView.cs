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

            var boardSize = Contexts.sharedInstance.game.boardSize;
            var boardWidth = boardSize.Value.x;
            var boardHeight = boardSize.Value.y;

            var transform1 = transform;

            var newPos = transform1.position;
            newPos.x = boardWidth / 2f - 0.25f;
            newPos.y = 0.39f * boardHeight;
            transform1.position = newPos;

            var newScale = transform1.localScale;
            newScale.x = boardWidth - 0.5f;
            newScale.y = boardHeight * 0.88f;
            transform1.localScale = newScale;
        }
    }
}