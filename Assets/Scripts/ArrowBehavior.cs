using UnityEngine;

public class ArrowBehavior : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 15f; // Kecepatan anak panah
    public float lifetime = 10f; // Berapa lama anak panah akan ada sebelum dihancurkan
    
    [Header("Debug Settings")]
    public bool showDebugInfo = true; // Tampilkan info debug
    
    private bool isLaunched = false; // Flag untuk mengecek apakah panah sudah ditembak
    private ArrowRotation arrowRotation; // Referensi ke script rotasi
    private Vector3 moveDirection; // Arah gerakan panah
    private float launchTime; // Waktu saat panah ditembak
    
    void Awake()
    {
        arrowRotation = GetComponent<ArrowRotation>();
    }
    
    void Start()
    {
        // Jangan langsung destroy di Start, tunggu sampai benar-benar launched
        if (showDebugInfo)
        {
            Debug.Log("Arrow created. Launched: " + isLaunched);
        }
    }

    void Update()
    {
        // Hanya bergerak jika panah sudah ditembak
        if (isLaunched)
        {
            // Gerakkan arrow berdasarkan direction yang sudah dihitung
            transform.position += moveDirection * speed * Time.deltaTime;
            
            // Cek apakah sudah saatnya dihancurkan
            if (Time.time - launchTime >= lifetime)
            {
                if (showDebugInfo)
                {
                    Debug.Log("Arrow destroyed after lifetime: " + lifetime + " seconds");
                }
                Destroy(gameObject);
            }
            
            // Cek apakah arrow sudah terlalu jauh dari screen (safety check)
            if (Vector3.Distance(transform.position, Camera.main.transform.position) > 50f)
            {
                if (showDebugInfo)
                {
                    Debug.Log("Arrow destroyed - too far from camera");
                }
                Destroy(gameObject);
            }
        }
    }

    // Method ini dipanggil dari BowController saat menembak
    public void LaunchArrow(Vector3 targetDirection)
    {
        // Set arah gerakan berdasarkan parameter
        moveDirection = targetDirection.normalized;
        
        // Set rotasi ke arah gerakan
        if (arrowRotation != null)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            arrowRotation.SetRotationDirect(angle);
            arrowRotation.SetLaunched(true);
        }
        else
        {
            // Jika tidak ada ArrowRotation, set rotasi langsung
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        
        // Set flag bahwa panah sudah ditembak
        isLaunched = true;
        launchTime = Time.time;
        
        if (showDebugInfo)
        {
            Debug.Log("Arrow launched! Direction: " + moveDirection + ", Speed: " + speed);
        }
    }
    
    // Method untuk setup arrow standby (belum ditembak)
    public void SetupStandbyArrow()
    {
        isLaunched = false;
        
        if (arrowRotation != null)
        {
            arrowRotation.SetFollowMouse(true);
            arrowRotation.SetLaunched(false);
        }
        
        if (showDebugInfo)
        {
            Debug.Log("Arrow setup as standby");
        }
    }
    
    // Method untuk mengecek apakah arrow sudah ditembak
    public bool IsLaunched()
    {
        return isLaunched;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Hanya deteksi collision jika panah sudah ditembak
        if (!isLaunched) return;

        if (other.CompareTag("Enemy"))
        {
            if (showDebugInfo)
            {
                Debug.Log("Arrow hit an enemy: " + other.gameObject.name);
            }
            Destroy(gameObject);
        }
        else if (!other.CompareTag("Village") && !other.CompareTag("Arrow") && !other.CompareTag("Player"))
        {
            if (showDebugInfo)
            {
                Debug.Log("Arrow hit something else: " + other.gameObject.name);
            }
            Destroy(gameObject);
        }
    }
    
    // Method untuk debug info
    void OnDrawGizmos()
    {
        if (isLaunched && showDebugInfo)
        {
            // Gambar garis untuk menunjukkan arah gerakan
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + moveDirection * 2f);
        }
    }
}