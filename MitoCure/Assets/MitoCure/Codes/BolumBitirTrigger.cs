// Örnek bir BolumBitirTrigger.cs script'i
using UnityEngine;

public class BolumBitirTrigger : MonoBehaviour
{
    public int tamamlananBolumSeviyesi = 1; // Bu trigger 1. bölümün bittiğini belirtir

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Oyuncu çarptıysa
        {
            // SeviyeManager'a yeni seviyenin (1+1=2) olduğunu bildir.
            // Bu, 2. seviye tohumlarının kilidini açar.
            SeviyeManager.instance.SeviyeAyarla(tamamlananBolumSeviyesi + 1);
            
            // (Bu trigger'ı tekrar çalışmasın diye kapat)
            gameObject.SetActive(false); 
        }
    }
}