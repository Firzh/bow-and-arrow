// using UnityEngine;
// using System.Collections; // Untuk Coroutine

// public class EnemySpawner : MonoBehaviour
// {
//     public GameObject[] enemyPrefabs; // Array untuk menyimpan prefab Goblin dan Villager
//     public Transform enemyDestination; // Titik tujuan musuh
//     public float spawnInterval = 2f; // Interval waktu antar kemunculan musuh
//     public float minSpawnX; // Batas kiri area spawn (bisa diatur manual atau berdasarkan collider)
//     public float maxSpawnX; // Batas kanan area spawn (bisa diatur manual atau berdasarkan collider)

//     // Opsional: Untuk mendapatkan batas X dari Collider 2D
//     private BoxCollider2D spawnAreaCollider;

//     void Start()
//     {
//         spawnAreaCollider = GetComponent<BoxCollider2D>();
//         if (spawnAreaCollider != null)
//         {
//             // Menghitung batas X dari collider jika ada
//             minSpawnX = transform.position.x + spawnAreaCollider.offset.x - spawnAreaCollider.size.x / 2f;
//             maxSpawnX = transform.position.x + spawnAreaCollider.offset.x + spawnAreaCollider.size.x / 2f;
//         }
//         else
//         {
//             Debug.LogWarning("EnemySpawnArea tidak memiliki BoxCollider2D. Pastikan minSpawnX dan maxSpawnX diatur secara manual di Inspector.");
//             // Atur nilai default jika tidak ada collider dan belum diatur manual
//             if (minSpawnX == 0 && maxSpawnX == 0)
//             {
//                 minSpawnX = -5f; // Contoh nilai default
//                 maxSpawnX = 5f;  // Contoh nilai default
//             }
//         }

//         StartCoroutine(SpawnEnemiesRoutine());
//     }

//     IEnumerator SpawnEnemiesRoutine()
//     {
//         while (true) // Loop tanpa henti untuk terus spawn musuh
//         {
//             SpawnEnemy();
//             yield return new WaitForSeconds(spawnInterval);
//         }
//     }

//     void SpawnEnemy()
//     {
//         if (enemyPrefabs.Length == 0)
//         {
//             Debug.LogError("Tidak ada prefab musuh yang diatur di EnemySpawner!");
//             return;
//         }

//         // Pilih prefab musuh secara acak dari array
//         GameObject enemyToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

//         // Tentukan posisi X acak di dalam area spawn
//         float randomX = Random.Range(minSpawnX, maxSpawnX);
//         Vector3 spawnPosition = new Vector3(randomX, transform.position.y, transform.position.z);

//         // Instansiasi musuh
//         GameObject newEnemy = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);

//         // Jika musuh memiliki script pergerakan, berikan targetnya
//         EnemyMovement enemyMovement = newEnemy.GetComponent<EnemyMovement>();
//         if (enemyMovement != null && enemyDestination != null)
//         {
//             enemyMovement.SetTarget(enemyDestination.position);
//         }
//         else if (enemyMovement == null)
//         {
//             Debug.LogWarning("Prefab musuh " + enemyToSpawn.name + " tidak memiliki komponen EnemyMovement.");
//         }
//         else if (enemyDestination == null)
//         {
//             Debug.LogError("EnemyDestination belum diatur di EnemySpawner!");
//         }
//     }
// }