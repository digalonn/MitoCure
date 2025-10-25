using UnityEngine;
using TMPro; // TextMeshPro için
using UnityEngine.UI; // Button için
using System.Collections.Generic; // Queue için

public class DiyalogSistemi : MonoBehaviour
{
    public TextMeshProUGUI diyalogMetni;
    public Button devamButonu; // Tüm balonu kaplayan tıklanabilir alan
    public GameObject diyalogKutusu; // Balonun kendisi (açıp kapatmak için)

    // O an konuşulan cümlelerin sırası
    private Queue<string> cumleler;
    
    // Diyalog bittiğinde veya özel bir eylem gerektiğinde
    // BolumGorevManager'a haber verecek bir event
    public event System.Action DiyalogTiklandi;

    void Awake()
    {
        cumleler = new Queue<string>();
        devamButonu.onClick.AddListener(Tiklandi);
    }

    public void KonusmayiBaslat(string[] konusmaCumleleri)
    {
        cumleler.Clear();
        foreach (string cumle in konusmaCumleleri)
        {
            cumleler.Enqueue(cumle);
        }
        
        diyalogKutusu.SetActive(true);
        SonrakiCumleyiGoster();
    }

    public void TekCumleGoster(string cumle)
    {
        cumleler.Clear();
        cumleler.Enqueue(cumle); // Sadece 1 cümle ekle
        
        diyalogKutusu.SetActive(true);
        SonrakiCumleyiGoster();
    }

    public void Tiklandi()
    {
        // Önce event'i tetikle, BolumGorevManager durumu yönetsin
        DiyalogTiklandi?.Invoke();
    }
    
    public bool SonrakiCumleyiGoster()
    {
        if (cumleler.Count == 0)
        {
            // Konuşma bitti
            return false;
        }

        string cumle = cumleler.Dequeue();
        diyalogMetni.text = cumle;
        return true;
    }

    public void DiyalogKutusunuKapat()
    {
        diyalogKutusu.SetActive(false);
    }
}