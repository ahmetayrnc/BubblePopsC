using System.Collections.Generic;
using BubblePopsC.Scripts.Services;
using Entitas;
using UnityEngine;

namespace BubblePopsC.Scripts.Systems
{
    public class BoardShifterSystem : ReactiveSystem<GameEntity>, IInitializeSystem
    {
        private Vector2Int _boardSize;
        private readonly Contexts _contexts;
        private GameEntity[,] _hexMap;
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
            _boardSize = _contexts.game.boardSize.Value;
            _hexMap = new GameEntity[_boardSize.x, _boardSize.y];
            _bubbleGroup = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Bubble)
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
            var shouldShiftDown = true;
            foreach (var bubble in _bubbleGroup)
            {
                var point = HexHelperService.HexToPoint(bubble.axialCoord.Value);
                if (point.y <= UpShitLimit) shouldShiftDown = false;
                if (point.y <= DownShiftLimit)
                {
                    ShiftBoard(ShiftAmount);
                    return;
                }
            }

            if (shouldShiftDown) ShiftBoard(-ShiftAmount);
        }

        private void ShiftBoard(float amount)
        {
            var boardOffset = _contexts.game.boardOffset.Value;
            boardOffset += amount;
            _contexts.game.ReplaceBoardOffset(boardOffset);
        }
    }
}