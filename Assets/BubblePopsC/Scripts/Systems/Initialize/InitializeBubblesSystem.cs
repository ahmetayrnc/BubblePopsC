using System.Collections.Generic;
using BubblePopsC.Scripts.Components.Position;
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
            var boardSize = _contexts.game.boardSize.Value;
            const int firstRowsCount = 6;
            const int ceilingRowsCount = 4;
            var ceilingCoords = new HashSet<AxialCoord>();
            for (var r = 0; r < boardSize.y; r++)
            {
                var rOffset = r >> 1;
                for (var q = -rOffset; q < boardSize.x - rOffset; q++)
                {
                    if (boardSize.y - r <= firstRowsCount)
                    {
                        var axialCoord = new AxialCoord {Q = q, R = r};
                        BubbleCreatorService.CreateBoardBubble(axialCoord,
                            BubbleCreatorService.GenerateRandomBubbleNumber());
                    }

                    if (boardSize.y - r <= ceilingRowsCount)
                    {
                        ceilingCoords.Add(new AxialCoord {Q = q, R = r});
                    }
                }
            }

            _contexts.game.SetCeilingCoords(ceilingCoords);
        }

        private void CreateBubblesInShooter()
        {
            BubbleCreatorService.CreateShooterBubble();
        }
    }
}