using UnityEngine;

public class GoblinHealth : MonoBehaviour
{
    [Header("Goblin Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // Method to apply damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Current HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Method when Goblin dies
    void Die()
    {
        Debug.Log($"{gameObject.name} died!");
        // Tambahkan animasi/musik efek di sini jika perlu
        Destroy(gameObject);
    }

    // OPTIONAL: Method to heal
    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        Debug.Log($"{gameObject.name} healed. Current HP: {currentHealth}");
    }

    // Getter method (optional)
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
