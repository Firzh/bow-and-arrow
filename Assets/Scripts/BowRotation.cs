using UnityEngine;

public class BowRotation : MonoBehaviour
{
    // Kecepatan putar bow (opsional, untuk smoothing)
    public float rotationSpeed = 10f; 

    void Update()
    {
        // 1. Dapatkan posisi kursor di layar
        Vector3 mouseScreenPosition = Input.mousePosition;

        // 2. Konversi posisi kursor dari koordinat layar ke koordinat dunia Unity
        //    Penting: Kita perlu posisi Z agar kamera bisa mengkonversi dengan benar.
        //    Kita bisa menggunakan posisi Z dari Bow itu sendiri.
        mouseScreenPosition.z = Camera.main.WorldToScreenPoint(transform.position).z;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        // 3. Hitung arah dari Bow ke kursor
        Vector2 direction = mouseWorldPosition - transform.position;

        // 4. Hitung sudut rotasi (dalam derajat) dari arah tersebut
        //    Atan2 memberikan sudut dalam radian, jadi kita konversi ke derajat.
        //    Sudut 0 derajat Unity adalah ke kanan. Jika sprite bow Anda menghadap ke atas
        //    saat rotasi 0, Anda mungkin perlu menambahkan offset (misal +90 atau -90).
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 5. Sesuaikan offset rotasi (jika sprite bow Anda menghadap ke atas pada rotasi 0)
        //    Jika sprite bow Anda secara default menghadap ke atas (arah Y positif),
        //    maka Anda perlu mengurangi 90 derajat agar 0 derajat menjadi ke kanan (default Atan2).
        angle += 135f;

        // Terapkan offset yang bisa diatur di Inspector

        // 6. Buat Quaternion (representasi rotasi di Unity) dari sudut
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // 7. Terapkan rotasi ke Bow
        //    Gunakan Quaternion.Slerp untuk rotasi yang halus, atau langsung assign jika tidak perlu smoothing.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Jika Anda tidak ingin smoothing, cukup gunakan ini:
        // transform.rotation = targetRotation; 
    }
}