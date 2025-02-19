using System;
using UnityEngine;
using UnityEngine.UI;

namespace MyLetterbox
{
    public class Letterbox : MonoBehaviour
    {
        [SerializeField] private Image up;
        [SerializeField] private Image down;
        [SerializeField] private float transitionSpeed;

        private State _state;
        public const float DefaultSize = 70f;
        public const float CoveredSize = 500f;
        public const float ZoomInSize = 140f;

        public enum State
        {
            Default,
            Covered,
            ZoomIn
        }

        public void SetState(State state)
        {
            _state = state;
        }

        public void CompleteCurrentStateImmediately()
        {
            var target = new Vector2(up.rectTransform.sizeDelta.x, DefaultSize);
            
            switch (_state)
            {
                case State.Covered:
                    target = new Vector2(up.rectTransform.sizeDelta.x, CoveredSize);
                    break;
                case State.ZoomIn:
                    target = new Vector2(up.rectTransform.sizeDelta.x, ZoomInSize);
                    break;
            }
            
            up.rectTransform.sizeDelta = target;
            down.rectTransform.sizeDelta = target;
        }

        private void Update()
        {
            var current = up.rectTransform.sizeDelta;
            var target = new Vector2(up.rectTransform.sizeDelta.x, DefaultSize);
            
            switch (_state)
            {
                case State.Covered:
                    current = up.rectTransform.sizeDelta;
                    target = new Vector2(up.rectTransform.sizeDelta.x, CoveredSize);
                    break;
                case State.ZoomIn:
                    current = up.rectTransform.sizeDelta;
                    target = new Vector2(up.rectTransform.sizeDelta.x, ZoomInSize);
                    break;
            }
            
            up.rectTransform.sizeDelta = Vector2.Lerp(current, target, transitionSpeed * Time.deltaTime);
            down.rectTransform.sizeDelta = Vector2.Lerp(current, target, transitionSpeed * Time.deltaTime);
        }
    }
}