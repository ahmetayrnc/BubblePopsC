using UnityEngine;

namespace BubblePopsC.Scripts.Mono.Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class CameraScaler : MonoBehaviour
    {
        public float heightPadding;
        public float widthPadding;

        private float _sceneWidth = 10;
        private float _sceneHeight = 10;

//        private float _boardWidth = 10;
//        private float _boardHeight = 10;
        private UnityEngine.Camera _camera;
        private Transform _transform;

        private void Start()
        {
            _camera = GetComponent<UnityEngine.Camera>();
//            AttachToBoard();
            _transform = transform;
        }

        private void Update()
        {
            var playArea = Contexts.sharedInstance.game.playArea;
            _sceneHeight = playArea.Size.y + heightPadding;
            _sceneWidth = playArea.Size.x + widthPadding;

            var newPos = _transform.position;
            var unitsPerPixelAccToWidth = _sceneWidth / Screen.width;
            var desiredHalfHeightAccToWidth = 0.5f * unitsPerPixelAccToWidth * Screen.height;

            var unitsPerPixelAccToHeight = _sceneHeight / Screen.height;
            var desiredHalfHeightAccToHeight = 0.5f * unitsPerPixelAccToHeight * Screen.height;

            newPos.x = (_sceneWidth - widthPadding) / 2f;
            newPos.y = (_sceneHeight - heightPadding) / 2f; // - 2.5f;
            _transform.position = newPos;

            _camera.orthographicSize = Mathf.Max(desiredHalfHeightAccToHeight, desiredHalfHeightAccToWidth);
        }

//        private void AttachToBoard()
//        {
//            Contexts.sharedInstance.game.CreateEntity().AddAnyBoardSizeListener(this);
//        }
//
//        private void Reset()
//        {
//            AttachToBoard();
//        }
//
//        public void OnAnyBoardSize(GameEntity entity, Vector2Int value)
//        {
//            _boardWidth = value.x;
//            _boardHeight = value.y;
//        }
    }
}