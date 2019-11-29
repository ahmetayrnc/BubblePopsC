using BubblePopsC.Scripts.Services;
using UnityEngine;

namespace BubblePopsC.Scripts.Mono.View
{
    public class BubbleView : View, IAxialCoordListener, IPositionListener
    {
        public SpriteRenderer spriteRenderer;
        public CircleCollider2D visualCollider;
        public PolygonCollider2D realCollider;

        protected override void AddListeners(GameEntity entity)
        {
            entity.AddAxialCoordListener(this);
            entity.AddPositionListener(this);
        }

        protected override void InitializeView(GameEntity entity)
        {
            spriteRenderer.sortingLayerName = BubbleLayer;

            if (entity.isGhost)
            {
                InitializeGhost();
            }

            if (entity.isWillBeShotNext)
            {
                InitializeWillBeShot();
            }
        }

        private void InitializeGhost()
        {
            spriteRenderer.color = Color.green;
            spriteRenderer.sortingOrder = 1;
            DisableCollider();
        }

        private void InitializeWillBeShot()
        {
            DisableCollider();
        }

        private void DisableCollider()
        {
            visualCollider.enabled = false;
            realCollider.enabled = false;
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