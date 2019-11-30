using System.Collections.Generic;
using BubblePopsC.Scripts.Components.Position;
using Entitas;
using UnityEngine;

namespace BubblePopsC.Scripts.Systems.Initialize
{
    public class InitializeTilesSystem : IInitializeSystem
    {
        private readonly Contexts _contexts;

        public InitializeTilesSystem(Contexts contexts)
        {
            _contexts = contexts;
        }

        public void Initialize()
        {
            var boardSize = _contexts.game.boardSize;
            var width = boardSize.Value.x;
            var height = boardSize.Value.y;

            var ceilingCoords = new HashSet<AxialCoord>();

            for (var r = 0; r < height; r++)
            {
                var rOffset = r >> 1;
                for (var q = -rOffset; q < width - rOffset; q++)
                {
                    CreateTile(new AxialCoord {Q = q, R = r});

                    if (r == height - 1)
                    {
                        ceilingCoords.Add(new AxialCoord {Q = q, R = r});
                    }
                }
            }

            _contexts.game.SetCeilingCoords(ceilingCoords);
        }

        private void CreateTile(AxialCoord hex)
        {
            var tileEntity = _contexts.game.CreateEntity();

            tileEntity.isTile = true;
            tileEntity.AddAxialCoord(hex);
        }
    }
}