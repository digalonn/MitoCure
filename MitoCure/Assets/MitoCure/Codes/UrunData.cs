using UnityEngine;

[CreateAssetMenu(fileName = "YeniUrun", menuName = "Envanter/Urun Data")]
public class UrunData : ScriptableObject
{
    public string urunAdi;
    public Sprite urunIkonu;
    public int satisFiyati; // Gelecekte bir dükkan için kullanabilirsiniz
}