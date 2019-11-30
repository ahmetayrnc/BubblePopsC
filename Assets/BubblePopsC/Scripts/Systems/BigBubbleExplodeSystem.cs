using System.Collections.Generic;
using BubblePopsC.Scripts.Services;
using Entitas;
using UnityEngine;

namespace BubblePopsC.Scripts.Systems
{
    public class BigBubbleExplodeSystem : ReactiveSystem<GameEntity>, IInitializeSystem
    {
        private Vector2Int _boardSize;
        private readonly Contexts _contexts;
        private GameEntity[,] _hexMap;
        private IGroup<GameEntity> _bubbleGroup;

        public BigBubbleExplodeSystem(Contexts contexts) : base(contexts.game)
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
            UpdateHexMap();

            for (var x = 0; x < _boardSize.x; x++)
            {
                for (var y = 0; y < _boardSize.y; y++)
                {
                    var bubble = _hexMap[x, y];

                    if (bubble == null) continue;

                    var bubbleNumber = bubble.bubbleNumber.Value;
                    if (bubbleNumber < 2048) continue;

                    var bubbleCoord = bubble.axialCoord.Value;
                    var neighbours = HexHelperService.GetNeighbours(bubbleCoord);

                    foreach (var neighbourCoord in neighbours)
                    {
                        var arrayIndices = HexHelperService.GetArrayIndices(neighbourCoord);
                        //bounds check
                        if (arrayIndices.x >= _boardSize.x
                            || arrayIndices.y >= _boardSize.y
                            || arrayIndices.x < 0
                            || arrayIndices.y < 0) continue;

                        //null check
                        if (_hexMap[arrayIndices.x, arrayIndices.y] == null) continue;

                        _hexMap[arrayIndices.x, arrayIndices.y].isDestroyed = true;
                    }

                    bubble.isDestroyed = true;
                }
            }
        }

        private void UpdateHexMap()
        {
            for (var x = 0; x < _boardSize.x; x++)
            {
                for (var y = 0; y < _boardSize.y; y++)
                {
                    _hexMap[x, y] = null;
                }
            }

            //get the bubbles in an array
            foreach (var bubble in _bubbleGroup)
            {
                var bubbleAxialCoord = bubble.axialCoord.Value;
                var arrayIndices = HexHelperService.GetArrayIndices(bubbleAxialCoord);
                _hexMap[arrayIndices.x, arrayIndices.y] = bubble;
            }
        }
    }
}