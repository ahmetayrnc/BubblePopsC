namespace BubblePopsC.Scripts.Services
{
    public static class BubbleCreatorService
    {
        public static GameEntity CreateBubble()
        {
            var bubble = Contexts.sharedInstance.game.CreateEntity();

            bubble.isBubble = true;
            bubble.AddId(IdService.GetNewId());
            return bubble;
        }

        public static GameEntity CreateGhostBubble()
        {
            var bubble = CreateBubble();
            bubble.isGhost = true;

            return bubble;
        }
    }
}