using UnityEngine;

namespace BubblePopsC.Scripts.Mono.View
{
    public class TileView : View, IAxialCoordListener
    {
        public SpriteRenderer spriteRenderer;

        protected override void AddListeners(GameEntity entity)
        {
            entity.AddAxialCoordListener(this);
        }

        protected override void InitializeView(GameEntity entity)
        {
//            spriteRenderer.sortingLayerName = TileLayer;
        }

        public void OnAxialCoord(GameEntity entity, int q, int r)
        {
            transform.position = HexTo(q, r);
        }

        private static Vector2 HexTo(int q, int r)
        {
            var x = 1f / Mathf.Sqrt(3f) * (Mathf.Sqrt(3f) * q + Mathf.Sqrt(3f) / 2f * r);
            var y = 1f / Mathf.Sqrt(3f) * (3f / 2 * r);
            return new Vector2(x, y);
        }
    }
}