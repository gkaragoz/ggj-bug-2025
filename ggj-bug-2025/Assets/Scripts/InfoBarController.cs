using System;
using DG.Tweening;
using MyLetterbox;
using UnityEngine;

namespace DefaultNamespace
{
    public class InfoBarController : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private float duration;

        private Sequence _showSequence;
        private const float ShowSize = 415f;
        private const float HideSize = 0;

        private void Awake()
        {
            rectTransform.sizeDelta = new Vector2(HideSize, rectTransform.sizeDelta.y);
            rectTransform.anchoredPosition = new Vector2(0f, -Letterbox.DefaultSize * 1.5f);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
                Show();
            else if (Input.GetKeyDown(KeyCode.L))
                Hide();
        }

        public void Show()
        {
            _showSequence?.Kill();
            _showSequence = DOTween.Sequence();
            _showSequence.Append(DOVirtual.Float(0f, ShowSize, duration, value =>
            {
                rectTransform.sizeDelta = new Vector2(value, rectTransform.sizeDelta.y);
            }).SetEase(Ease.OutExpo));
            _showSequence.Play();
        }

        public void Hide()
        {
            _showSequence?.Kill();
            _showSequence = DOTween.Sequence();
            _showSequence.Append(DOVirtual.Float(ShowSize, HideSize, duration, value =>
            {
                rectTransform.sizeDelta = new Vector2(value, rectTransform.sizeDelta.y);
            }).SetEase(Ease.InExpo));
            _showSequence.Play();
        }
    }
}