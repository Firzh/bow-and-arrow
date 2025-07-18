using UnityEngine;
using TMPro; // Jika Anda menggunakan TextMeshPro untuk menampilkan Health

public class VillageHealthManager : MonoBehaviour
{
    public int currentHealth = 100; // Poin Health desa saat ini
    public TextMeshProUGUI healthText; // Referensi ke UI Text untuk menampilkan Health (Jika ada)

    // Pastikan GameObject ini memiliki Box Collider 2D dengan 'Is Trigger' dicentang
    // Pastikan juga GameObject ini memiliki Rigidbody 2D (Is Kinematic dicentang) untuk Trigger bekerja

    void Start()
    {
        UpdateHealthUI();
    }

    // Fungsi ini akan dipanggil ketika objek lain memasuki collider trigger ini
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Pastikan objek yang masuk adalah musuh (misalnya, melalui Tag)
        // Anda perlu memastikan prefab musuh (Goblin, Villager) memiliki Tag "Enemy"

        // Opsi 1: Deteksi berdasarkan Tag (Direkomendasikan)
        if (other.CompareTag("Enemy"))
        {
            ReduceHealth(10); // Kurangi Health (misal: 10 poin setiap kali musuh menembus)
            Destroy(other.gameObject); // Hancurkan musuh yang menembus
            Debug.Log("Musuh menembus desa! Health desa: " + currentHealth);

            // Cek Game Over
            if (currentHealth <= 0)
            {
                Debug.Log("GAME OVER! Desa telah hancur.");
                // Tambahkan logika Game Over di sini (misal: tampilkan layar Game Over, hentikan game)
            }
        }
        // Opsi 2: Deteksi berdasarkan Komponen (Kurang efisien jika banyak jenis musuh)
        // EnemyMovement enemy = other.GetComponent<EnemyMovement>();
        // if (enemy != null)
        // {
        //     ReduceHealth(10);
        //     Destroy(other.gameObject);
        //     Debug.Log("Musuh menembus desa! Health desa: " + currentHealth);
        //     if (currentHealth <= 0)
        //     {
        //         Debug.Log("GAME OVER! Desa telah hancur.");
        //     }
        // }
    }

    public void ReduceHealth(int amount)
    {
        currentHealth -= amount;
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + currentHealth.ToString();
        }
    }
}