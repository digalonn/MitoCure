using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BolumEnvanterSlot : MonoBehaviour
{
    [Header("Bileşenler")]
    public Image urunIkonu;
    public TextMeshProUGUI miktarText;
    public Button tiklamaButonu;

    [Header("Veri")]
    public UrunData temsilEttigiUrun; // Inspector'dan atanacak (Havuç, Patates vs.)
    
    private BolumGorevManager gorevManager;

    void Start()
    {
        // Manager'ı bul
        gorevManager = FindObjectOfType<BolumGorevManager>();
        
        // Butona tıklandığında UrunuTeslimEt fonksiyonunu çağır
        tiklamaButonu.onClick.AddListener(UrunuTeslimEt);
        
        // Envanter güncellendiğinde bu slotun da güncellenmesi için abone ol
        EnvanterManager.instance.EnvanterGuncellendi += MiktarGuncelle;
        
        // İkonu ayarla
        if (temsilEttigiUrun != null)
        {
            urunIkonu.sprite = temsilEttigiUrun.urunIkonu;
            urunIkonu.enabled = true;
        }
        
        // İlk miktarı ayarla
        MiktarGuncelle();
    }

    void MiktarGuncelle()
    {
        if (temsilEttigiUrun == null) return;

        // EnvanterManager'dan bu ürünün miktarını al
        int miktar = EnvanterManager.instance.GetUrunMiktari(temsilEttigiUrun);
        miktarText.text = miktar.ToString();

        // Eğer envanterde ürün yoksa (miktar 0 ise) butonu tıklanamaz yap
        tiklamaButonu.interactable = (miktar > 0);
    }

    void UrunuTeslimEt()
    {
        if (gorevManager != null && temsilEttigiUrun != null)
        {
            // Görev yöneticisine bu ürünü teslim etmeyi denemesini söyle
            gorevManager.UrunTeslimEtmeyiDene(temsilEttigiUrun);
        }
    }
    
    // Aboneliği iptal et
    void OnDestroy()
    {
        if (EnvanterManager.instance != null)
        {
            EnvanterManager.instance.EnvanterGuncellendi -= MiktarGuncelle;
        }
    }
}