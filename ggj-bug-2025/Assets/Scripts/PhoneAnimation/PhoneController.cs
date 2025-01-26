using System;
using System.Collections;
using DG.Tweening;
using GameSequence;
using UnityEngine;

namespace PhoneAnimation
{
    public class PhoneController : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Transform extraTarget;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioSource pickupSource;
        [SerializeField] private AudioSource releaseSource;
        [SerializeField] private Vector3 targetScale = new Vector3(-1.3f, 1.3f, 1.3f);
        [SerializeField] private float shakeStrength = 0.5f;
        [SerializeField] private CanvasGroup bubbleCanvasGroup;

        private Sequence _ringingSequence;
        private Coroutine _ringingCoroutine;
        private Vector3 _initialScale;
        private const float AudioDuration = 6.104f;
        private const float ShakeDuration = 3f;

        private void Awake()
        {
            _initialScale = target.localScale;
            ActOneBeginSequence.ShowPhoneBubbleAct += ShowPhoneBubbleAct;
        }

        public void StartRinging()
        {
            if (_ringingSequence != null)
                StopCoroutine(_ringingCoroutine);
            
            _ringingCoroutine = StartCoroutine(Do());
        }

        private IEnumerator Do()
        {
            while(true)
            {
                audioSource.Play();
                _ringingSequence?.Kill();
                _ringingSequence = DOTween.Sequence();
                _ringingSequence.Join(target.DOScale(targetScale, 0.1f));
                _ringingSequence.Append(target.DOShakeRotation(ShakeDuration, Vector3.one * shakeStrength));
                _ringingSequence.Join(extraTarget.DOShakeRotation(ShakeDuration, Vector3.one * shakeStrength));
                _ringingSequence.Append(target.DOScale(_initialScale, 0.1f));
                _ringingSequence.Play();

                yield return new WaitForSeconds(AudioDuration);
            }
        }

        private void ShowPhoneBubbleAct()
        {
            bubbleCanvasGroup.DOFade(1f, 0.2f);
        }

        public void StopRinging()
        {
            if (_ringingCoroutine != null)
                StopCoroutine(_ringingCoroutine);
            _ringingSequence?.Kill();
            audioSource.Stop();
            target.localScale = _initialScale;
            bubbleCanvasGroup.DOFade(0f, 0.2f);
        }

        public void PlayPickupSound()
        {
            pickupSource.Play();
        }

        public void PlayReleaseSound()
        {
            releaseSource.Play();
        }
    }
}