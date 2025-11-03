using System.Linq;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;
public class FloatingHealthBar : MonoBehaviour
{
    public Slider slider;
    public Vector2 offset;
    
    public EntityController Source { get; set; }
    
    private Camera _camera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _camera = Camera.main;
    }
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        slider.value = currentHealth / maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = _camera.transform.rotation;
        Vector2 pos = Source.transform.position;
        transform.position = pos + offset;
    }
}
