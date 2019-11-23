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

            for (var r = 0; r < height; r++)
            {
                var rOffset = r >> 1;
                for (var q = -rOffset; q < width - rOffset; q++)
                {
                    CreateTile(q, r);
                }
            }
        }

        private void CreateTile(int q, int r)
        {
            var tileEntity = _contexts.game.CreateEntity();

            tileEntity.isTile = true;
            tileEntity.AddAxialCoord(q, r);
        }
    }
}