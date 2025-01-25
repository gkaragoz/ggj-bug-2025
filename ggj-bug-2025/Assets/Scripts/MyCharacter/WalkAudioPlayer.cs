using System;
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
        }

        public void Play()
        {
            _audioSource.clip = walkSounds[UnityEngine.Random.Range(0, walkSounds.Length)];
            _audioSource.Play();
        }
    }
}