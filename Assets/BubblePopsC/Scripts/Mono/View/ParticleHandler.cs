using BubblePopsC.Scripts.Mono.Misc;

namespace BubblePopsC.Scripts.Mono.View
{
    using System.Threading;
    using UnityEngine;

    public class ParticleHandler : MonoBehaviour
    {
        public ParticleSystem particle;
        public Renderer particleRenderer;

        private CancellationTokenSource _cancellationTokenSource;
        private Transform _transform;

        private void Awake()
        {
            particleRenderer = particle.GetComponent<Renderer>();
            _transform = transform;
        }

        public void Play()
        {
            Refresh();
            particle.Play(true);
        }

        public void Stop()
        {
            Refresh();
            particle.Stop(true);
        }

        public void PlayAndDie()
        {
            particle.Play(true);

            DoWait.WaitWhile(() => particle.isPlaying, () => Destroy(gameObject));
        }

        public void Refresh()
        {
            gameObject.SetActive(true);
            particle.time = 0;
            particle.Clear(true);
            particle.Simulate(0, true, true);
        }

        public void DeActivate()
        {
            gameObject.SetActive(false);
        }

        public void SetParent(Transform parent)
        {
            _transform.SetParent(parent);
        }

        public void SetParentToRoot()
        {
            _transform.SetParent(null);
        }

        public void SetPosition(Vector2 pos)
        {
            _transform.position = pos;
        }

        public void SetName(string newName)
        {
            gameObject.name = newName;
        }
    }
}