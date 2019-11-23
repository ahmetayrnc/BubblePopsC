using System.Collections.Generic;
using BubblePopsC.Scripts.Mono.View;
using Entitas;
using UnityEngine;

namespace BubblePopsC.Scripts.Systems.View
{
    public class AddViewSystem : ReactiveSystem<GameEntity>
    {
        public AddViewSystem(Contexts contexts) : base(contexts.game)
        {
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.AnyOf(GameMatcher.Tile));
        }

        protected override bool Filter(GameEntity entity)
        {
            return !entity.hasView && (entity.isTile);
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var e in entities)
            {
                InstantiateView(e);
            }
        }

        private static void InstantiateView(GameEntity entity)
        {
            GameObject go;
//            if (entity.isTile)
//            {
            go = ViewFactory.SpawnTile();
//            }

//            else if (entity.isPiece)
//            {
//                go = ViewFactory.SpawnPiece();
//            }
//            else
//            {
//                go = ViewFactory.SpawnCube();
//            }
//
            var view = go.GetComponent<Mono.View.View>();
            view.Link(entity);
            entity.AddView(view);
        }
    }
}