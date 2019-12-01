using Entitas;
using Entitas.Unity;
using UnityEngine;

namespace BubblePopsC.Scripts.Mono.View
{
    public abstract class View : MonoBehaviour
    {
        protected const string PlayAreaLayer = "PlayArea";
        protected const string TileLayer = "Tile";
        protected const string BubbleLayer = "Bubble";
        
        public void Link(IEntity entity)
        {
            gameObject.Link(entity);
            var ge = (GameEntity) entity;
            AddListeners(ge);
            InitializeView(ge);
        }

        protected abstract void AddListeners(GameEntity entity);

        protected abstract void InitializeView(GameEntity entity);
    }
}