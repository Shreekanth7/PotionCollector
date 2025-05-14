using System;
using UnityEngine;

public class Potion : MonoBehaviour {
    public PotionData data;

    void Start() {
        Destroy(gameObject, 5f); // Auto despawn
    }

    void OnMouseDown() {
        EventManager.Trigger("PotionCollectedEvent", data.potionName, data.potency, DateTime.Now);
        GameManager.Instance.AddScore((int)data.potency);
        Destroy(gameObject);
    }
}