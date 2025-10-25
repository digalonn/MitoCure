using UnityEngine;

[CreateAssetMenu(fileName = "YeniGorev", menuName = "Bolum/Gorev Data")]
public class GorevData : ScriptableObject
{
    [Header("Görev Detayları")]
    public UrunData istenenUrun;
    public int istenenMiktar;
    
    [Header("Diyaloglar")]
    [TextArea(3, 5)]
    public string[] istekDiyaloglari; // Tıkladıkça ilerleyecek ilk konuşmalar
    
    [TextArea(2, 3)]
    public string tesekkurDiyalogu; // Doğru ürünü verince
    
    [TextArea(2, 3)]
    public string yanlisUrunDiyalogu; // Yanlış ürünü verince
    
    [TextArea(2, 3)]
    public string eksikUrunDiyalogu; // Doğru ürün ama eksik miktar
    
    [Header("Bölüm İlerlemesi")]
    public int tamamlanincaAcilacakSeviye; // Bu görev bitince hangi seviye açılacak
    public string sonrakiSahneAdi; // Teşekkürden sonra gidilecek sahne (Örn: "Level_2" veya "Farm")
}