using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;

public class PotionSpawner : MonoBehaviour
{
    public List<string> potionLabels = new() { "potion_blue", "potion_green" , "potion_pink", "potion_red", "potion_yellow" };
    public float spawnInterval = 3f;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnPotion), 1f, spawnInterval);
    }

    void SpawnPotion()
    {
        string label = potionLabels[Random.Range(0, potionLabels.Count)];
        Addressables.LoadAssetAsync<GameObject>(label).Completed += OnPotionLoaded;
    }

    void OnPotionLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject potionPrefab = handle.Result;
            Vector3 pos = GetRandomPosition();
            GameObject potion = Instantiate(potionPrefab, pos, Quaternion.identity);
            EventManager.Trigger("PotionSpawnedEvent", potion.name, pos);
        }
        else
        {
            Debug.LogError("Failed to load potion addressable.");
        }
    }

    Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
    }
}