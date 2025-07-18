using UnityEngine;

public class BowController : MonoBehaviour
{
    [Header("Arrow Settings")]
    public GameObject arrowPrefab; // Prefab anak panah
    public Transform arrowSpawnPoint; // Titik dari mana anak panah akan muncul
    
    [Header("Shooting Settings")]
    public float fireRate = 0.5f; // Interval antar tembakan (detik)
    public bool autoFire = true; // Apakah otomatis menembak saat hold klik
    
    [Header("Debug Settings")]
    public bool showDebugInfo = true; // Tampilkan info debug
    
    private float nextFireTime = 0f;
    private bool isMouseHeld = false; // Status apakah mouse sedang ditekan
    private BowRotation bowRotationScript;
    private GameObject currentArrowOnBow; // Referensi ke panah yang sedang standby di busur

    void Awake()
    {
        bowRotationScript = GetComponent<BowRotation>();
        if (bowRotationScript == null && showDebugInfo)
        {
            Debug.LogWarning("BowController: No BowRotation script found on the same GameObject. This is optional.");
        }
    }

    void Start()
    {
        if (arrowSpawnPoint == null)
        {
            Debug.LogError("Arrow Spawn Point belum diatur di BowController! Defaulting to bow's transform.", this);
            arrowSpawnPoint = transform; // Default ke posisi busur itu sendiri
        }

        if (arrowPrefab == null)
        {
            Debug.LogError("Arrow Prefab belum diatur di BowController! Tidak bisa menembak panah.", this);
            return;
        }

        SetupInitialArrow();
    }

    void Update()
    {
        HandleMouseInput();
        
        // Jika mouse ditekan dan hold, dan autoFire aktif, tembak dengan interval
        if (isMouseHeld && autoFire && Time.time >= nextFireTime)
        {
            ShootArrow();
            nextFireTime = Time.time + fireRate;
        }
    }
    
    void HandleMouseInput()
    {
        // Deteksi mouse button down
        if (Input.GetMouseButtonDown(0))
        {
            isMouseHeld = true;
            
            // Tembak langsung saat pertama kali klik
            if (Time.time >= nextFireTime)
            {
                ShootArrow();
                nextFireTime = Time.time + fireRate;
            }
        }
        
        // Deteksi mouse button up
        if (Input.GetMouseButtonUp(0))
        {
            isMouseHeld = false;
        }
    }

    void SetupInitialArrow()
    {
        if (arrowPrefab == null) return;

        // Pastikan tidak ada panah lain di busur sebelum menyiapkan yang baru
        if (currentArrowOnBow != null)
        {
            Destroy(currentArrowOnBow);
        }

        // Instansiasi panah baru sebagai child dari arrowSpawnPoint
        currentArrowOnBow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.identity, arrowSpawnPoint);
        currentArrowOnBow.transform.localPosition = Vector3.zero;
        currentArrowOnBow.transform.localRotation = Quaternion.identity;

        // Setup komponen-komponen arrow
        ArrowBehavior arrowBehav = currentArrowOnBow.GetComponent<ArrowBehavior>();
        if (arrowBehav != null)
        {
            arrowBehav.enabled = true; // Enable script tapi belum launched
            arrowBehav.SetupStandbyArrow(); // Setup untuk standby
        }

        Collider2D arrowCollider = currentArrowOnBow.GetComponent<Collider2D>();
        if (arrowCollider != null)
        {
            arrowCollider.enabled = false; // Disable collision saat standby
        }
        
        // Setup ArrowRotation script untuk standby arrow
        ArrowRotation arrowRotation = currentArrowOnBow.GetComponent<ArrowRotation>();
        if (arrowRotation != null)
        {
            arrowRotation.SetFollowMouse(true); // Arrow mengikuti mouse saat standby
            arrowRotation.SetLaunched(false);
        }

        if (showDebugInfo)
        {
            Debug.Log("Arrow prepared on bow. Ready to shoot!");
        }
    }

    void ShootArrow()
    {
        if (currentArrowOnBow == null)
        {
            Debug.LogError("No arrow currently on the bow to shoot!");
            SetupInitialArrow(); // Coba setup ulang
            return;
        }
        
        if (Camera.main == null)
        {
            Debug.LogError("Main Camera not found! Make sure your camera GameObject is tagged as 'MainCamera'.");
            return;
        }

        if (showDebugInfo)
        {
            Debug.Log("Shooting arrow...");
        }

        // Hitung arah ke mouse dari titik spawn panah
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.WorldToScreenPoint(arrowSpawnPoint.position).z;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 shootDirection = (mouseWorldPosition - arrowSpawnPoint.position).normalized;

        // Lepaskan panah dari busur (ubah parent ke null)
        currentArrowOnBow.transform.SetParent(null);
        
        // Pindahkan panah ke posisi spawn point (karena sudah tidak child lagi)
        currentArrowOnBow.transform.position = arrowSpawnPoint.position;

        // Aktifkan collider untuk deteksi tabrakan
        Collider2D arrowCollider = currentArrowOnBow.GetComponent<Collider2D>();
        if (arrowCollider != null)
        {
            arrowCollider.enabled = true;
        }

        // Launch arrow dengan arah yang sudah dihitung
        ArrowBehavior arrowBehav = currentArrowOnBow.GetComponent<ArrowBehavior>();
        if (arrowBehav != null)
        {
            arrowBehav.LaunchArrow(shootDirection); // Launch dengan direction vector
        }

        if (showDebugInfo)
        {
            Debug.Log("Arrow shot successfully! Direction: " + shootDirection);
        }

        // Reset referensi dan siapkan panah baru
        currentArrowOnBow = null;
        
        // Delay sedikit sebelum setup arrow baru untuk menghindari conflict
        Invoke("SetupInitialArrow", 0.1f);
    }
    
    // Method untuk toggle auto fire (bisa dipanggil dari UI atau script lain)
    public void SetAutoFire(bool enabled)
    {
        autoFire = enabled;
        
        if (showDebugInfo)
        {
            Debug.Log("Auto fire set to: " + enabled);
        }
    }
    
    // Method untuk mengubah fire rate
    public void SetFireRate(float newFireRate)
    {
        fireRate = Mathf.Max(0.1f, newFireRate); // Minimum 0.1 detik
        
        if (showDebugInfo)
        {
            Debug.Log("Fire rate set to: " + fireRate);
        }
    }
    
    // Method untuk manual shoot (bisa dipanggil dari script lain)
    public void ManualShoot()
    {
        if (Time.time >= nextFireTime)
        {
            ShootArrow();
            nextFireTime = Time.time + fireRate;
        }
    }
}