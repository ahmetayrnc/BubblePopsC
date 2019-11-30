using System;
using BubblePopsC.Scripts.Components.Position;
using BubblePopsC.Scripts.Services;
using DG.Tweening;
using Entitas.Unity;
using TMPro;
using UnityEngine;

namespace BubblePopsC.Scripts.Mono.View
{
    public class BubbleView : View, IAxialCoordListener, IPositionListener, IDestroyedListener, IShotListener,
        IGhostListener, IWillBeShotNextListener, IWillBeShotNextRemovedListener, IBubbleNumberListener, IMergeToListener
    {
        public SpriteRenderer spriteRenderer;
        public CircleCollider2D visualCollider;
        public PolygonCollider2D realCollider;
        public TextMeshPro bubbleNumber;

        protected override void AddListeners(GameEntity entity)
        {
            entity.AddAxialCoordListener(this);
            entity.AddPositionListener(this);
            entity.AddDestroyedListener(this);
            entity.AddShotListener(this);
            entity.AddGhostListener(this);
            entity.AddWillBeShotNextListener(this);
            entity.AddWillBeShotNextRemovedListener(this);
            entity.AddBubbleNumberListener(this);
            entity.AddMergeToListener(this);
        }

        protected override void InitializeView(GameEntity entity)
        {
            spriteRenderer.sortingLayerName = BubbleLayer;
            bubbleNumber.sortingLayerID = SortingLayer.NameToID(BubbleLayer);

            spriteRenderer.sortingOrder = 0;
            bubbleNumber.sortingOrder = 1;
        }

        public void OnAxialCoord(GameEntity entity, AxialCoord hex)
        {
            transform.position = HexHelperService.HexToPoint(hex);
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
            movement.onComplete += () => callback();
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

        public void OnBubbleNumber(GameEntity entity, int value)
        {
            bubbleNumber.text = entity.bubbleNumber.Value.ToString();
        }

        public void OnMergeTo(GameEntity entity, AxialCoord spot, Action callback)
        {
            transform.DOMove(HexHelperService.HexToPoint(spot), 0.1f).onComplete +=
                () => callback();
        }
    }
}