using BubblePopsC.Scripts.Components.Position;
using UnityEngine;

namespace BubblePopsC.Scripts.Services
{
    public static class BubbleCreatorService
    {
        private static GameEntity CreateBubble()
        {
            var bubble = Contexts.sharedInstance.game.CreateEntity();

            bubble.isBubble = true;
            bubble.AddId(IdService.GetNewId());
            return bubble;
        }

        public static GameEntity CreateBoardBubble(AxialCoord coord, int bubbleNumber)
        {
            var bubble = CreateBubble();
            bubble.AddAxialCoord(coord);
            bubble.AddBubbleNumber(bubbleNumber);
            return bubble;
        }

        public static GameEntity CreateGhostBubble(AxialCoord coord)
        {
            var bubble = CreateBubble();
            bubble.isGhost = true;
            bubble.AddAxialCoord(coord);

            return bubble;
        }

        public static GameEntity CreateShooterBubble()
        {
            var bubble = CreateBubble();

            bubble.AddPosition(Contexts.sharedInstance.game.shooterPosition.Value);
            bubble.isWillBeShotNext = true;
            bubble.AddBubbleNumber(GenerateRandomBubbleNumber());

            return bubble;
        }

        public static int GenerateRandomBubbleNumber()
        {
            var power = Random.Range(1, 6);
            return (int) Mathf.Pow(2, power);
        }
    }
}