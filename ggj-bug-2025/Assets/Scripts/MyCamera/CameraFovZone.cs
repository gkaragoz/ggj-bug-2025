using UnityEngine;

namespace MyCamera
{
    [RequireComponent(typeof(BoxCollider))]
    public class CameraFovZone : MonoBehaviour
    {
        public float targetFOV = 20f; // Bu bölgeye girildiğinde kamera FOV'un ulaşacağı değer
        public float transitionSpeed = 2f; // FOV değişiminin hızı

        private void Awake()
        {
            GetComponent<BoxCollider>().isTrigger = true;
        }
    }
}