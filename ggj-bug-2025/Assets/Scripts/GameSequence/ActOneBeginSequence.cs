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
            sequence.AppendCallback(() =>
            {
                gameTitle.DOFade(1, 3);
                gameSubTitle.DOFade(1, 3);
            });
            sequence.AppendCallback(() =>
            {
                phoneController.StartRinging();
            });
            sequence.AppendCallback(() =>
            {
                gameTitle.DOFade(0, 2);
                gameSubTitle.DOFade(0, 2);
            }).SetDelay(3);
            sequence.AppendCallback(() => letterbox.SetState(Letterbox.State.Default));
            sequence.Join(DOVirtual.DelayedCall(2, () =>
            {
                characterAnimator.SetTrigger("FallAnim");
            }));
            sequence.Append(DOVirtual.DelayedCall(10.5f, () => tutorialController.ShowTutorial()));
            sequence.Append(DOVirtual.DelayedCall(1f, () => characterController.canMove = true));
            sequence.OnComplete(() =>
            {
                ActOneState = -1;
            });
        }
    }
}