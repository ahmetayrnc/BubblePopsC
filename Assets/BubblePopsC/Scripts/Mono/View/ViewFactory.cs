using UnityEngine;

namespace BubblePopsC.Scripts.Mono.View
{
    public class ViewFactory : MonoBehaviour
    {
        private static ViewFactory _inst;

        private static ViewFactory Instance
        {
            get
            {
                if (_inst) return _inst;

                _inst = FindObjectOfType<ViewFactory>();
                Debug.Assert(_inst != null, "No ViewFactory in scene");

                return _inst;
            }
        }

        public GameObject bubbleView;
        public GameObject tileView;
        public GameObject playAreaView;

        [Space] public Transform tileParent;
        public Transform bubbleParent;

        public static GameObject SpawnBubble()
        {
            return Instance.SpawnBubbleInternal();
        }

        public static GameObject SpawnTile()
        {
            return Instance.SpawnTileInternal();
        }

        private GameObject SpawnBubbleInternal()
        {
            return Instantiate(bubbleView, bubbleParent);
        }

        private GameObject SpawnTileInternal()
        {
            return Instantiate(tileView, tileParent);
        }

        public static GameObject SpawnPlayArea()
        {
            return Instance.SpawnPlayAreaInternal();
        }

        private GameObject SpawnPlayAreaInternal()
        {
            return Instantiate(playAreaView, null);
        }
    }
}