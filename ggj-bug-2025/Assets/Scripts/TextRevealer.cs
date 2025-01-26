using System.Collections;
using UnityEngine;
using TMPro;

namespace MyNamespace
{
    public class TextRevealer : MonoBehaviour
    {
        public TextMeshProUGUI textComponent; // TextMeshPro nesnesi
        public float revealSpeed = 0.05f;     // Harf başına geçen süre

        private string fullText;              // Tam metin
        private bool isRevealing = false;     // Animasyon çalışıyor mu?

        void Start()
        {
            // Eğer metin ayarlanmışsa otomatik başlat
            if (textComponent != null)
            {
                fullText = textComponent.text; // TextMeshPro'daki tam metni al
                textComponent.text = "";       // Önce boş yap
                StartCoroutine(RevealText());
            }
        }

        public IEnumerator RevealText()
        {
            isRevealing = true;

            for (int i = 0; i <= fullText.Length; i++)
            {
                textComponent.text = fullText.Substring(0, i); // İlk i harfi göster
                yield return new WaitForSeconds(revealSpeed);  // Belirlenen süre kadar bekle
            }

            isRevealing = false;
        }

        // Eğer farklı bir metin için tekrar çalıştırmak istersen
        public void StartReveal(string newText)
        {
            if (!isRevealing)
            {
                fullText = newText;
                textComponent.text = "";
                StartCoroutine(RevealText());
            }
        }

        public void HideText()
        {
            textComponent.text = "";
        }
    }   
}