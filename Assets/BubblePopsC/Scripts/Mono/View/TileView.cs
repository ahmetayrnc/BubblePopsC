using BubblePopsC.Scripts.Components.Position;
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

        public void OnAxialCoord(GameEntity entity, AxialCoord hex)
        {
            transform.position = HexHelperService.HexToPoint(hex);
        }
    }
}