using UnityEngine;
using System.Collections;

public class AreaSpawner : MonoBehaviour
{
    [Header("Goblin and Villager Prefabs")]
    public GameObject goblinPrefab;
    public GameObject villagerPrefab;

    [Header("Spawn Probability")]
    [Range(0f, 1f)] public float goblinSpawnChance = 0.5f;

    [Header("Timing Settings")]
    public float minSpawnTime = 1f;
    public float maxSpawnTime = 3f;

    private BoxCollider2D spawnArea;

    void Start()
    {
        spawnArea = GetComponent<BoxCollider2D>();
        if (spawnArea == null || !spawnArea.isTrigger)
        {
            Debug.LogWarning("Missing or non-trigger BoxCollider2D on spawn area.");
            return;
        }

        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);
            SpawnCharacter();
        }
    }

    void SpawnCharacter()
    {
        if (goblinPrefab == null || villagerPrefab == null) return;

        Bounds bounds = spawnArea.bounds;

        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float yPosition = bounds.center.y;

        Vector3 spawnPos = new Vector3(randomX, yPosition, 0f);
        GameObject prefab = (Random.value < goblinSpawnChance) ? goblinPrefab : villagerPrefab;

        Instantiate(prefab, spawnPos, Quaternion.identity);
    }
}
