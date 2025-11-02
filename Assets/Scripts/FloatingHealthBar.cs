using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;
public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void UpdateHealthBar(float curh, float maxh)
    {
        slider.value = curh / maxh;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
