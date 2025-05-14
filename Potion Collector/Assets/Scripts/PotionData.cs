using UnityEngine;

[CreateAssetMenu(fileName = "PotionData", menuName = "Potion/Create New Potion")]
public class PotionData : ScriptableObject {
    public string potionName;
    public float potency;
    public GameObject potionPrefab;
    [TextArea] public string description;
}
