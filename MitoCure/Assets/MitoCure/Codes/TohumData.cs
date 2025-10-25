using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "TohumData", menuName = "Envar/TohumData")]
public class TohumData : ScriptableObject
{
    
    public string tohumAdi;
    public Sprite tohumIkonu;
    public GameObject bitkiPrefabi; // Bu tohum ekildiğinde büyüyecek bitkinin prefabı
    // İleride ekleyebilirsiniz:
    // public int buyumeSuresi;
    // public Sprite olgunlasmisBitkiSprite;
    
    [Header("Büyüme Bilgileri")]
    public float buyumeSuresiSaniye = 10f; // Bitkinin tam olgunluğa erişme süresi (saniye)
    
    // Büyüme aşamalarını (fide, genç bitki, olgun bitki) göstermek için
    // [0] = Fide, [1] = Genç, [2] = Olgun (Hasat edilebilir)
    public Sprite[] buyumeAsamalari; 

    [Header("Hasat Bilgisi")]
    public GameObject hasatEdilenUrunPrefabi; // Hasat edince envantere eklenecek ürün
    public int hasatMiktari = 1;


   
}
