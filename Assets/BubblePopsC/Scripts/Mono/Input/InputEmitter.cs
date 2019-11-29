using System.Linq;
using BubblePopsC.Scripts.Mono.Collision;
using UnityEngine;

namespace BubblePopsC.Scripts.Mono.Input
{
    public class InputEmitter : MonoBehaviour
    {
        private Contexts _contexts;

        private UnityEngine.Camera _cam;

        private void Start()
        {
            _cam = UnityEngine.Camera.main;
            _contexts = Contexts.sharedInstance;
        }

        private void Update()
        {
            EmitInput();
        }

        private void EmitInput()
        {
            var touchPos = _cam.ScreenToWorldPoint(UnityEngine.Input.mousePosition);

            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                _contexts.input.CreateEntity().AddTouchDown(touchPos);
                _contexts.input.SetTouchPosition(touchPos);
            }

            if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                _contexts.input.CreateEntity().AddTouchUp(touchPos);
            }

            if (_contexts.input.hasTouchPosition)
            {
                _contexts.input.ReplaceTouchPosition(touchPos);
//                var list = CollisionFinder.GetTrajectory(_contexts.game.shooterPosition.Value, touchPos);
//                Debug.Log(list.Count);
            }
        }
    }
}