using UnityEngine;

public class TarlaPlot : MonoBehaviour
{
    public bool ekiliMi = false;
    private TohumData ekiliTohum;
    private GameObject ekiliBitkiObjesi;

    // Sprite'ı değiştirmek için
    private SpriteRenderer spriteRenderer; 
    public Sprite surulmusToprakSprite; // Inspector'dan atayın
    public Sprite ekilmisToprakSprite; // Inspector'dan atayın

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Başlangıçta sürülmüş (boş) toprak olsun
        if (surulmusToprakSprite != null)
        {
            spriteRenderer.sprite = surulmusToprakSprite;
        }
    }

    // DraggableItem script'inden çağrılacak fonksiyon
    public bool TohumEk(TohumData ekilecekTohum)
    {
        // Eğer tarla zaten ekili ise, tekrar ekme
        if (ekiliMi)
        {
            Debug.Log("Bu tarla zaten ekili!");
            return false;
        }

        // Tarla boş, ekim yap
        ekiliMi = true;
        ekiliTohum = ekilecekTohum;

        // 1. Tarlanın görünümünü değiştir (ekilmiş toprak)
        if (ekilmisToprakSprite != null)
        {
            spriteRenderer.sprite = ekilmisToprakSprite;
        }

        // 2. Tohumun bitki prefabını tarlanın üzerine oluştur
        if (ekilecekTohum.bitkiPrefabi != null)
        {
            // Bitkiyi bu tarla objesinin çocuğu (child) yapalım
            ekiliBitkiObjesi = Instantiate(ekilecekTohum.bitkiPrefabi, transform.position, Quaternion.identity);
            ekiliBitkiObjesi.transform.SetParent(this.transform);
        }

        Debug.Log(ekiliTohum.tohumAdi + " tarlaya ekildi.");
        
        // İleride büyüme mekaniğini buradan başlatabilirsiniz
        // StartCoroutine(BuyumeBaslat(ekiliTohum.buyumeSuresi));

        return true; // Ekim başarılı
    }

    // (İsteğe bağlı) Bitki büyüdüğünde hasat etmek için
    public void HasatEt()
    {
        if (!ekiliMi) return; // Ekili değilse hasat edilemez

        // (Büyüme kontrolü yapılmalı)

        Debug.Log(ekiliTohum.tohumAdi + " hasat edildi!");
        
        // Envantere ürün ekle
        // InventoryManager.instance.ItemEkle(ekiliTohum.hasatEdilenUrun, 1);

        // Tarlayı temizle
        Destroy(ekiliBitkiObjesi);
        ekiliMi = false;
        ekiliTohum = null;
        spriteRenderer.sprite = surulmusToprakSprite; // Tarlayı boş hale getir
    }
}