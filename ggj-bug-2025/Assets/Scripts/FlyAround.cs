using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class FlyAround : MonoBehaviour
    {
        [SerializeField] private Transform center; // Çemberin merkezi
        [SerializeField] private float radius = 5f; // Çemberin yarıçapı
        [SerializeField] private float speed = 2f; // Hareket hızı
        [SerializeField] private float moveInterval = 1f; // Hareketler arasındaki süre

        private Vector3 targetPosition; // Rastgele bir hedef pozisyon
        private float timer; // Süre sayacı

        void Start()
        {
            // Başlangıçta bir hedef pozisyon seç
            SetNewTargetPosition();
        }

        void Update()
        {
            // Zamanlayıcıyı güncelle
            timer += Time.deltaTime;
            if (timer >= moveInterval)
            {
                // Yeni bir hedef pozisyon belirle
                SetNewTargetPosition();
                timer = 0f; // Zamanlayıcıyı sıfırla
            }

            // Hedefe doğru hareket et
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }

        void SetNewTargetPosition()
        {
            if (center == null)
            {
                Debug.LogWarning("Center transform is not assigned!");
                return;
            }

            // Çemberin içindeki rastgele bir pozisyonu belirle
            float randomAngle = UnityEngine.Random.Range(0f, 360f); // 0-360 derece arasında bir açı
            float randomDistance = UnityEngine.Random.Range(0f, radius); // 0 ile yarıçap arasında bir mesafe

            float x = center.position.x + Mathf.Cos(randomAngle * Mathf.Deg2Rad) * randomDistance * center.lossyScale.x;
            float y = center.position.y + Mathf.Sin(randomAngle * Mathf.Deg2Rad) * randomDistance * center.lossyScale.y;
            float z = center.position.z; // Sabit Z değeri (UI olduğu için)

            targetPosition = new Vector3(x, y, z);
        }
    }
}