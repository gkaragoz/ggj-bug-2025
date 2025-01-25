using System;
using GameSequence;
using MyCharacter;
using MyLetterbox;
using UnityEngine;

namespace MyCamera
{
    [RequireComponent(typeof(Camera))]
    public class SmoothCameraController : MonoBehaviour
    {
        public static Action<int> OnEnterZone;
        public static Action<int> OnExitZone;
        
        public Camera selfCamera;
        public Camera stackCamera;
        public Letterbox letterbox;
        public Transform target; // Takip edilecek nesne
        public Vector3 baseOffset = new Vector3(0, 2, -10); // Temel ofset
        public float followSpeed = 5f; // Takip hızı
        public float offsetChangeSpeed = 1f; // Offset'in artış hızı
        public float maxOffsetX = 3f; // Maksimum X offset
        public float returnSpeed = 2f; // Offset'in sıfırlama hızı
        public float returnDelay = 0.5f; // Sıfırlama işlemine başlamadan önceki bekleme süresi
        public float easeInDuration = 1f; // Geri dönüş hızlanma süresi

        private Vector3 currentOffset; // Dinamik ofset
        private float currentOffsetX = 0f; // Offset'in X eksenindeki mevcut değeri
        private float direction = 0f; // Oyuncunun hareket yönü (-1 = sola, 1 = sağa)
        private Vector3 lastTargetPosition; // Hedefin bir önceki pozisyonu

        private float returnTimer = 0f; // Hareket durduğunda geri dönüş için bekleme zamanlayıcısı
        private bool isMoving = false; // Hedefin hareket edip etmediğini takip eder
        private float easeFactor = 0f; // Geri dönüş için hızlanma faktörü
        private bool forceReturn;
        
        private float _initialFov;
        private float _targetFov;
        private float _fovTransitionSpeed;
        private bool _isInFovZone;

        private int _lastEnteredZoneIndex = -1;
        
        private void Awake()
        {
            _initialFov = selfCamera.fieldOfView;
        }

        void Start()
        {
            if (target != null)
            {
                lastTargetPosition = target.position;
                currentOffset = baseOffset; // İlk başta ofseti temel değere ayarla
            }
            
            OnExitZone += OnExitZoneHandler;
        }

        private void OnExitZoneHandler(int obj)
        {
            _targetFov = _initialFov;
            _isInFovZone = false;
            forceReturn = false;
        }

        private void Update()
        {
            // Kamera şu anda bölgedeyse hedef FOV'a yumuşak geçiş yap
            if (_isInFovZone)
            {
                selfCamera.fieldOfView = Mathf.Lerp(selfCamera.fieldOfView, _targetFov, _fovTransitionSpeed * Time.deltaTime);
                stackCamera.fieldOfView = selfCamera.fieldOfView;
                
                if (ActOneBeginSequence.ActOneState == -1)
                    letterbox.SetState(Letterbox.State.ZoomIn);
            }
            // Kamera bölgeden çıktıysa başlangıç FOV'una geri dön
            else
            {
                selfCamera.fieldOfView = Mathf.Lerp(selfCamera.fieldOfView, _initialFov, _fovTransitionSpeed * Time.deltaTime);
                stackCamera.fieldOfView = selfCamera.fieldOfView;
                
                if (ActOneBeginSequence.ActOneState == -1)
                    letterbox.SetState(Letterbox.State.Default);
            }
        }

        void LateUpdate()
        {
            if (target == null) return;

            // 1. Hedefin hareket yönünü belirle
            float movementX = target.position.x - lastTargetPosition.x;

            if (Mathf.Abs(movementX) > 0.01f && !forceReturn) // Yeterince hareket varsa
            {
                isMoving = true;
                returnTimer = 0f; // Hareket olduğu için zamanlayıcıyı sıfırla
                direction = Mathf.Sign(movementX); // Hareket yönünü belirle (-1 ya da 1)
                currentOffsetX += direction * offsetChangeSpeed * Time.deltaTime; // Yöne bağlı olarak X ofsetini artır/azalt
                currentOffsetX = Mathf.Clamp(currentOffsetX, -maxOffsetX, maxOffsetX); // Maksimum/minimum değerler arasında sınırla
                easeFactor = 0f; // Ease faktörünü sıfırla, çünkü hareket ediyor
            }
            else
            {
                isMoving = false;
            }

            // 2. Hareket durduktan sonra zamanlayıcıyı başlat
            if (!isMoving || forceReturn)
            {
                returnTimer += Time.deltaTime;

                if (returnTimer >= returnDelay) // Bekleme süresi dolduysa sıfırlamaya başla
                {
                    easeFactor += Time.deltaTime / easeInDuration; // Ease faktörünü zamanla artır
                    easeFactor = Mathf.Clamp01(easeFactor); // 0 ile 1 arasında tut
                    float smoothStepFactor = Mathf.SmoothStep(0, 1, easeFactor); // Ease-in-out etkisi yarat

                    // Geri dönüş hızını smooth şekilde uygula
                    currentOffsetX = Mathf.Lerp(currentOffsetX, 0, smoothStepFactor * returnSpeed * Time.deltaTime);
                }
            }

            // 3. Dinamik ofseti güncelle
            currentOffset.x = baseOffset.x + currentOffsetX;

            // 4. Kamerayı smooth bir şekilde hedefin pozisyonuna taşı
            Vector3 desiredPosition = target.position + currentOffset;
            desiredPosition.y = baseOffset.y; // Y eksenini sabit tut
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

            // 5. Son hedef pozisyonunu güncelle
            lastTargetPosition = target.position;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Fov"))
            {
                var fovZone = other.GetComponent<CameraFovZone>();
                _targetFov = fovZone.targetFOV;
                _fovTransitionSpeed = fovZone.transitionSpeed;
                _isInFovZone = true;
                forceReturn = true;
                OnEnterZone?.Invoke(fovZone.fovZoneID);
                other.GetComponent<Collider>().enabled = false;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Fov"))
            {
                var fovZone = other.GetComponent<CameraFovZone>();
                _targetFov = _initialFov;
                _fovTransitionSpeed = fovZone.transitionSpeed;
                _isInFovZone = false;
                forceReturn = false;
            }
        }
    }
}
