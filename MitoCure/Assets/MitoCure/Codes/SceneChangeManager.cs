using UnityEngine;
using UnityEngine.SceneManagement;

// Bu script, "Geri Dön" butonları HARİÇ, 
// diğer tüm sahne geçiş butonlarında (Level1 -> Farm, Level2 -> Farm vb.) kullanılabilir.
public class SceneChangeManager : MonoBehaviour
{
    // Bu fonksiyonu butona bağlayacağız.
    // Hangi sahneye gideceğini Inspector'daki OnClick event'inden yazacağız.
    public void GoToScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Sahne adı (sceneName) boş! Butonun OnClick event'inden gidilecek sahnenin adını yazdığınıza emin olun.");
            return;
        }

        // 1. Önceki sahneyi (yani şu anki aktif sahneyi) hafızaya al.
        //    (örn: Level1'deysek "Level1" kaydedilecek)
        SceneHistory.previousSceneName = SceneManager.GetActiveScene().name;
        
        // Konsola yazdırarak kontrol edelim
        Debug.Log("Önceki sahne kaydedildi: " + SceneHistory.previousSceneName);

        // 2. Yeni hedef sahneyi (örn: "Farm") yükle.
        SceneManager.LoadScene(sceneName);
    }
}