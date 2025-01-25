using UnityEngine;

namespace MyCamera
{
    [RequireComponent(typeof(BoxCollider))]
    public class CameraFovZone : MonoBehaviour
    {
        public float targetFOV = 20f; // Bu bölgeye girildiğinde kamera FOV'un ulaşacağı değer
        public float transitionSpeed = 2f; // FOV değişiminin hızı
        private float initialFOV; // Kameranın başlangıç FOV değeri
        private bool isInZone = false; // Kamera şu anda bu bölgede mi?
        private Camera[] _cameras; // Kamera referansları
        private float currentFOV => _cameras != null ? _cameras[0].fieldOfView : 0;
        
        private void Awake()
        {
            _cameras = FindObjectsOfType<Camera>();
        }

        void Start()
        {
            if (_cameras != null)
                initialFOV = _cameras[0].fieldOfView; // Kameranın ilk FOV değerini kaydet

            // Box Collider'ı trigger olarak ayarla
            BoxCollider boxCollider = GetComponent<BoxCollider>();
            boxCollider.isTrigger = true;
        }

        void Update()
        {
            if (_cameras == null) return;

            // Kamera şu anda bölgedeyse hedef FOV'a yumuşak geçiş yap
            if (isInZone)
                SetCameraFov(Mathf.Lerp(currentFOV, targetFOV, transitionSpeed * Time.deltaTime));
            // Kamera bölgeden çıktıysa başlangıç FOV'una geri dön
            else
                SetCameraFov(Mathf.Lerp(currentFOV, initialFOV, transitionSpeed * Time.deltaTime));
        }

        private void SetCameraFov(float fov)
        {
            if (_cameras == null) return;

            foreach (Camera cam in _cameras)
                cam.fieldOfView = fov;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("MainCamera")) // Eğer etkileşime giren ana kameraysa
                isInZone = true; // Kamera bölgeye girdi
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("MainCamera")) // Eğer etkileşime giren ana kameraysa
                isInZone = false; // Kamera bölgeden çıktı
        }
    }
}