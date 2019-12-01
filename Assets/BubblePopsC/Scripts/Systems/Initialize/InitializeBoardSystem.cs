using Entitas;
using UnityEngine;

namespace BubblePopsC.Scripts.Systems.Initialize
{
    public class InitializeBoardSystem : IInitializeSystem
    {
        private readonly Contexts _contexts;

        public InitializeBoardSystem(Contexts contexts)
        {
            _contexts = contexts;
        }

        public void Initialize()
        {
            const int boardWidth = 6;
            const int boardHeight = 14;
            _contexts.game.SetBoardSize(new Vector2Int(boardWidth, boardHeight));
            _contexts.game.SetIdCount(0);
            SetPlayArea(boardWidth);
            _contexts.game.SetShooterPosition(new Vector2(_contexts.game.playArea.Size.x / 2f, -0.5f));
            _contexts.game.SetBoardOffset(false);
        }

        private void SetPlayArea(int boardWidth)
        {
            var center = new Vector2((boardWidth - 0.5f) * 0.5f * Mathf.Sqrt(3) * 0.5f, 3.375f);
            var size = new Vector2((boardWidth - 0.5f) * 0.5f * Mathf.Sqrt(3), 7.75f);

            _contexts.game.SetPlayArea(center, size);
        }
    }
}