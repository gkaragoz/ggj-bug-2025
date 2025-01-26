using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class TutorialController : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Sprite defaultSprite;
        [SerializeField] private Sprite aHighlightedSprite;
        [SerializeField] private Sprite dHighlightedSprite;

        private Sequence _sequence;

        private void Awake()
        {
            var imageColor = image.color;
            imageColor.a = 0;
            image.color = imageColor;
            image.sprite = defaultSprite;
        }

        private void OnEnable()
        {
            HorizontalCharacterController.OnWalkTutorialFinished += HideTutorial;
        }

        private void OnDisable()
        {
            HorizontalCharacterController.OnWalkTutorialFinished -= HideTutorial;
        }

        public void ShowTutorial() 
        {
            gameObject.SetActive(true);
            image.DOFade(1f, 0.2f);
            
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence.InsertCallback(0f, () =>
            {
                image.sprite = defaultSprite;
            });
            _sequence.InsertCallback(0.5f, () =>
            {
                image.sprite = dHighlightedSprite;
            });
            _sequence.InsertCallback(2f, () =>
            {
                image.sprite = defaultSprite;
            });
            _sequence.InsertCallback(2.5f, () =>
            {
                image.sprite = aHighlightedSprite;
            });
            _sequence.InsertCallback(4f, () =>
            {
                image.sprite = aHighlightedSprite;
            });

            _sequence.SetLoops(-1, LoopType.Restart);
            _sequence.Play();
        }

        private void HideTutorial()
        {
            _sequence?.Kill();

            if (image.color.a != 0)
            {
                image.DOFade(0f, 0.2f)
                    .OnComplete(() =>
                    {
                        gameObject.SetActive(false);
                        enabled = false;        
                    });    
            }
            else
            {
                gameObject.SetActive(false);
                enabled = false;
            }
        }
    }
}