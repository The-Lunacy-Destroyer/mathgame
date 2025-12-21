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
    
    private bool _isStarted;
    
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
        if(!_isStarted) StartCoroutine(nameof(ChangeOpacity));
        else
        {
            StopCoroutine(nameof(ChangeOpacity));
            StartCoroutine(nameof(ChangeOpacity));
        }
    }
    IEnumerator ChangeOpacity()
    {
        _isStarted = true;
        while (ColorOpacity < maxOpacity)
        {
            yield return new WaitForSeconds(increaseOpacityRate);
            ColorOpacity += opacityIncrement;
        }
        while (ColorOpacity > 0)
        {
            yield return new WaitForSeconds(decreaseOpacityRate);
            ColorOpacity -= opacityDecrement;
        }
        _isStarted = false;
    }
}