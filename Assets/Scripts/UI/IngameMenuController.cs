using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class IngameMenuController : MonoBehaviour
    {
        private GameObject _ingameMenu;

        void Start()
        {
            _ingameMenu = GameObject.Find("IngameMenu");
            _ingameMenu?.SetActive(false);
        }
        
        void Update()
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                _ingameMenu.SetActive(!_ingameMenu.activeSelf);
            }
        }
        
        void OnDestroy()
        {
            if (_ingameMenu && !_ingameMenu.activeSelf)
            {
                _ingameMenu.SetActive(true);
            }
        }
    }
}