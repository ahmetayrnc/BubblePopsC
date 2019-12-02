//using System.Collections.Generic;
//using BubblePopsC.Scripts.Components.Position;
//using BubblePopsC.Scripts.Services;
//using Entitas;
//
//namespace BubblePopsC.Scripts.Systems
//{
//    public class NudgeSystem : ReactiveSystem<GameEntity>
//    {
//        private readonly Contexts _contexts;
//
//        public NudgeSystem(Contexts contexts) : base(contexts.game)
//        {
//            _contexts = contexts;
//        }
//
//        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
//        {
//            return context.CreateCollector(GameMatcher.WillBeShotNext.Removed());
//        }
//
//        protected override bool Filter(GameEntity entity)
//        {
//            return !entity.isWillBeShotNext;
//        }
//
//        protected override void Execute(List<GameEntity> entities)
//        {
//            var boardSize = _contexts.game.boardSize.Value;
//            var hexMap = HexStorageService.UpdateHexMap();
//
//            var placedBubble = entities[0];
//            var neighbours = HexHelperService.GetNeighbours(placedBubble.axialCoord.Value);
//
//            foreach (var neighbourCoord in neighbours)
//            {
//                var arrayIndices = HexHelperService.GetArrayIndices(neighbourCoord);
//                //bounds check
//                if (arrayIndices.x >= boardSize.x
//                    || arrayIndices.y >= boardSize.y
//                    || arrayIndices.x < 0
//                    || arrayIndices.y < 0) continue;
//
//                //null check
//                if (hexMap[arrayIndices.x, arrayIndices.y] == null) continue;
//
//                NudgeBubble(hexMap[arrayIndices.x, arrayIndices.y], placedBubble.axialCoord.Value);
//            }
//        }
//    }
//}