using UnityEngine;

public class VillagerHealth : MonoBehaviour
{
    [Header("Villager Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // Method untuk menerima damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Current HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Metode ketika villager mati
    void Die()
    {
        Debug.Log($"{gameObject.name} died!");
        // Bisa tambahkan animasi kabur atau efek lainnya di sini
        Destroy(gameObject);
    }

    // Heal (opsional)
    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        Debug.Log($"{gameObject.name} healed. Current HP: {currentHealth}");
    }

    // Getter untuk HP saat ini
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
