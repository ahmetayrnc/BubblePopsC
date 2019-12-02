using System;
using BubblePopsC.Scripts.Components.Position;
using BubblePopsC.Scripts.Mono.Misc;
using BubblePopsC.Scripts.Mono.ScriptableObjects;
using BubblePopsC.Scripts.Services;
using DG.Tweening;
using Entitas.Unity;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BubblePopsC.Scripts.Mono.View
{
    public class BubbleView : View, IAxialCoordListener, IPositionListener, IDestroyedListener, IShotListener,
        IGhostListener, IWillBeShotNextListener, IWillBeShotNextRemovedListener, IBubbleNumberListener,
        IMergeToListener, IShiftToListener, IDroppedListener, IExplodedListener, ISpareBubbleListener,
        IMoveToShooterListener, INudgedListener
    {
        public SpriteRenderer spriteRenderer;
        public SpriteRenderer shadow;
        public CircleCollider2D visualCollider;
        public PolygonCollider2D realCollider;
        public TextMeshPro bubbleNumber;
        public BubbleColors bubbleColors;
        public TrailRenderer trail;
        public ParticleHandler mergeParticle;
        public ParticleHandler dropParticle;
        public ParticleHandler explodeParticle;

        private const int ShadowOrder = -1;
        private const int BubbleOrder = 0;
        private const int BubbleNumberOrder = 1;
        private const int GhostBubbleOrder = 2;
        private const int SpareBubbleOrder = 3;
        private const int SpareBubbleNumberOrder = 4;
        private const int ShotBubbleOrder = 5;
        private const int ShotBubbleNumberOrder = 6;

        private const int ShotSpeed = 16;
        private const float GhostBallOpacity = 0.5f;
        private const float MergeDuration = 0.25f;
        private const float ShiftDuration = 0.5f;
        private const float GhostAppearDuration = 0.2f;

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
            entity.AddShiftToListener(this);
            entity.AddDroppedListener(this);
            entity.AddExplodedListener(this);
            entity.AddSpareBubbleListener(this);
            entity.AddMoveToShooterListener(this);
            entity.AddNudgedListener(this);
        }

        protected override void InitializeView(GameEntity entity)
        {
            spriteRenderer.sortingLayerName = BubbleLayer;
            shadow.sortingLayerName = BubbleLayer;
            bubbleNumber.sortingLayerID = SortingLayer.NameToID(BubbleLayer);

            shadow.sortingOrder = ShadowOrder;
            spriteRenderer.sortingOrder = BubbleOrder;
            bubbleNumber.sortingOrder = BubbleNumberOrder;
            trail.enabled = false;
        }

        public void OnAxialCoord(GameEntity entity, AxialCoord hex)
        {
            transform.position = HexHelperService.HexToPoint(hex);

            if (entity.isGhost)
            {
                GhostBubbleAppear();
            }
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
            var movement = transform.DOPath(trajectory, ShotSpeed).SetSpeedBased();
            movement.onComplete += () => callback();
        }

        private void ToggleCollider(bool active)
        {
            visualCollider.enabled = active;
            realCollider.enabled = active;
        }

        public void OnGhost(GameEntity entity)
        {
            var willBeShotBubble = Contexts.sharedInstance.game.GetGroup(GameMatcher.WillBeShotNext).GetSingleEntity();
            var ghostColor = bubbleColors.value[(int) Mathf.Log(willBeShotBubble.bubbleNumber.Value, 2) - 1];
            ghostColor.a = GhostBallOpacity;

            spriteRenderer.color = ghostColor;
            spriteRenderer.sortingOrder = GhostBubbleOrder;
            shadow.enabled = false;
            ToggleCollider(false);
        }

        private void GhostBubbleAppear()
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, GhostAppearDuration);
        }

        public void OnWillBeShotNext(GameEntity entity)
        {
            trail.enabled = true;
            spriteRenderer.sortingOrder = ShotBubbleOrder;
            bubbleNumber.sortingOrder = ShotBubbleNumberOrder;
            ToggleCollider(false);
        }

        public void OnWillBeShotNextRemoved(GameEntity entity)
        {
            trail.enabled = false;
            spriteRenderer.sortingOrder = BubbleOrder;
            bubbleNumber.sortingOrder = BubbleNumberOrder;
            ToggleCollider(true);
        }

        public void OnBubbleNumber(GameEntity entity, int value)
        {
            bubbleNumber.text = GetBubbleNumberText(entity.bubbleNumber.Value);
            spriteRenderer.color = bubbleColors.value[(int) Math.Log(value, 2) - 1];
        }

        private string GetBubbleNumberText(int number)
        {
            if (number == 1024)
            {
                return "1K";
            }

            if (number == 2048)
            {
                return "2K";
            }

            return number.ToString();
        }

        public void OnMergeTo(GameEntity entity, AxialCoord spot, Action callback)
        {
            var movement = transform.DOMove(HexHelperService.HexToPoint(spot), MergeDuration);
            DoWait.WaitSeconds(0.05f, () =>
                CreateBubbleParticle(transform.position, spriteRenderer.color, mergeParticle));
            var seq = DOTween.Sequence();
            seq.AppendInterval(0.05f);
            seq.Append(movement);
            seq.onComplete += () => callback();
        }

        public void OnShiftTo(GameEntity entity, Vector2 spot, Action callback)
        {
            var movement = transform.DOMove(spot, ShiftDuration);
            movement.onComplete += () => callback();
        }

        public void OnDropped(GameEntity entity, Action callback)
        {
            var speed = Random.Range(4f, 10f);
            var xOffset = Random.Range(-2f, 2f);
            var transform1 = transform;
            var pos = transform1.position;
            transform1.DOMoveX(pos.x + xOffset, speed / 16f).SetSpeedBased();
            var yMovement = transform1.DOMoveY(-0.5f, speed).SetSpeedBased();
            yMovement.onComplete += () => callback();
            yMovement.onComplete += () =>
            {
                CreateBubbleParticle(transform.position,
                    bubbleColors.value[(int) Math.Log(entity.bubbleNumber.Value, 2) - 1], dropParticle);
            };
        }

        private void CreateBubbleParticle(Vector3 pos, Color color, ParticleHandler particlePrefab)
        {
            var particle = Instantiate(particlePrefab);
            particle.particleRenderer.sortingLayerName = BubbleParticleLayer;
            particle.transform.position = pos;
            var main = particle.particle.main;
            main.startColor = color;
            particle.PlayAndDie();
        }

        public void OnExploded(GameEntity entity, Action callback, bool isMaster)
        {
            if (!isMaster)
            {
                DoWait.WaitSeconds(0.05f, () =>
                {
                    CreateBubbleParticle(transform.position, spriteRenderer.color, dropParticle);
                    callback();
                });
                return;
            }

            DoWait.WaitSeconds(0.05f, () =>
            {
                CreateBubbleParticle(transform.position, spriteRenderer.color, explodeParticle);
                callback();
            });
        }

        public void OnSpareBubble(GameEntity entity)
        {
            spriteRenderer.sortingOrder = SpareBubbleOrder;
            bubbleNumber.sortingOrder = SpareBubbleNumberOrder;
            transform.localScale = Vector3.one * 0.75f;
            ToggleCollider(false);
        }

        public void OnMoveToShooter(GameEntity entity)
        {
            transform.DOScale(1f, 0.2f).SetEase(Ease.OutSine).OnComplete(() =>
            {
                transform.DOMove(Contexts.sharedInstance.game.shooterPosition.Value, 0.1f).SetEase(Ease.OutSine);
            });
        }

        public void OnNudged(GameEntity entity, AxialCoord from, Action callback)
        {
            var pos = (Vector2) transform.position;
            var nudgePos = HexHelperService.HexToPoint(from);
            var nudgeDirection = pos - nudgePos;
            nudgeDirection = nudgeDirection.normalized;

            transform.DOMove(pos + nudgeDirection * 0.2f, 0.2f).SetEase(Ease.OutSine).SetLoops(2, LoopType.Yoyo)
                .onComplete += () => callback();
        }
    }
}