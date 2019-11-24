using UnityEngine;

namespace BubblePopsC.Scripts.Mono.View
{
    public abstract class UniqueView : MonoBehaviour
    {
        private void Start()
        {
            AddListeners();
        }

        protected abstract void AddListeners();
    }
}