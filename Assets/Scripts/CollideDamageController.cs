using Health;
using UnityEngine;

public class CollideDamageController : MonoBehaviour
{
    public float minDamageSpeed = 8f;
    public float maxDamageSpeed = 50f;
    public float damageScale = 0.75f;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        float speed = other.relativeVelocity.magnitude;
        if (speed < minDamageSpeed) return;
            
        EntityHealthController entityHealth = other.gameObject.GetComponent<EntityHealthController>();
        if (entityHealth)
        {
            entityHealth.CurrentHealth -= 
                Mathf.Clamp(speed, minDamageSpeed, maxDamageSpeed) * other.otherRigidbody.mass * damageScale;
        }
    }
}
