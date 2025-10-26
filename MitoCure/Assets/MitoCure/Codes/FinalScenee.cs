using UnityEngine;
using UnityEngine.UI; // Buton için bu gerekli
using UnityEngine.SceneManagement; // Sahne geçişi için bu gerekli
using TMPro; // TextMeshPro için bu gerekli


public class FinalScenee : MonoBehaviour
{
  [Header("UI Elemanları")]
    public TextMeshProUGUI storyTextComponent; // Yazıyı göstereceğimiz Text objesi
    public Button clickButton; // Tıklama için kullanacağımız buton

    [Header("Hikaye Ayarları")]
    [TextArea(3, 10)] // Inspector'da daha rahat yazı yazmak için
    public string[] storyLines; // Gösterilecek tüm hikaye cümleleri
    
    [Header("Sahne Geçişi")]
    public string sceneToLoad; // Hikaye bitince yüklenecek ana oyun sahnesinin adı

    private int currentLineIndex = 0; // Şu an hangi cümlede olduğumuzu takip eder

    void Start()
    {
        // Butonun OnClick event'ine programatik olarak fonksiyonumuzu atıyoruz.
        // Bunu Inspector'dan da yapabilirdik ama bu yöntem daha garantidir.
        clickButton.onClick.AddListener(OnScreenClick);

        // Oyunu ilk cümleyle başlat
        if (storyLines.Length > 0)
        {
            storyTextComponent.text = storyLines[currentLineIndex];
        }
        else
        {
            Debug.LogWarning("IntroController: Gösterilecek hikaye cümlesi bulunamadı!");
            LoadNextScene(); // Hikaye yoksa direkt diğer sahneye geç
        }
    }

    // Butona tıklandığında bu fonksiyon çalışacak
    public void OnScreenClick()
    {
        // Bir sonraki cümleye geç
        currentLineIndex++;

        // Hâlâ gösterecek cümlemiz var mı?
        if (currentLineIndex < storyLines.Length)
        {
            // Varsa, Text objesini güncelle
            storyTextComponent.text = storyLines[currentLineIndex];
        }
        else
        {
            // Cümleler bittiyse, ana oyun sahnesine geç
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void LoadNextScene()
    {
        // Butonu devre dışı bırak
        clickButton.interactable = false;
    
        // --- YENİ EKLENEN SATIR ---
        // Yeni sahneyi yüklemeden önce, şu anki sahnenin (yani IntroScene) adını
        // hafızaya alıyoruz.
        SceneHistory.previousSceneName = SceneManager.GetActiveScene().name;
        // --------------------------

        // Belirtilen isimdeki sahneyi yükle
        SceneManager.LoadScene(sceneToLoad);
    }  
  
}
