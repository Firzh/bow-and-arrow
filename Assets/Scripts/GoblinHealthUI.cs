using UnityEngine;
using UnityEngine.UI;

public class GoblinHealthUI : MonoBehaviour
{
    public GoblinHealth goblinHealth;       // Referensi ke GoblinHealth.cs
    public Image fillImage;                 // Bar yang berubah (fill hijau)
    public Vector3 offset = new Vector3(0, 1.5f, 0);  // Posisi di atas goblin

    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;

        if (goblinHealth == null)
        {
            goblinHealth = GetComponentInParent<GoblinHealth>();
        }
    }

    void Update()
    {
        if (goblinHealth != null && fillImage != null)
        {
            float fill = (float)goblinHealth.GetCurrentHealth() / goblinHealth.maxHealth;
            fillImage.fillAmount = fill;
        }

        // Posisi HP Bar mengikuti goblin di layar
        Vector3 worldPos = goblinHealth.transform.position + offset;
        transform.position = worldPos;
    }
}
