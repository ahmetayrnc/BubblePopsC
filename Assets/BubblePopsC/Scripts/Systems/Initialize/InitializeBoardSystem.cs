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
            const int boardHeight = 10;
            _contexts.game.SetBoardSize(new Vector2Int(boardWidth, boardHeight));
            _contexts.game.SetIdCount(0);
            SetPlayArea(boardWidth, boardHeight);
            _contexts.game.SetShooterPosition(new Vector2(_contexts.game.playArea.Size.x / 2f, -1f));
        }

        private void SetPlayArea(int boardWidth, int boardHeight)
        {
            var center = new Vector2((boardWidth - 0.5f) * 0.5f * Mathf.Sqrt(3) * 0.5f, 3.375f);
            var size = new Vector2((boardWidth - 0.5f) * 0.5f * Mathf.Sqrt(3), 7.75f);

            var rightWallStart = center;
            rightWallStart.x += size.x / 2;
            rightWallStart.y -= size.y / 2;

            var rightWallEnd = rightWallStart;
            rightWallEnd.y += size.y;

            var leftWallStart = rightWallStart;
            leftWallStart.x -= size.x;

            var leftWallEnd = rightWallEnd;
            leftWallEnd.x -= size.x;

            _contexts.game.SetPlayArea(center, size, rightWallStart, rightWallEnd,
                leftWallStart, leftWallEnd);
        }
    }
}