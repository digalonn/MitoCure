using UnityEngine;
using System.Collections; // Coroutine için bu kütüphane gerekli

public class TarlaPlot : MonoBehaviour
{
    [Header("Tarla Sprite'ları")]
    public Sprite surulmemisToprakSprite; // Yabani otlu, ham toprak (Başlangıç)
    public Sprite ekilmisToprakSprite;    // İçinde tohum varken (Büyüme aşaması)
    public Sprite surulmusToprakSprite;   // Hasattan sonra (Ekime hazır)

    [Header("Durum Değişkenleri")]
    private SpriteRenderer spriteRenderer;
    private bool ekiliMi = false;
    private bool olgunlastiMi = false;

    // Diğer script'lerden (TohumData) gelen bilgiler
    private TohumData ekiliTohum;
    private GameObject ekiliBitkiObjesi;
    private SpriteRenderer bitkiSpriteRenderer; // Bitkinin kendi sprite'ı
    private Coroutine buyumeCoroutini;
    

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Oyun başladığında tarla "sürülmemiş" ham toprak olarak başlar.
        ekiliMi = false;
        olgunlastiMi = false;
        spriteRenderer.sprite = surulmemisToprakSprite;
    }

    /// <summary>
    /// DraggableItem script'inden (tohumu sürüklediğimizde) çağrılır.
    /// Tarlayı otomatik olarak eker.
    /// </summary>
    public bool TohumEk(TohumData ekilecekTohum)
    {
        // 1. Tarlada zaten ekili bir şey varsa, ekime izin verme
        if (ekiliMi)
        {
            Debug.Log("Bu tarla zaten ekili!");
            return false;
        }

        // 2. Tarla boş (sürülmüş veya sürülmemiş olması fark etmez).
        // Tıklamaya gerek kalmadan, sürükleme işlemi tarlayı "ekili" hale getirir.
        
        ekiliMi = true;
        olgunlastiMi = false;
        ekiliTohum = ekilecekTohum;

        // Tarlanın zemin görüntüsünü "ekilmiş" olarak ayarla
        if (ekilmisToprakSprite != null)
        {
            spriteRenderer.sprite = ekilmisToprakSprite;
        }

        // Bitki objesini oluştur ve SpriteRenderer'ını al
        if (ekilecekTohum.bitkiPrefabi != null)
        {
            ekiliBitkiObjesi = Instantiate(ekilecekTohum.bitkiPrefabi, transform.position, Quaternion.identity);
            ekiliBitkiObjesi.transform.SetParent(this.transform);
            bitkiSpriteRenderer = ekiliBitkiObjesi.GetComponent<SpriteRenderer>();
        }

        Debug.Log(ekiliTohum.tohumAdi + " tarlaya ekildi. Büyüme başlıyor...");

        // Büyüme sürecini başlat
        buyumeCoroutini = StartCoroutine(BuyumeSureci());

        return true; 
    }

    /// <summary>
    /// Bitkinin büyüme aşamalarını yönetir.
    /// "Hasata hazır" hal, bitkinin son büyüme sprite'ıdır.
    /// </summary>
    private IEnumerator BuyumeSureci()
    {
        // TohumData (ScriptableObject) içinde "Buyume Asamalari" dizisi ayarlı mı?
        if (ekiliTohum.buyumeAsamalari == null || ekiliTohum.buyumeAsamalari.Length == 0)
        {
            Debug.LogError(ekiliTohum.tohumAdi + " için 'Buyume Asamalari' sprite dizisi atanmamış! Lütfen TohumData dosyasını kontrol edin.");
            yield break; // Coroutine'i durdur
        }

        int asamaSayisi = ekiliTohum.buyumeAsamalari.Length;
        float surePerAsama = ekiliTohum.buyumeSuresiSaniye / (float)asamaSayisi;

        for (int i = 0; i < asamaSayisi; i++)
        {
            // Bitkinin sprite'ını o anki aşamanın sprite'ı yap
            if (bitkiSpriteRenderer != null)
            {
                bitkiSpriteRenderer.sprite = ekiliTohum.buyumeAsamalari[i];
            }
            
            // Bir sonraki aşamaya kadar bekle
            yield return new WaitForSeconds(surePerAsama);
        }

        // Büyüme tamamlandı
        olgunlastiMi = true;
        buyumeCoroutini = null;

        // "Hasata hazır" hal, bitkinin aldığı son sprite halidir.
        // (ekiliTohum.buyumeAsamalari dizisindeki son eleman)
        Debug.Log(ekiliTohum.tohumAdi + " olgunlaştı, hasat edilebilir!");
    }

    /// <summary>
    /// Tarlaya tıklandığında çalışır.
    /// Artık SADECE hasat etmek için kullanılır.
    /// </summary>
    private void OnMouseDown()
    {
        // 1. ÖNCELİK: Hasat
        // Eğer tarla ekili VE bitki olgunlaşmışsa
        if (ekiliMi && olgunlastiMi)
        {
            HasatEt();
            return; // İşlemi bitir
        }

        // 2. Diğer Durumlar (Sadece bilgi verme)
        if (ekiliMi && !olgunlastiMi)
        {
            Debug.Log("Bitki henüz büyümedi!");
        }
        else if (!ekiliMi)
        {
            // Tarla ya "sürülmemiş" ya da "sürülmüş" (boş) haldedir
            Debug.Log("Bu tarlaya ekim yapmak için bir tohum sürükleyin.");
        }
    }

    /// <summary>
    /// Bitkiyi hasat eder ve tarlayı "ekime hazır" (sürülmüş) hale döndürür.
    /// </summary>
    public void HasatEt()
    {
        if (!ekiliMi || !olgunlastiMi) return; // Güvenlik kontrolü

        // Envantere ürünü ekle
        if (ekiliTohum.hasatEdilenUrunData != null)
        {
            EnvanterManager.instance.UrunEkle(
                ekiliTohum.hasatEdilenUrunData, 
                ekiliTohum.hasatMiktari
            );
        }
        
        // Bitki objesini yok et
        Destroy(ekiliBitkiObjesi); 

        // Coroutine çalışıyorsa durdur
        if (buyumeCoroutini != null)
        {
            StopCoroutine(buyumeCoroutini);
            buyumeCoroutini = null;
        }

        // --- TARLA SIFIRLAMA ---
        // Tarlayı temizle, durumu "Sürülmüş" (Ekime Hazır) olarak ayarla
        ekiliMi = false;
        olgunlastiMi = false;
        ekiliTohum = null;
        bitkiSpriteRenderer = null;

        // Tarlayı "sürülmüş" (ekime hazır) sprite'a döndür
        spriteRenderer.sprite = surulmusToprakSprite; 
    }
}