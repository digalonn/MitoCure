using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButtonManager : MonoBehaviour
{
    // Bu fonksiyonu butona bağlayacağız
    public void GoToPreviousScene()
    {
        // Hafızaya kaydettiğimiz sahne adını kontrol edelim
        if (!string.IsNullOrEmpty(SceneHistory.previousSceneName))
        {
            // Hafızada bir isim varsa, o sahneyi yükle
            SceneManager.LoadScene(SceneHistory.previousSceneName);
        }
        else
        {
            // Eğer hafıza boşsa (örn: oyuna direkt bu sahneden başladık,
            // Intro'dan gelmedik), ne yapacağına karar vermelisin.
            // Genellikle bir "Ana Menü" sahnesine yollanır.
            Debug.LogWarning("Önceki sahne bulunamadı! Ana Menü'ye yönlendiriliyor.");
            
            // "MainMenu" yazan yeri kendi ana menü sahnenizin adıyla değiştirin
            // Eğer bir ana menünüz yoksa bu satırı silebilir veya
            // SceneManager.LoadScene("IntroScene"); yazarak Intro'ya da yollayabilirsiniz.
            SceneManager.LoadScene("MainMenu"); 
        }
    }
}