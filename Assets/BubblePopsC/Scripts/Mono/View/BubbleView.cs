using BubblePopsC.Scripts.Services;
using UnityEngine;

namespace BubblePopsC.Scripts.Mono.View
{
    public class BubbleView : View, IAxialCoordListener, IPositionListener
    {
        public SpriteRenderer spriteRenderer;

        protected override void AddListeners(GameEntity entity)
        {
            entity.AddAxialCoordListener(this);
            entity.AddPositionListener(this);
        }

        protected override void InitializeView(GameEntity entity)
        {
            spriteRenderer.sortingLayerName = BubbleLayer;
        }

        public void OnAxialCoord(GameEntity entity, int q, int r)
        {
            transform.position = HexHelperService.HexToPoint(q, r);
        }

        public void OnPosition(GameEntity entity, Vector2 value)
        {
            transform.position = value;
        }
    }
}