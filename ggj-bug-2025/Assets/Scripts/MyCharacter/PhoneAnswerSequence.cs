using System;
using System.Collections;
using DG.Tweening;
using MyCamera;
using UnityEngine;
using UnityEngine.Playables;

namespace MyCharacter
{
    public class PhoneAnswerSequence : MonoBehaviour
    {

        public PlayableDirector phoneAnswerDirector;
        public HorizontalCharacterController characterController;
        public Transform animationHolderParent;
        public int zoneIndex;
        private void Awake()
        {
            PlayerInteractionController.OnEnterZone += OnEnterZoneHandler;
        }

        private void OnEnterZoneHandler(Collider col,int index)
        {
            if (index != zoneIndex)
            {
                return;
            }
            
            StartCoroutine(Do());
            return;
            IEnumerator Do()
            {
                characterController.canMove = false;
                characterController.transform.SetParent(animationHolderParent);
                yield return null;
                phoneAnswerDirector.Play();
                float duration = (float)phoneAnswerDirector.duration;
                DOVirtual.DelayedCall(duration, () =>
                {
                    PlayerInteractionController.OnExitZone?.Invoke(col,zoneIndex);
                    var pos = characterController.transform.position;
                    pos.z = 2.2f;
                    characterController.transform.SetParent(null);
                    characterController.transform.position = pos;
                    characterController.canMove = true;
                });
            }

        }

        private void OnDestroy()
        {
            PlayerInteractionController.OnEnterZone -= OnEnterZoneHandler;
        }
    }
}
