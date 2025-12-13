using System.Collections;
using UnityEngine;

public class FlashMaskController : MonoBehaviour
{
    private SpriteRenderer _flashMaskSpriteRenderer;
    
    public float increaseOpacityRate = 0.025f;
    public float opacityIncrement = 0.11f;
    public float decreaseOpacityRate = 0.025f;
    public float opacityDecrement = 0.055f;
    public float maxOpacity = 0.2f;
    
    private float ColorOpacity
    {
        get => _flashMaskSpriteRenderer.color.a;
        set => _flashMaskSpriteRenderer.color = new Color(1, 1, 1, 
            Mathf.Clamp(value, 0f, maxOpacity));
    }
    
    private void Start()
    {
        _flashMaskSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }
    
    public void Flash()
    {
        StartCoroutine(nameof(IncreaseOpacity));
    }
    IEnumerator IncreaseOpacity()
    {
        while (ColorOpacity < maxOpacity)
        {
            yield return new WaitForSeconds(increaseOpacityRate);
            ColorOpacity += opacityIncrement;
        }
        StartCoroutine(nameof(DecreaseOpacity));
    }
    IEnumerator DecreaseOpacity()
    {
        while (ColorOpacity > 0)
        {
            yield return new WaitForSeconds(decreaseOpacityRate);
            ColorOpacity -= opacityDecrement;
        }
    }
}