using UnityEngine;
using UnityEngine.SceneManagement; // Sahne geçişi için

public class BolumGorevManager : MonoBehaviour
{
    [Header("Görev")]
    public GorevData mevcutGorev; // Inspector'dan "Level_1_Gorev"i buraya sürükleyin

    [Header("Bağlantılar")]
    public DiyalogSistemi diyalogSistemi;

    // Görevin o anki durumunu takip etmek için
    private enum GorevDurumu { Baslangic, IstekYapiliyor, Beklemede, YanlisUrun, DogruUrun }
    private GorevDurumu durum;

    void Start()
    {
        if (mevcutGorev == null || diyalogSistemi == null)
        {
            Debug.LogError("Gorev Manager'da GorevData veya DiyalogSistemi eksik!");
            return;
        }

        // Diyalog sisteminin 'Tiklandi' event'ine abone ol
        diyalogSistemi.DiyalogTiklandi += DiyalogaTiklandi;
        
        // Bölüme başla
        durum = GorevDurumu.Baslangic;
        diyalogSistemi.KonusmayiBaslat(mevcutGorev.istekDiyaloglari);
    }

    private void DiyalogaTiklandi()
    {
        switch (durum)
        {
            case GorevDurumu.Baslangic:
            case GorevDurumu.IstekYapiliyor:
                // Diyalog devam ediyorsa
                if (diyalogSistemi.SonrakiCumleyiGoster())
                {
                    durum = GorevDurumu.IstekYapiliyor;
                }
                else // İstek diyalogları bittiyse
                {
                    durum = GorevDurumu.Beklemede;
                    // (İsteğe bağlı) Konuşma balonu kapanabilir
                    // diyalogSistemi.DiyalogKutusunuKapat();
                }
                break;
            
            case GorevDurumu.Beklemede:
                // Bekleme durumunda tıklamanın bir etkisi yok (veya balon kapalı)
                break;
            
            case GorevDurumu.YanlisUrun:
                // "Bu ürünü istemedim" yazısına tıklandı.
                // İsteği tekrar göster.
                durum = GorevDurumu.Beklemede;
                string anaIstek = mevcutGorev.istekDiyaloglari[mevcutGorev.istekDiyaloglari.Length - 1]; // İsteğin son cümlesi
                diyalogSistemi.TekCumleGoster(anaIstek);
                break;
            
            case GorevDurumu.DogruUrun:
                // "Teşekkürler" yazısına tıklandı.
                // Seviyeyi tamamla ve sonraki sahneye geç.
                SeviyeManager.instance.SeviyeAyarla(mevcutGorev.tamamlanincaAcilacakSeviye);
                SceneManager.LoadScene(mevcutGorev.sonrakiSahneAdi);
                break;
        }
    }

    /// <summary>
    /// Envanter slotları tarafından çağrılacak ana fonksiyon.
    /// </summary>
    public void UrunTeslimEtmeyiDene(UrunData verilenUrun)
    {
        // Sadece bekleme durumundaysa veya yanlış ürün verdiyse ürün kabul et
        if (durum != GorevDurumu.Beklemede && durum != GorevDurumu.YanlisUrun)
            return;
            
        // Envanterden 1 adet ürünü kullan (düş)
        bool envanterdeVardi = EnvanterManager.instance.UrunKullan(verilenUrun, 1);

        if (!envanterdeVardi)
        {
            // Bu normalde olmamalı, çünkü slotlar miktarı 0 olanı tıklatmamalı.
            Debug.LogWarning("Olmayan bir ürüne tıklandı?");
            return;
        }

        // --- Kontrol ---
        // 1. Ürün Tipi Doğru mu?
        if (verilenUrun == mevcutGorev.istenenUrun)
        {
            // 2. Miktar Yeterli mi? (İstenen miktar 3 ise, 1 verdikten sonra hâlâ 2 tane daha olmalı)
            // Bizden 3 istiyor, 1 verdik, envanterde kalan 2 ise, toplamda 3'ümüz varmış demektir.
            int envanterdeKalanMiktar = EnvanterManager.instance.GetUrunMiktari(verilenUrun);
            int istenenEksikMiktar = mevcutGorev.istenenMiktar - 1; // 1 tanesini az önce verdik

            if (envanterdeKalanMiktar >= istenenEksikMiktar)
            {
                // BAŞARILI!
                // Geri kalan miktarı da envanterden düş
                EnvanterManager.instance.UrunKullan(verilenUrun, istenenEksikMiktar);
                
                durum = GorevDurumu.DogruUrun;
                diyalogSistemi.TekCumleGoster(mevcutGorev.tesekkurDiyalogu);
            }
            else
            {
                // Ürün doğru ama miktar yetersiz. (Zaten 1 tanesi gitti)
                durum = GorevDurumu.YanlisUrun; // Yanlış ürünle aynı duruma düşsün
                
                // Özel eksik miktar mesajı
                string eksikMesaji = mevcutGorev.eksikUrunDiyalogu;
                eksikMesaji = eksikMesaji.Replace("[MİKTAR]", (envanterdeKalanMiktar + 1).ToString()); // Elinde toplam kaç tane olduğunu söyle
                diyalogSistemi.TekCumleGoster(eksikMesaji);
            }
        }
        else // 1. Yanlış Ürün
        {
            // YANLIŞ ÜRÜN! (Ürün zaten envanterden 1 adet düştü)
            durum = GorevDurumu.YanlisUrun;
            diyalogSistemi.TekCumleGoster(mevcutGorev.yanlisUrunDiyalogu);
        }
    }
    
    // Event aboneliğini iptal etmeyi unutma
    void OnDestroy()
    {
        if (diyalogSistemi != null)
        {
            diyalogSistemi.DiyalogTiklandi -= DiyalogaTiklandi;
        }
    }
}