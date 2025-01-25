using DG.Tweening;
using UnityEngine;

namespace PhoneAnimation
{
    public class PhoneController : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private Vector3 targetScale = new Vector3(-1.3f, 1.3f, 1.3f);
        [SerializeField] private float animationDuration = 0.5f;
        [SerializeField] private float shakeStrength = 0.5f;

        private Sequence _ringingSequence;
        private Vector3 _initialScale;
        
        public void StartRinging()
        {
            audioSource.Play();
            _initialScale = target.localScale;
            _ringingSequence?.Kill();
            _ringingSequence = DOTween.Sequence();
            _ringingSequence.Append(target.DOScale(targetScale, animationDuration));
            _ringingSequence.Join(target.DOShakeRotation(animationDuration, Vector3.one * shakeStrength));
            _ringingSequence.Append(target.DOScale(_initialScale, animationDuration));
            _ringingSequence.SetLoops(-1, LoopType.Restart);
            _ringingSequence.Play();
        }

        public void StopRinging()
        {
            audioSource.Stop();
            _ringingSequence?.Kill();
            target.localScale = _initialScale;
        }
    }
}