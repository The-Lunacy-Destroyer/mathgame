using UnityEngine;

public class HealthDropController : MonoBehaviour
{
    public float healValue = 10f;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.CurrentHealth += healValue;
            Destroy(gameObject);
        }
    }
}
