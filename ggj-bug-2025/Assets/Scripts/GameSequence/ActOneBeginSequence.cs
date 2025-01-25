using DG.Tweening;
using MyLetterbox;
using TMPro;
using UnityEngine;

namespace GameSequence
{
    public class ActOneBeginSequence : MonoBehaviour
    {
        public static int ActOneState = 0;
    
        public TMP_Text gameTitle;
        public Letterbox letterbox;
        public Animator characterAnimator;
        public HorizontalCharacterController characterController;

        private void Start()
        {
            letterbox.SetState(Letterbox.State.Covered);
            letterbox.CompleteCurrentStateImmediately();
        
            var sequence = DOTween.Sequence();
            sequence.Append(gameTitle.DOFade(1, 3));
            sequence.Append(gameTitle.DOFade(0, 2)).SetDelay(3);
            sequence.AppendCallback(() => letterbox.SetState(Letterbox.State.Default));
            sequence.Join(DOVirtual.DelayedCall(2, () => characterAnimator.SetTrigger("FallAnim")));
            sequence.Append(DOVirtual.DelayedCall(12.5f, () => characterController.canMove = true));
            sequence.OnComplete(() =>
            {
                ActOneState = -1;
            });
        }
    }
}