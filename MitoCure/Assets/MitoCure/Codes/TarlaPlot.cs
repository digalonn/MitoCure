using UnityEngine;
using System.Collections; // Coroutine için bu kütüphane gerekli

public class TarlaPlot : MonoBehaviour
{
    public bool ekiliMi = false;
    private TohumData ekiliTohum;
    private GameObject ekiliBitkiObjesi;

    // Sprite'ı değiştirmek için
    private SpriteRenderer spriteRenderer; 
    public Sprite surulmusToprakSprite; 
    public Sprite ekilmisToprakSprite; 

    // --- YENİ EKLENEN KISIM ---
    private SpriteRenderer bitkiSpriteRenderer; // Ekilen bitkinin sprite'ını değiştirmek için
    private Coroutine buyumeCoroutini;
    private bool olgunlastiMi = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = surulmusToprakSprite;
    }

    // DraggableItem script'inden çağrılacak fonksiyon
    public bool TohumEk(TohumData ekilecekTohum)
    {
        if (ekiliMi)
        {
            Debug.Log("Bu tarla zaten ekili!");
            return false;
        }

        // Tarla boş, ekim yap
        ekiliMi = true;
        olgunlastiMi = false; // Yeni ekildi, olgun değil
        ekiliTohum = ekilecekTohum;

        if (ekilmisToprakSprite != null)
        {
            spriteRenderer.sprite = ekilmisToprakSprite;
        }

        if (ekilecekTohum.bitkiPrefabi != null)
        {
            ekiliBitkiObjesi = Instantiate(ekilecekTohum.bitkiPrefabi, transform.position, Quaternion.identity);
            ekiliBitkiObjesi.transform.SetParent(this.transform);
            
            // Bitkinin SpriteRenderer'ını al
            bitkiSpriteRenderer = ekiliBitkiObjesi.GetComponent<SpriteRenderer>();
        }

        Debug.Log(ekiliTohum.tohumAdi + " tarlaya ekildi. Büyüme başlıyor...");

        // Büyüme sürecini başlat
        buyumeCoroutini = StartCoroutine(BuyumeSureci());

        return true; 
    }

    // --- YENİ FONKSİYON: BÜYÜME SÜRECİ (COROUTINE) ---
    private IEnumerator BuyumeSureci()
    {
        // Tohum datasında büyüme aşaması var mı kontrol et
        if (ekiliTohum.buyumeAsamalari == null || ekiliTohum.buyumeAsamalari.Length == 0)
        {
            Debug.LogError(ekiliTohum.tohumAdi + " için büyüme aşaması sprite'ı atanmamış!");
            yield break; // Coroutine'i durdur
        }

        int asamaSayisi = ekiliTohum.buyumeAsamalari.Length;
        // Her aşama ne kadar sürecek?
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
        buyumeCoroutini = null; // Coroutine bitti
        Debug.Log(ekiliTohum.tohumAdi + " olgunlaştı, hasat edilebilir!");
    }

    // --- YENİ FONKSİYON: TIKLAYARAK HASAT ETME ---
    // Bu fonksiyonun çalışması için Tarla objesinde bir Collider2D (Box Collider 2D) olmalıdır!
    // (Zaten sürükle-bırak için eklemiştik)
    private void OnMouseDown()
    {
        // Eğer tarla ekili VE bitki olgunlaşmışsa
        if (ekiliMi && olgunlastiMi)
        {
            HasatEt();
        }
        else if (ekiliMi && !olgunlastiMi)
        {
            Debug.Log("Bitki henüz büyümedi!");
        }
        else
        {
            Debug.Log("Tarlada ekili bir şey yok.");
        }
    }


    // (Daha önce oluşturduğumuz) HasatEt fonksiyonunu güncelleyelim
    public void HasatEt()
    {
        if (!ekiliMi || !olgunlastiMi) return; // Güvenlik kontrolü

        Debug.Log(ekiliTohum.tohumAdi + " hasat edildi! Envantere " + ekiliTohum.hasatMiktari + " adet eklendi.");
        
        // --- TODO: Envanter Yönetimi ---
        // Burada envanter sisteminize hasat edilen ürünü eklersiniz.
        // Örnek:
        // EnvanterManager.instance.ItemEkle(ekiliTohum.hasatEdilenUrunPrefabi, ekiliTohum.hasatMiktari);

        // Tarlayı temizle
        Destroy(ekiliBitkiObjesi); // Bitki objesini yok et
        
        // Coroutine çalışıyorsa (bir hata olduysa) durdur
        if (buyumeCoroutini != null)
        {
            StopCoroutine(buyumeCoroutini);
            buyumeCoroutini = null;
        }

        // Tarla durumunu sıfırla
        ekiliMi = false;
        olgunlastiMi = false;
        ekiliTohum = null;
        bitkiSpriteRenderer = null;
        spriteRenderer.sprite = surulmusToprakSprite; // Tarlayı boş (sürülmüş) hale getir
    }
}