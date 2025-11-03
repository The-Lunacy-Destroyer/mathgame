using System.Linq;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;
public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Vector2 offset;
    
    private Camera camera;
    public EntityController Source { get; set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        camera = Camera.main;
    }
    public void UpdateHealthBar(float curh, float maxh)
    {
        slider.value = curh / maxh;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = camera.transform.rotation;
        
        Vector2 pos = Source.transform.position;
        transform.position = pos + offset;
    }
}
