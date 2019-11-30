using System.Collections.Generic;
using System.Linq;
using BubblePopsC.Scripts.Components.Position;
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

            foreach (var bubble in _hexMap)
            {
                if (bubble == null) continue;

                if (IsConnected(bubble.axialCoord.Value)) continue;

                bubble.isDestroyed = true;
            }
        }

        private bool IsConnected(AxialCoord rootCoord)
        {
            var ceilingCoords = _contexts.game.ceilingCoords.Value;

            var visited = new bool[_boardSize.x, _boardSize.y];
            for (var x = 0; x < _boardSize.x; x++)
            {
                for (var y = 0; y < _boardSize.y; y++)
                {
                    visited[x, y] = false;
                }
            }

            var queue = new Queue<AxialCoord>();
            queue.Enqueue(rootCoord);
            var rootIndices = HexHelperService.GetArrayIndices(rootCoord);
            visited[rootIndices.x, rootIndices.y] = true;

            while (queue.Count > 0)
            {
                var testCord = queue.Dequeue();

                if (ceilingCoords.Any(c => c.Q == testCord.Q && c.R == testCord.R))
                {
                    return true;
                }

                var neighbours = HexHelperService.GetNeighbours(testCord);
                foreach (var neighbourCoord in neighbours)
                {
                    var arrayIndices = HexHelperService.GetArrayIndices(neighbourCoord);

                    //bounds check
                    if (arrayIndices.x >= _boardSize.x
                        || arrayIndices.y >= _boardSize.y
                        || arrayIndices.x < 0
                        || arrayIndices.y < 0) continue;

                    //visited check
                    if (visited[arrayIndices.x, arrayIndices.y]) continue;
                    visited[arrayIndices.x, arrayIndices.y] = true;

                    //null check
                    if (_hexMap[arrayIndices.x, arrayIndices.y] == null) continue;

                    queue.Enqueue(neighbourCoord);
                }
            }

            return false;
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