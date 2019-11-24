namespace BubblePopsC.Scripts.Services
{
    public class BubbleCreatorService
    {
        public static GameEntity CreateBubble()
        {
            var bubble = Contexts.sharedInstance.game.CreateEntity();

            bubble.isBubble = true;
            bubble.AddId(IdService.GetNewId());
            return bubble;
//            bubble.AddAxialCoord(axialCoord.Q, axialCoord.R);
        }
    }
}