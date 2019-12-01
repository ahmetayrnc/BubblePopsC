using System.Collections.Generic;
using BubblePopsC.Scripts.Services;
using Entitas;
using UnityEngine;

namespace BubblePopsC.Scripts.Systems
{
    public class BubbleShifterSystem : ReactiveSystem<GameEntity>, IInitializeSystem
    {
        private readonly Contexts _contexts;
        private IGroup<GameEntity> _bubbleGroup;

        public BubbleShifterSystem(Contexts contexts) : base(contexts.game)
        {
            _contexts = contexts;
        }

        public void Initialize()
        {
            _bubbleGroup = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Bubble, GameMatcher.AxialCoord)
                .NoneOf(GameMatcher.Destroyed, GameMatcher.Ghost, GameMatcher.WillBeShotNext));
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.BubbleShiftDirty);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.isBubbleShiftDirty;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            _contexts.game.isBubbleShiftDirty = false;
            var indented = _contexts.game.boardOffset.Indented;
            _contexts.game.ReplaceBoardOffset(!indented);

            foreach (var bubble in _bubbleGroup)
            {
                var bubblePos = bubble.position.Value;
                bubble.ReplaceAxialCoord(HexHelperService.PointToHex(bubblePos));
                bubble.RemovePosition();
            }
        }
    }
}