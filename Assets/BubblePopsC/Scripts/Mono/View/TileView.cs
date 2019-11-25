using BubblePopsC.Scripts.Services;
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
            spriteRenderer.sortingLayerName = TileLayer;
        }

        public void OnAxialCoord(GameEntity entity, int q, int r)
        {
            transform.position = HexHelperService.HexToPoint(q, r);
        }
    }
}