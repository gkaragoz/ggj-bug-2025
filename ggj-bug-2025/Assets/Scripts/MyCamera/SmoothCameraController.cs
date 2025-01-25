using UnityEngine;

namespace MyCamera
{
    public class SmoothCameraController : MonoBehaviour
    {
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

        void Start()
        {
            if (target != null)
            {
                lastTargetPosition = target.position;
                currentOffset = baseOffset; // İlk başta ofseti temel değere ayarla
            }
        }

        void LateUpdate()
        {
            if (target == null) return;

            // 1. Hedefin hareket yönünü belirle
            float movementX = target.position.x - lastTargetPosition.x;

            if (Mathf.Abs(movementX) > 0.01f) // Yeterince hareket varsa
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
            if (!isMoving)
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
    }
}
