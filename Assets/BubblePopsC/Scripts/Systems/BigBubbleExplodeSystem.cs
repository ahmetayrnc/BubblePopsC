using System.Collections.Generic;
using BubblePopsC.Scripts.Services;
using Entitas;

namespace BubblePopsC.Scripts.Systems
{
    public class BigBubbleExplodeSystem : ReactiveSystem<GameEntity>, IInitializeSystem
    {
        private readonly Contexts _contexts;
//        private GameEntity[,] _hexMap;

        public BigBubbleExplodeSystem(Contexts contexts) : base(contexts.game)
        {
            _contexts = contexts;
        }

        public void Initialize()
        {
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
            var boardSize = _contexts.game.boardSize.Value;
            var hexMap = HexStorageService.UpdateHexMap();

            for (var x = 0; x < boardSize.x; x++)
            {
                for (var y = 0; y < boardSize.y; y++)
                {
                    var bubble = hexMap[x, y];

                    if (bubble == null) continue;

                    var bubbleNumber = bubble.bubbleNumber.Value;
                    if (bubbleNumber < 2048) continue;

                    var bubbleCoord = bubble.axialCoord.Value;
                    var neighbours = HexHelperService.GetNeighbours(bubbleCoord);

                    foreach (var neighbourCoord in neighbours)
                    {
                        var arrayIndices = HexHelperService.GetArrayIndices(neighbourCoord);
                        //bounds check
                        if (arrayIndices.x >= boardSize.x
                            || arrayIndices.y >= boardSize.y
                            || arrayIndices.x < 0
                            || arrayIndices.y < 0) continue;

                        //null check
                        if (hexMap[arrayIndices.x, arrayIndices.y] == null) continue;

                        hexMap[arrayIndices.x, arrayIndices.y].isDestroyed = true;
                    }

                    bubble.isDestroyed = true;
                }
            }
        }
    }
}