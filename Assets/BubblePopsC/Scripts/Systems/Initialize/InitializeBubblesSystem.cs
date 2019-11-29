using BubblePopsC.Scripts.Services;
using Entitas;
using UnityEngine;

namespace BubblePopsC.Scripts.Systems.Initialize
{
    public class InitializeBubblesSystem : IInitializeSystem
    {
        private readonly Contexts _contexts;
        private readonly IGroup<GameEntity> _tilesGroup;

        public InitializeBubblesSystem(Contexts contexts)
        {
            _contexts = contexts;
            _tilesGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Tile));
        }

        public void Initialize()
        {
            CreateBubblesInGrid();
            CreateBubblesInShooter();
        }

        private void CreateBubblesInGrid()
        {
            var boardSize = _contexts.game.boardSize;
            const int firstRowsCount = 3;
            foreach (var tileEntity in _tilesGroup.GetEntities())
            {
                var axialCoord = tileEntity.axialCoord;
                if (boardSize.Value.y - axialCoord.R > firstRowsCount) continue;

                var bubble = BubbleCreatorService.CreateBubble();
                bubble.AddAxialCoord(axialCoord.Q, axialCoord.R);
            }
        }

        private void CreateBubblesInShooter()
        {
            BubbleCreatorService.CreateWillBeShotNextBubble();
        }
    }
}