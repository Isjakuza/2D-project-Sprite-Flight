using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float minSize = 0.5f;
    public float maxSize = 2.0f;
    public float minSpeed = 50f;
    public float maxSpeed = 150f;
    public float maxSpinSpeed = 10f;

    [Header("Shield Bounce Settings")]
    public float bounceForce = 300f;

    Rigidbody2D rb;
    void Start()
    {
        float randomSize = Random.Range(minSize, maxSize);
        transform.localScale = new Vector3(randomSize, randomSize, 1);

        rb = GetComponent<Rigidbody2D>();

        float randomSpeed = Random.Range(minSpeed, maxSpeed) / randomSize;
        Vector2 randomDirection = Random.insideUnitCircle;
        rb.AddForce(randomDirection * randomSpeed);

        float randomTorque = Random.Range(-maxSpinSpeed, maxSpeed);
        rb.AddTorque(randomTorque);
    }

    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "ShieldVisual" && other.gameObject.activeInHierarchy)
        {
            PlayerController player = other.GetComponentInParent<PlayerController>();

            if (player != null && player.hasShield)
            {
                if (rb != null)
                {
                    Vector2 bounceDirection = (transform.position - other.transform.position).normalized;
                    rb.linearVelocity = Vector2.zero;
                    rb.AddForce(bounceDirection * bounceForce);
                    float randomSpin = Random.Range(-100f, 100f);
                    rb.AddTorque(randomSpin);
                }
            }
        }
    }
}
