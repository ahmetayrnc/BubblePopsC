using System.Collections.Generic;
using BubblePopsC.Scripts.Services;
using Entitas;
using UnityEngine;

namespace BubblePopsC.Scripts.Systems
{
    public class BubbleDropperSystem : ReactiveSystem<GameEntity>, IInitializeSystem
    {
        private Vector2Int _boardSize;
        private readonly Contexts _contexts;
        private GameEntity[,] _hexMap;
        private IGroup<GameEntity> _bubbleGroup;

        public BubbleDropperSystem(Contexts contexts) : base(contexts.game)
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
            return context.CreateCollector(GameMatcher.Destroyed.Added());
        }

        protected override bool Filter(GameEntity entity)
        {
            return true;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            UpdateHexMap();

            var ceilingCoords = _contexts.game.ceilingCoords.Value;

            for (var x = 0; x < _boardSize.x; x++)
            {
                for (var y = 0; y < _boardSize.y; y++)
                {
                    var bubble = _hexMap[x, y];

                    if (bubble == null) continue;

                    var connectedToCeiling = false;
                    foreach (var ceilingCoord in ceilingCoords)
                    {
                        if (!HexHelperService.HasPath(bubble.axialCoord.Value, ceilingCoord)) continue;
                        
                        connectedToCeiling = true;
                        break;
                    }

                    if (connectedToCeiling) continue;

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