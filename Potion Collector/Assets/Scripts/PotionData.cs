using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PotionData", menuName = "Potion/Create New Potion")]
public class PotionData : ScriptableObject {
    public string potionName;
    public int potency;
    public float potionTime;
    public GameObject prefab;
    [TextArea] public string description;
}
