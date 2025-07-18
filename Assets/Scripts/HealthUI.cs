using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image fillImage;                     // Image yang akan di-fill (HP bar)
    public Vector3 offset = new Vector3(0, 1.5f, 0); // Offset dari posisi karakter

    private Camera mainCam;
    private Transform targetTransform;

    // Bisa salah satu
    private GoblinHealth goblinHealth;
    private VillagerHealth villagerHealth;

    void Start()
    {
        mainCam = Camera.main;

        // Coba cari ke parent
        goblinHealth = GetComponentInParent<GoblinHealth>();
        villagerHealth = GetComponentInParent<VillagerHealth>();

        // Simpan posisi parent untuk dijadikan anchor bar
        if (goblinHealth != null)
            targetTransform = goblinHealth.transform;
        else if (villagerHealth != null)
            targetTransform = villagerHealth.transform;
        else
            Debug.LogWarning("No health component found in parent.");
    }

    void Update()
    {
        if (targetTransform == null) return;

        // Update fill bar berdasarkan HP
        if (goblinHealth != null)
        {
            float fill = (float)goblinHealth.GetCurrentHealth() / goblinHealth.maxHealth;
            fillImage.fillAmount = fill;
        }
        else if (villagerHealth != null)
        {
            float fill = (float)villagerHealth.GetCurrentHealth() / villagerHealth.maxHealth;
            fillImage.fillAmount = fill;
        }

        // Posisi bar mengikuti karakter
        transform.position = targetTransform.position + offset;
    }
}
