using System.Collections;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    public int shakeTotalCount = 2;
    public int shakeIncreaseCount = 3;
    public int shakeDecreaseCount = 3;
    public float shakeDelay = 0.004f;
    public float minShake = 0.05f;
    public float maxShake = 0.075f;
    
    public void Shake()
    {
        StartCoroutine(nameof(ShakeEffect));
    }
    
    IEnumerator ShakeEffect()
    {
        float shake = Random.Range(minShake, maxShake);
        for (int i = 0; i < shakeTotalCount; i++)
        {
            for (int j = 0; j < shakeIncreaseCount; j++)
            {
                transform.position += new Vector3(shake / shakeIncreaseCount, shake / shakeIncreaseCount);
                yield return new WaitForSeconds(shakeDelay / shakeIncreaseCount);  
            }            
            for (int j = 0; j < shakeDecreaseCount; j++)
            {
                transform.position -= new Vector3(shake / shakeDecreaseCount, shake / shakeDecreaseCount);
                yield return new WaitForSeconds(shakeDelay / shakeDecreaseCount);  
            }
        }
    }
}
