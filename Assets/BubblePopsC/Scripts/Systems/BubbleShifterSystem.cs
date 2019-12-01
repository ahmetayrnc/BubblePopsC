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
            for (var r = 0; r < height; r++)
            {
                var rOffset = r >> 1;
                for (var q = -rOffset; q < width - rOffset; q++)
                {
                    if (indented)
                    {
                        if (height - r != 1) continue;
                    }
                    else
                    {
                        if (height - r != 2) continue;
                    }

                    var axialCoord = new AxialCoord {Q = q, R = r};

                    var indices = HexHelperService.GetArrayIndices(axialCoord);
                    if (hexMap[indices.x, indices.y] != null) continue;

                    BubbleCreatorService.CreateBoardBubble(axialCoord,
                        BubbleCreatorService.GenerateRandomBubbleNumber());
                }
            }
        }
    }
}