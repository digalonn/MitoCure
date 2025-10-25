using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public TohumData tohumData; // Bu ikonun hangi tohumu temsil ettiğini belirtin (Inspector'dan atayın)

    private Image image;
    private Transform orjinalParent;
    private Vector3 orjinalPozisyon;
    private Canvas anaCanvas;

    // Sürüklerken ikonun fareyi takip etmesi için kullanacağız
    private GameObject suruklenenIkon;

    void Start()
    {
        image = GetComponent<Image>();
        orjinalParent = transform.parent;
        orjinalPozisyon = transform.localPosition;
        
        // Hiyerarşideki en üstteki Canvas'ı bul
        anaCanvas = GetComponentInParent<Canvas>(); 
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // --- YENİ EKLENEN KISIM: SEVİYE KONTROLÜ ---
        if (tohumData == null) 
        {
            Debug.LogError("BU SLOTTA TOHUM DATA ATANMAMIŞ! (None)");
            eventData.pointerDrag = null; // Sürüklemeyi iptal et
            return;
        }

        // --- KONTROL 2: Değerleri konsola yazdır ---
        int oyuncuSeviyesi = SeviyeManager.instance.GetMevcutSeviye();
        int gerekenSeviye = tohumData.gerekenSeviye;
        
        Debug.Log("SÜRÜKLEME BAŞLADI: Tohum: " + tohumData.tohumAdi + 
                  " | Gereken Seviye: " + gerekenSeviye + 
                  " | Oyuncu Seviyesi: " + oyuncuSeviyesi);
        // --- DEBUG KODU BİTTİ ---
        
        // SeviyeManager'dan kontrol et
        bool seviyeYeterli = SeviyeManager.instance.SeviyeYeterliMi(tohumData.gerekenSeviye);

        if (!seviyeYeterli)
        {
            // --- KONTROL 3: İptal bloğuna giriyor mu? ---
        Debug.LogWarning("SEVİYE YETERSİZ! Sürükleme iptal edildi.");
        // --- DEBUG KODU BİTTİ ---
        
        eventData.pointerDrag = null;
        
            Debug.Log(tohumData.tohumAdi + " ekmek için gereken seviye: " 
                                         + tohumData.gerekenSeviye + ". (Mevcut Seviye: " 
                                         + SeviyeManager.instance.GetMevcutSeviye() + ")");
            
            // (İsteğe bağlı: Ekranda bir uyarı sesi çal veya mesaj göster)
            // UyariManager.instance.Goster("Seviye Yetersiz!");
            
            // Sürüklemeyi tamamen iptal et.
            eventData.pointerDrag = null;
            return;
        }
        // --- SEVİYE KONTROLÜ BİTTİ ---
        // 1. Sürüklenen bir kopya ikon oluştur
        suruklenenIkon = new GameObject("DragIcon");
        suruklenenIkon.transform.SetParent(anaCanvas.transform, false); // Canvas'ın altına koy
        suruklenenIkon.transform.SetAsLastSibling(); // En üstte görünsün
        
        var ikonResmi = suruklenenIkon.AddComponent<Image>();
        ikonResmi.sprite = image.sprite;
        ikonResmi.rectTransform.sizeDelta = image.rectTransform.sizeDelta * 0.8f; // Biraz küçültebiliriz
        ikonResmi.raycastTarget = false; // Farenin altındaki diğer objeleri algılamasını engellemez

        // 2. Orjinal ikonu sürüklerken geçici olarak gizle (veya yarı saydam yap)
        image.color = new Color(1, 1, 1, 0.5f);
        
        // 3. Sürüklenen objeyi statik bir değişkende tutabiliriz (isteğe bağlı ama faydalı)
        // InventoryManager.instance.SuanSuruklenen = this;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Sürüklenen ikonu fare pozisyonuna taşı
        // Canvas'ın render moduna göre pozisyonu ayarlamak önemlidir.
        // Screen Space - Overlay için:
        suruklenenIkon.transform.position = Input.mousePosition;

        // Screen Space - Camera için (Daha karmaşık)
        // RectTransformUtility.ScreenPointToLocalPointInRectangle(...)
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 1. Sürüklenen kopya ikonu yok et
        Destroy(suruklenenIkon);

        // 2. Orjinal ikonun görünürlüğünü düzelt
        image.color = new Color(1, 1, 1, 1f);

        // 3. Nereye bırakıldığını kontrol et
        // Fare pozisyonunu Dünya (World) koordinatlarına çevir
        Vector2 dunyaPozisyonu = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Bu pozisyona bir ışın gönder (veya OverlapPoint kullan)
        RaycastHit2D hit = Physics2D.Raycast(dunyaPozisyonu, Vector2.zero);

        bool ekimBasarili = false;
        if (hit.collider != null && hit.collider.CompareTag("Tarla"))
        {
            // Bir tarla objesine çarptık!
            TarlaPlot tarlaScripti = hit.collider.GetComponent<TarlaPlot>();
            if (tarlaScripti != null)
            {
                // Tarlaya tohumu ekmeyi dene
                ekimBasarili = tarlaScripti.TohumEk(tohumData);
            }
        }

        // 4. Eğer ekim başarılı olmadıysa (veya geçersiz bir yere bırakıldıysa)
        if (!ekimBasarili)
        {
            // Eski pozisyonuna geri dön (Basitçe)
            transform.localPosition = orjinalPozisyon; 
        }
        else
        {
            // Başarılıysa: Envanterden bu tohumu azalt
            // (Bu kısım envanter yönetimi sisteminize bağlıdır)
            Debug.Log(tohumData.tohumAdi + " ekildi!");
            // Örnek: InventoryManager.instance.TohumKullan(tohumData, 1);
            // Şimdilik, slotu boşaltalım (Eğer envanter sistemi yoksa)
            // image.sprite = null;
            // image.enabled = false;
            // tohumData = null;
        }

        // Statik değişkeni temizle
        // InventoryManager.instance.SuanSuruklenen = null;
    }
}