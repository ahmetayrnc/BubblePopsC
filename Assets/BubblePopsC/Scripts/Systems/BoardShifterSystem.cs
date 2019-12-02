using System;
using System.Collections.Generic;
using BubblePopsC.Scripts.Components.Position;
using BubblePopsC.Scripts.Services;
using Entitas;
using UnityEngine;

namespace BubblePopsC.Scripts.Systems
{
    public class BoardShifterSystem : ReactiveSystem<GameEntity>, IInitializeSystem
    {
        private readonly Contexts _contexts;
        private IGroup<GameEntity> _bubbleGroup;

        private const float ShiftAmount = 0.75f;
        private const float UpShitLimit = 2.25f;
        private const float DownShiftLimit = 1.5f;

        public BoardShifterSystem(Contexts contexts) : base(contexts.game)
        {
            _contexts = contexts;
        }

        public void Initialize()
        {
            _bubbleGroup = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Bubble, GameMatcher.AxialCoord)
                .NoneOf(GameMatcher.Destroyed, GameMatcher.Ghost, GameMatcher.WillBeShotNext));
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.Merging.Removed());
        }

        protected override bool Filter(GameEntity entity)
        {
            return !entity.isMerging;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            _contexts.input.isInputDisabled = false;
            var shouldShiftDown = true;
            foreach (var bubble in _bubbleGroup)
            {
                var point = HexHelperService.HexToPoint(bubble.axialCoord.Value);
                if (point.y <= UpShitLimit) shouldShiftDown = false;
                if (point.y <= DownShiftLimit)
                {
                    ShiftUp();
                    return;
                }
            }

            if (shouldShiftDown) ShiftDown();
        }

        private void ShiftUp()
        {
            ShiftBubbles(+1);
        }

        private void ShiftDown()
        {
            ShiftBubbles(-1);
        }

        private void ShiftBubbles(int direction)
        {
            foreach (var bubble in _bubbleGroup)
            {
                var bubblePos = HexHelperService.HexToPoint(bubble.axialCoord.Value);
                bubblePos.y += ShiftAmount * direction;
                bubble.AddShiftTo(bubblePos, () =>
                {
                    bubble.RemoveShiftTo();
                    bubble.AddPosition(bubblePos);
                    _contexts.game.isBubbleShiftDirty = true;
                });
            }
        }
    }
}