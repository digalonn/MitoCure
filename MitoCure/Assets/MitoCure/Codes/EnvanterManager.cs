using System.Collections.Generic; // Dictionary için gerekli
using UnityEngine;
using System; // Action (event) için gerekli

public class EnvanterManager : MonoBehaviour
{
    // --- Singleton Deseni ---
    // Bu, script'e EnvanterManager.instance şeklinde her yerden erişmemizi sağlar
    public static EnvanterManager instance;

    void Awake()
    {
        // Yalnızca bir tane EnvanterManager olduğundan emin ol
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            // (İsteğe bağlı) Sahne değişse bile envanterin korunması için
            // DontDestroyOnLoad(this.gameObject); 
        }
    }
    // --- Singleton Bitişi ---

    // Ana envanter verimiz. Hangi üründen (UrunData) kaç adet (int) olduğunu tutar.
    public Dictionary<UrunData, int> envanterSlotlari = new Dictionary<UrunData, int>();

    // Envanterde bir değişiklik olduğunda UI'ı (Kullanıcı Arayüzü)
    // bilgilendirmek için bir event (olay) tanımlıyoruz.
    public event Action EnvanterGuncellendi;


    /// <summary>
    /// Envantere belirtilen üründen belirtilen miktarda ekler.
    /// </summary>
    /// <param name="urun">Eklenecek ürünün ScriptableObject'i</param>
    /// <param name="miktar">Eklenecek adet</param>
    public void UrunEkle(UrunData urun, int miktar)
    {
        if (urun == null) return;

        // 1. Ürün envanterde zaten var mı?
        if (envanterSlotlari.ContainsKey(urun))
        {
            // Varsa, miktarını arttır
            envanterSlotlari[urun] += miktar;
        }
        else
        {
            // Yoksa, sözlüğe yeni bir kayıt olarak ekle
            envanterSlotlari.Add(urun, miktar);
        }

        Debug.Log(miktar + " adet " + urun.urunAdi + " eklendi. Toplam: " + envanterSlotlari[urun]);

        // 2. EnvanterGuncellendi event'ini tetikle
        // UI script'leri bu event'i dinleyerek kendilerini güncelleyebilir.
        EnvanterGuncellendi?.Invoke();
    }

    /// <summary>
    /// Envanterden belirtilen üründen belirtilen miktarda kullanır/siler.
    /// </summary>
    /// <returns>Eğer işlem başarılıysa true, envanterde yeterli ürün yoksa false döner.</returns>
    public bool UrunKullan(UrunData urun, int miktar)
    {
        if (urun == null) return false;

        // 1. Ürün envanterde var mı VE yeterli miktarda var mı?
        if (envanterSlotlari.ContainsKey(urun) && envanterSlotlari[urun] >= miktar)
        {
            // Varsa, miktarını azalt
            envanterSlotlari[urun] -= miktar;

            // 2. Eğer miktar 0'a düşerse, ürünü sözlükten kaldır (isteğe bağlı ama temiz)
            if (envanterSlotlari[urun] <= 0)
            {
                envanterSlotlari.Remove(urun);
            }

            Debug.Log(miktar + " adet " + urun.urunAdi + " kullanıldı.");
            EnvanterGuncellendi?.Invoke();
            return true; // İşlem başarılı
        }
        else
        {
            Debug.LogWarning("Envanterde yeterli " + urun.urunAdi + " yok!");
            return false; // İşlem başarısız
        }
    }

    /// <summary>
    /// Belirli bir ürünün envanterdeki miktarını döndürür.
    /// </summary>
    public int GetUrunMiktari(UrunData urun)
    {
        if (envanterSlotlari.ContainsKey(urun))
        {
            return envanterSlotlari[urun];
        }
        else
        {
            return 0; // O üründen envanterde hiç yok
        }
    }
}