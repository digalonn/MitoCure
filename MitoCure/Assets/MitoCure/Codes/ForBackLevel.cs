using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // TextMeshPro için bu gerekli
using UnityEngine.UI; // Buton için bu gerekli


public class ForBackLevel : MonoBehaviour
{
    public Button clickButton; // Tıklama için kullanacağımız buton
    public string sceneToLoad; // Hikaye bitince yüklenecek ana oyun sahnesinin adı

    
    // using UnityEngine.SceneManagement; satırının en üstte ekli olduğundan emin olun

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
