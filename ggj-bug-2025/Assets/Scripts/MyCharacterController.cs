using UnityEngine;

public class MyCharacterController : MonoBehaviour
{
    public float moveSpeed = 5f; // Maksimum yürüme hızı
    public float acceleration = 2f; // Hızlanma oranı
    public float deceleration = 2f; // Yavaşlama oranı

    private float currentSpeed = 0f; // Mevcut hareket hızı
    private float inputDirection = 0f; // Oyuncunun hareket ettiği yön (-1 = sol, 1 = sağ)
    private Rigidbody rb;

    void Start()
    {
        // Rigidbody bileşenini al
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 1. Oyuncunun girişlerini al (A, D veya Ok Tuşları)
        float horizontalInput = Input.GetAxisRaw("Horizontal"); // -1 (sol), 0 (dur), 1 (sağ)

        // 2. Yönü belirle
        if (horizontalInput != 0)
        {
            inputDirection = horizontalInput; // Hareket eden yöne göre güncelle
            currentSpeed = Mathf.MoveTowards(currentSpeed, moveSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            // Hareket durduysa hızı yavaşça sıfıra yaklaştır
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        // 3. Rigidbody üzerinden hareketi uygula
        Vector2 velocity = new Vector2(inputDirection * currentSpeed, rb.velocity.y);
        rb.velocity = velocity;
    }
}