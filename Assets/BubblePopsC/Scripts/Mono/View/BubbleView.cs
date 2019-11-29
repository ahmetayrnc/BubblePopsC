using System;
using BubblePopsC.Scripts.Services;
using DG.Tweening;
using Entitas.Unity;
using UnityEngine;

namespace BubblePopsC.Scripts.Mono.View
{
    public class BubbleView : View, IAxialCoordListener, IPositionListener, IDestroyedListener, IShotListener,
        IGhostListener, IWillBeShotNextListener, IWillBeShotNextRemovedListener
    {
        public SpriteRenderer spriteRenderer;
        public CircleCollider2D visualCollider;
        public PolygonCollider2D realCollider;

        protected override void AddListeners(GameEntity entity)
        {
            entity.AddAxialCoordListener(this);
            entity.AddPositionListener(this);
            entity.AddDestroyedListener(this);
            entity.AddShotListener(this);
            entity.AddGhostListener(this);
            entity.AddWillBeShotNextListener(this);
            entity.AddWillBeShotNextRemovedListener(this);
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

        public void OnDestroyed(GameEntity entity)
        {
            GameObject o;
            (o = gameObject).Unlink();
            Destroy(o);
        }

        public void OnShot(GameEntity entity, Vector3[] trajectory, Action callback)
        {
            var movement = transform.DOPath(trajectory, 0.4f);
            movement.onComplete += () => { callback(); };
            movement.SetEase(Ease.InOutSine);
        }

        private void ToggleCollider(bool active)
        {
            visualCollider.enabled = active;
            realCollider.enabled = active;
        }

        public void OnGhost(GameEntity entity)
        {
            spriteRenderer.color = Color.green;
            spriteRenderer.sortingOrder = 1;
            ToggleCollider(false);
        }

        public void OnWillBeShotNext(GameEntity entity)
        {
            ToggleCollider(false);
        }

        public void OnWillBeShotNextRemoved(GameEntity entity)
        {
            ToggleCollider(true);
        }
    }
}