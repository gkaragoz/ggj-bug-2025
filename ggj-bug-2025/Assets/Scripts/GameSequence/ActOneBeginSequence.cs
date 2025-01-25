using System;
using DG.Tweening;
using MyCamera;
using TMPro;
using UnityEngine;

public class ActOneBeginSequence : MonoBehaviour
{
    public TMP_Text gameTitle;

    public RectTransform upLetterBox;
    public RectTransform downLetterBox;
    public Animator characterAnimator;
    public ParticleSystem smokeEffect;
    public HorizontalCharacterController characterController;
    public float smokeDelay = 2;
    public SmoothCameraController smoothCameraController;

    private void Awake()
    {
       // smoothCameraController.enabled = false;
    }

    private void Start()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(gameTitle.DOFade(1, 3));
        sequence.Append(gameTitle.DOFade(0, 2)).SetDelay(3);
        sequence.Append(DOVirtual.Float(upLetterBox.rect.height, 70, 1,
            value => upLetterBox.sizeDelta = new Vector2(upLetterBox.sizeDelta.x, value)));
        sequence.Join(DOVirtual.Float(downLetterBox.rect.height, 70, 1,
            value => downLetterBox.sizeDelta = new Vector2(downLetterBox.sizeDelta.x, value)));
         sequence.Join(DOVirtual.DelayedCall(2, () => characterAnimator.SetTrigger("FallAnim")));
         sequence.Append(DOVirtual.DelayedCall(12.5f, () => characterController.canMove = true));


        // sequence.Append(DOVirtual.DelayedCall(2, () => characterAnimator.SetTrigger("StartSmoking")));
        // sequence.Append(DOVirtual.DelayedCall(smokeDelay, () => smokeEffect.Play()));
        // sequence.Append(DOVirtual.DelayedCall(4, () => smokeEffect.Stop()));
        // sequence.Append(DOVirtual.DelayedCall(5, () => characterAnimator.SetTrigger("idleTrigger")).OnComplete(() =>
        // {
        //     smokeEffect.Stop();
        //     characterController.canMove = true;
        //     characterController.Flip();
        //     smoothCameraController.enabled = true;
        // }));
    }
}