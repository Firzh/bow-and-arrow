using UnityEngine;

public class ArrowRotation : MonoBehaviour
{
    [Header("Rotation Settings")]
    [Range(-180f, 180f)]
    public float rotationOffset = 0f; // Offset rotasi untuk menyesuaikan orientasi sprite
    public bool followMouse = false; // Apakah arrow mengikuti mouse (untuk standby arrow)
    public float rotationSpeed = 15f; // Kecepatan rotasi (untuk smoothing)
    public bool useSmoothing = true; // Apakah menggunakan smooth rotation
    
    [Header("Debug Settings")]
    public bool showDebugInfo = false; // Tampilkan info debug
    
    private bool isLaunched = false; // Status apakah arrow sudah ditembak
    private float targetAngle; // Target angle untuk rotasi
    
    void Update()
    {
        // Hanya rotasi jika belum ditembak dan followMouse aktif
        if (!isLaunched && followMouse)
        {
            RotateTowardsMouse();
        }
    }
    
    void RotateTowardsMouse()
    {
        if (Camera.main == null) return;
        
        // Dapatkan posisi mouse di dunia
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        
        // Hitung arah dari arrow ke mouse
        Vector2 direction = mouseWorldPosition - transform.position;
        
        // Hitung sudut rotasi
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        // Simpan target angle
        targetAngle = angle + rotationOffset;
        
        // Buat target rotation
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        
        // Terapkan rotasi (dengan atau tanpa smoothing)
        if (useSmoothing)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            transform.rotation = targetRotation;
        }
        
        if (showDebugInfo)
        {
            Debug.Log("Arrow rotating to mouse. Angle: " + targetAngle);
        }
    }
    
    // Method untuk set rotasi secara langsung (dipanggil saat menembak)
    public void SetRotationDirect(float angle)
    {
        targetAngle = angle + rotationOffset;
        transform.rotation = Quaternion.Euler(0, 0, targetAngle);
        
        if (showDebugInfo)
        {
            Debug.Log("Arrow rotation set directly to: " + targetAngle + " degrees");
        }
    }
    
    // Method untuk set rotasi ke arah mouse (dipanggil saat menembak)
    public void SetRotationToMouse()
    {
        if (Camera.main == null) return;
        
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        
        Vector2 direction = mouseWorldPosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        // Set rotasi dengan offset
        targetAngle = angle + rotationOffset;
        transform.rotation = Quaternion.Euler(0, 0, targetAngle);
        
        if (showDebugInfo)
        {
            Debug.Log("Arrow rotation set to mouse: " + targetAngle + " degrees");
        }
    }
    
    // Method untuk mendapatkan current target angle
    public float GetTargetAngle()
    {
        return targetAngle;
    }
    
    // Method untuk mengaktifkan/menonaktifkan follow mouse
    public void SetFollowMouse(bool follow)
    {
        followMouse = follow;
        
        if (showDebugInfo)
        {
            Debug.Log("Arrow follow mouse set to: " + follow);
        }
    }
    
    // Method untuk menandai arrow sudah ditembak
    public void SetLaunched(bool launched)
    {
        isLaunched = launched;
        if (launched)
        {
            followMouse = false; // Stop following mouse setelah ditembak
        }
        
        if (showDebugInfo)
        {
            Debug.Log("Arrow launched state set to: " + launched);
        }
    }
    
    // Method untuk mendapatkan direction vector berdasarkan current rotation
    public Vector3 GetDirectionVector()
    {
        float angleInRadians = targetAngle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians), 0);
    }
}