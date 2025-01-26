using DG.Tweening;
using MyLetterbox;
using PhoneAnimation;
using TMPro;
using Tutorial;
using UnityEngine;

namespace GameSequence
{
    public class ActOneBeginSequence : MonoBehaviour
    {
        public static int ActOneState = 0;
    
        public TMP_Text gameTitle;
        public TMP_Text gameSubTitle;
        public Letterbox letterbox;
        public Animator characterAnimator;
        public HorizontalCharacterController characterController;
        public PhoneController phoneController;
        public AudioSource bgMusic;
        public TutorialController tutorialController;

        private void Start()
        {
            letterbox.SetState(Letterbox.State.Covered);
            letterbox.CompleteCurrentStateImmediately();
        
            var sequence = DOTween.Sequence();
            sequence.Insert(1,DOVirtual.DelayedCall(0, () =>
            {
                gameTitle.DOFade(1, 3);
                gameSubTitle.DOFade(1, 3);
            }));
            sequence.Insert(2, DOVirtual.DelayedCall(0, () =>
            {
                phoneController.StartRinging();
            }));
            sequence.Insert(3, DOVirtual.DelayedCall(0, () =>
            {
                gameTitle.DOFade(0, 5);
                gameSubTitle.DOFade(0, 5);  
            }));
            sequence.Insert(5, DOVirtual.DelayedCall(0, () =>
            {
                letterbox.SetState(Letterbox.State.Default);
            }));
            sequence.Insert(5, DOVirtual.DelayedCall(0, () =>
            {
                characterAnimator.SetTrigger("FallAnim");
            }));
            sequence.Append(DOVirtual.DelayedCall(10.5f, () => tutorialController.ShowTutorial()));
            sequence.Append(DOVirtual.DelayedCall(2f, () => characterController.canMove = true));
            sequence.OnComplete(() =>
            {
                ActOneState = -1;
            });
        }
    }
}