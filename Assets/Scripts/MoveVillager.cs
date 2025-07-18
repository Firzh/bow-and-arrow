using UnityEngine;

public class MoveVillager : MonoBehaviour
{
    [Header("Movement Speed")]
    public float speed = 2f;

    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }
}
