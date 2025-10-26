using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Buton için bu gerekli
using UnityEngine.SceneManagement; // Sahne geçişi için bu gerekli
using TMPro;

public class LevelsControls : MonoBehaviour
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
            SceneManager.LoadScene("Level 1");
        }
    }

    private void LoadNextScene()
    {
        // Butonu devre dışı bırak (kullanıcı sahneler yüklenirken tekrar tıklamasın)
        clickButton.interactable = false;
        
        // Belirtilen isimdeki sahneyi yükle
        SceneManager.LoadScene(sceneToLoad);
    }
}
