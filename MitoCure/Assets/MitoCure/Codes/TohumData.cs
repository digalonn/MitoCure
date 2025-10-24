using System.Collections;
using System.Collections.Generic;
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
    
}
