using UnityEngine;
using System; // Action (event) için gerekli

public class SeviyeManager : MonoBehaviour
{
    // --- Singleton Deseni ---
    public static SeviyeManager instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            // (İsteğe bağlı) Seviye bilgisi sahneler arası korunmalı
            // DontDestroyOnLoad(this.gameObject); 
        }
    }
    // --- Singleton Bitişi ---

    [SerializeField]
    private int mevcutSeviye = 1; // Oyuncu 1. seviyeden başlar

    // Seviye değiştiğinde UI gibi diğer sistemleri bilgilendirmek için event
    public event Action SeviyeGuncellendi;

    /// <summary>
    /// Oyuncunun mevcut seviyesini döndürür.
    /// </summary>
    public int GetMevcutSeviye()
    {
        return mevcutSeviye;
    }

    /// <summary>
    /// Oyuncunun seviyesini belirler. (Örn: 5. bölümü bitirdi, seviyesi 5 oldu).
    /// </summary>
    public void SeviyeAyarla(int yeniSeviye)
    {
        if (yeniSeviye <= mevcutSeviye) return; // Seviye düşürmeyi engelle

        mevcutSeviye = yeniSeviye;
        Debug.Log("Oyuncu seviye atladı! Yeni Seviye: " + mevcutSeviye);

        // SeviyeGuncellendi event'ini tetikle
        SeviyeGuncellendi?.Invoke();
    }

    /// <summary>
    /// Belirli bir eylem için gereken seviyenin yeterli olup olmadığını kontrol eder.
    /// </summary>
    /// <returns>Seviye yeterliyse true, değilse false döner.</returns>
    public bool SeviyeYeterliMi(int gerekenSeviye)
    {
        return mevcutSeviye >= gerekenSeviye;
    }
}