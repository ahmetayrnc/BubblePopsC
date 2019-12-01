namespace BubblePopsC.Scripts.Services
{
    public static class HexStorageService
    {
        public static GameEntity[,] UpdateHexMap()
        {
            var contexts = Contexts.sharedInstance;
            var boardSize = contexts.game.boardSize.Value;
            var hexMap = new GameEntity[boardSize.x, boardSize.y];
            var bubbleGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Bubble, GameMatcher.AxialCoord)
                .NoneOf(GameMatcher.Destroyed, GameMatcher.Ghost, GameMatcher.WillBeShotNext));

            for (var x = 0; x < boardSize.x; x++)
            {
                for (var y = 0; y < boardSize.y; y++)
                {
                    hexMap[x, y] = null;
                }
            }

            //get the bubbles in an array
            foreach (var bubble in bubbleGroup)
            {
                var bubbleAxialCoord = bubble.axialCoord.Value;
                var arrayIndices = HexHelperService.GetArrayIndices(bubbleAxialCoord);
                hexMap[arrayIndices.x, arrayIndices.y] = bubble;
            }

            return hexMap;
        }
    }
}