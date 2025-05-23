using System;
using UnityEngine;

public class Potion : MonoBehaviour {
    public PotionData data;
    void Start() {
        Destroy(gameObject, data.potionTime); 
    }

    void OnMouseDown() {
        {
            if (GameManager.Instance == null || GameManager.Instance.IsGameOver)
                return;
            Collect();
        }
    }

    void Collect()
    {
        EventManager.Trigger("PotionCollectedEvent", data.potionName, data.potency, DateTime.Now);
        GameManager.Instance.AddScore((int)data.potency);
        Destroy(gameObject);
    }
}