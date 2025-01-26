using System;
using System.Collections;
using UnityEngine;

namespace MyCharacter
{
    [RequireComponent(typeof(AudioSource))]
    public class WalkAudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip[] walkSounds;

        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            PlayerInteractionController.OnEnterZone += OnEnterZoneHandler;
        }

        private void OnDestroy()
        {
            PlayerInteractionController.OnEnterZone -= OnExitZoneHandler;
        }

        private void OnExitZoneHandler(Collider arg1, int arg2)
        {
            Unmute();
        }

        private void OnEnterZoneHandler(Collider arg1, int arg2)
        {
            if (arg2 != 0)
            {
                Mute();
            }
            else
            {
                StartCoroutine(Do());
            }

            return;

            IEnumerator Do()
            {
                yield return new WaitForSeconds(2);
                Mute();
            }
        }

        public void Mute()
        {
            _audioSource.volume = 0;
        }

        public void Unmute()
        {
            _audioSource.volume = 1;
        }

        public void Play()
        {
            _audioSource.clip = walkSounds[UnityEngine.Random.Range(0, walkSounds.Length)];
            _audioSource.Play();
        }
    }
}