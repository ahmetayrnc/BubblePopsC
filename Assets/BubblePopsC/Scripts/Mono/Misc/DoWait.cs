namespace BubblePopsC.Scripts.Mono.Misc
{
    using System;
    using System.Collections;
    using EnumeratorExtensions;
    using UnityEngine;

    public class DoWait : MonoBehaviour
    {
        private static DoWait _inst;

        private static DoWait Instance
        {
            get
            {
                if (_inst) return _inst;

                _inst = FindObjectOfType<DoWait>();
                Debug.Assert(_inst != null, "No DoWait in scene");

                return _inst;
            }
        }

        public static void KillAllRoutines()
        {
            Instance.KillAllRoutinesInternal();
        }

        public static Coroutine WaitFrame(int frameCount, Action action)
        {
            return Instance.WaitFrameInternal(frameCount, action);
        }

        public static Coroutine WaitFrame(int frameCount)
        {
            return Instance.WaitFrameInternal(frameCount);
        }

        public static Coroutine WaitSeconds(float seconds, Action action)
        {
            return Instance.WaitSecondsInternal(seconds, action);
        }

        public static Coroutine WaitSeconds(float seconds)
        {
            return Instance.WaitSecondsInternal(seconds);
        }

        public static Coroutine WaitUntil(Func<bool> predicate, Action action)
        {
            return Instance.WaitUntilInternal(predicate, action);
        }

        public static Coroutine WaitUntil(Func<bool> predicate)
        {
            return Instance.WaitUntilInternal(predicate);
        }

        public static Coroutine WaitWhile(Func<bool> predicate, Action action)
        {
            return Instance.WaitWhileInternal(predicate, action);
        }

        public static Coroutine WaitWhileAfterFrame(Func<bool> predicate, Action action)
        {
            return Instance.WaitWhileAfterFrameInternal(predicate, action);
        }

        public static Coroutine WaitWhile(Func<bool> predicate)
        {
            return Instance.WaitWhileInternal(predicate);
        }

        #region Internal

        private Coroutine WaitUntilInternal(Func<bool> predicate, Action action)
        {
            return StartCoroutine(WaitUntilCo(predicate, action));
        }

        private Coroutine WaitUntilInternal(Func<bool> predicate)
        {
            return StartCoroutine(WaitUntilCo(predicate));
        }

        private Coroutine WaitWhileInternal(Func<bool> predicate, Action action)
        {
            return StartCoroutine(WaitWhileCo(predicate, action));
        }

        private Coroutine WaitWhileAfterFrameInternal(Func<bool> predicate, Action action)
        {
            return StartCoroutine(WaitWhileAfterFrameCo(predicate, action));
        }

        private Coroutine WaitWhileInternal(Func<bool> predicate)
        {
            return StartCoroutine(WaitWhileCo(predicate));
        }

        private Coroutine WaitSecondsInternal(float seconds, Action action)
        {
            return StartCoroutine(WaitSecondsCo(seconds, action));
        }

        private Coroutine WaitSecondsInternal(float seconds)
        {
            return StartCoroutine(WaitSecondsCo(seconds));
        }

        private Coroutine WaitFrameInternal(int frameCount, Action action)
        {
            return StartCoroutine(WaitFrameCo(frameCount, action));
        }

        private Coroutine WaitFrameInternal(int frameCount)
        {
            return StartCoroutine(WaitFrameCo(frameCount));
        }

        private void KillAllRoutinesInternal()
        {
            StopAllCoroutines();
            RoutinePlayer.Instance.StopAllCoroutines();
        }

        #endregion

        #region Coroutines

        private static IEnumerator WaitUntilCo(Func<bool> predicate, Action action)
        {
            while (!predicate())
            {
                yield return null;
            }

            action();
        }

        private static IEnumerator WaitUntilCo(Func<bool> predicate)
        {
            while (!predicate())
            {
                yield return null;
            }
        }

        private static IEnumerator WaitWhileCo(Func<bool> predicate, Action action)
        {
            while (predicate())
            {
                yield return null;
            }

            action();
        }

        private static IEnumerator WaitWhileAfterFrameCo(Func<bool> predicate, Action action)
        {
            yield return null;

            while (predicate())
            {
                yield return null;
            }

            action();
        }

        private static IEnumerator WaitWhileCo(Func<bool> predicate)
        {
            while (predicate())
            {
                yield return null;
            }
        }

        private static IEnumerator WaitSecondsCo(float seconds, Action action)
        {
            yield return new WaitForSeconds(seconds);

            action();
        }

        private static IEnumerator WaitSecondsCo(float seconds)
        {
            yield return new WaitForSeconds(seconds);
        }

        private static IEnumerator WaitFrameCo(int frameCount, Action action)
        {
            while (frameCount > 0)
            {
                yield return null;
                frameCount--;
            }

            action();
        }

        private static IEnumerator WaitFrameCo(int frameCount)
        {
            while (frameCount > 0)
            {
                yield return null;
                frameCount--;
            }
        }

        #endregion
    }

    namespace EnumeratorExtensions
    {
        public static class MyExtensions
        {
            public static ARoutine Start(this IEnumerator routine)
            {
                return new ARoutine(routine);
            }
        }

        public class ARoutine : CustomYieldInstruction
        {
            public bool IsPlaying { get; private set; }

            public Coroutine Coroutine { get; }

            public ARoutine(IEnumerator routine)
            {
                IsPlaying = true;
                Coroutine = RoutinePlayer.Instance.StartCoroutine(WrapRoutine(routine));
            }

            private IEnumerator WrapRoutine(IEnumerator routine)
            {
                IsPlaying = true;
                yield return routine;
                IsPlaying = false;
            }

            public override bool keepWaiting => IsPlaying;

            public static implicit operator Coroutine(ARoutine routine)
            {
                return routine.Coroutine;
            }
        }

        public class RoutinePlayer : MonoBehaviour
        {
            private static RoutinePlayer _inst;

            public static RoutinePlayer Instance
            {
                get
                {
                    if (_inst) return _inst;

                    _inst = FindObjectOfType<RoutinePlayer>();

                    if (_inst == null)
                    {
                        var go = new GameObject("RoutinePlayer");
                        _inst = go.AddComponent<RoutinePlayer>();
                    }

                    Debug.Assert(_inst != null, "No RoutinePlayer in scene");

                    return _inst;
                }
            }
        }
    }
}