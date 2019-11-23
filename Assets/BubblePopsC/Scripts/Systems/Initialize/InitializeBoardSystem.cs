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
            _contexts.game.SetBoardSize(new Vector2Int(7, 10));
            _contexts.game.SetIdCount(0);
            _contexts.game.SetPlayArea(Mathf.Sqrt(3f) * 5.5f, 9f);
        }
    }
}