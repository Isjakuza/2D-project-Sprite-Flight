using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float speed = 3f; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                player.ActivateShield();
                Destroy(gameObject); 
            }
        }
    }

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);

        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }
}