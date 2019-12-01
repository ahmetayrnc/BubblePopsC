using System.Collections.Generic;
using BubblePopsC.Scripts.Components.Position;
using BubblePopsC.Scripts.Services;
using Entitas;
using UnityEngine;

namespace BubblePopsC.Scripts.Systems
{
    public class BubbleShifterSystem : ReactiveSystem<GameEntity>, IInitializeSystem
    {
        private readonly Contexts _contexts;
        private IGroup<GameEntity> _bubbleGroup;

        public BubbleShifterSystem(Contexts contexts) : base(contexts.game)
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
            return context.CreateCollector(GameMatcher.BubbleShiftDirty);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.isBubbleShiftDirty;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            _contexts.game.isBubbleShiftDirty = false;
            var indented = _contexts.game.boardOffset.Indented;
            _contexts.game.ReplaceBoardOffset(!indented);

            var hexMap = HexStorageService.UpdateHexMap();

            foreach (var bubble in hexMap)
            {
                if (bubble == null) continue;
                var bubblePos = bubble.position.Value;
                bubble.ReplaceAxialCoord(HexHelperService.PointToHex(bubblePos));
                bubble.RemovePosition();
            }

            hexMap = HexStorageService.UpdateHexMap();

            var boardSize = _contexts.game.boardSize.Value;
            var width = boardSize.x;
            var height = boardSize.y;
            var r = height - 1;

            RemoveRow(r, width, hexMap);
            RemoveRow(r - 1, width, hexMap);
            FillRow(r - 2, width, hexMap);
            FillRow(r - 3, width, hexMap);
        }

        private void RemoveRow(int r, int width, GameEntity[,] hexMap)
        {
            var rOffset = r / 2;
            for (var q = -rOffset; q < width - rOffset; q++)
            {
                var axialCoord = new AxialCoord {Q = q, R = r};
                var indices = HexHelperService.GetArrayIndices(axialCoord);
                if (hexMap[indices.x, indices.y] == null) return;

                var bubble = hexMap[indices.x, indices.y];
                bubble.isDestroyed = true;
            }
        }

        private void FillRow(int r, int width, GameEntity[,] hexMap)
        {
            var rOffset = r / 2;
            for (var q = -rOffset; q < width - rOffset; q++)
            {
                var axialCoord = new AxialCoord {Q = q, R = r};

                var indices = HexHelperService.GetArrayIndices(axialCoord);

                if (hexMap[indices.x, indices.y] != null) continue;

                BubbleCreatorService.CreateBoardBubble(axialCoord,
                    BubbleCreatorService.GenerateRandomBubbleNumber());
            }
        }
    }
}