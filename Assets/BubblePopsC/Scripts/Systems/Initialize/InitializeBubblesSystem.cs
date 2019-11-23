using Entitas;

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
            var boardSize = _contexts.game.boardSize;
            const int firstRowsCount = 3;
            foreach (var tileEntity in _tilesGroup.GetEntities())
            {
                var axialCoord = tileEntity.axialCoord;
                if (boardSize.Value.y - axialCoord.R > firstRowsCount) continue;

                var bubble = _contexts.game.CreateEntity();

                bubble.isBubble = true;
                bubble.AddAxialCoord(axialCoord.Q, axialCoord.R);
            }
        }
    }
}