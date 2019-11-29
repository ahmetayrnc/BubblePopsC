using UnityEngine;

namespace BubblePopsC.Scripts.Services
{
    public static class BubbleCreatorService
    {
        public static GameEntity CreateBubble()
        {
            var bubble = Contexts.sharedInstance.game.CreateEntity();

            bubble.isBubble = true;
            bubble.AddId(IdService.GetNewId());
            bubble.AddBubbleNumber(GenerateBubbleNumber());
            return bubble;
        }

        public static GameEntity CreateGhostBubble()
        {
            var bubble = CreateBubble();
            bubble.isGhost = true;
            bubble.RemoveBubbleNumber();

            return bubble;
        }

        public static GameEntity CreateWillBeShotNextBubble()
        {
            var bubble = CreateBubble();

            bubble.AddPosition(Contexts.sharedInstance.game.shooterPosition.Value);
            bubble.isWillBeShotNext = true;
            return bubble;
        }

        public static int GenerateBubbleNumber()
        {
            var power = Random.Range(1, 6);
            return (int) Mathf.Pow(2, power);
        }
    }
}