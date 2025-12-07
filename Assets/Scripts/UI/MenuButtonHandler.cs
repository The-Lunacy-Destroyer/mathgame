using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI
{
    public class MenuButtonHandler : MonoBehaviour
    {
        private VisualElement _root;
    
        private Button _newGameButton;
        private Button _leaveButton;
        private Button _leaveToMenuButton;
    
        private void OnEnable()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
        
            _newGameButton = _root.Q<Button>("NewGame");
            _leaveButton = _root.Q<Button>("Leave");
            _leaveToMenuButton = _root.Q<Button>("LeaveToMenu");
        
            if(_newGameButton != null)
                _newGameButton.clicked += NewGame;
            if(_leaveButton != null)
                _leaveButton.clicked += Leave;
            if(_leaveToMenuButton != null)
                _leaveToMenuButton.clicked += LeaveToMenu;
        }

        private void OnDisable()
        {
            if(_newGameButton != null)
                _newGameButton.clicked -= NewGame;
            if(_leaveButton != null)
                _leaveButton.clicked -= Leave;
            if(_leaveToMenuButton != null)
                _leaveToMenuButton.clicked -= LeaveToMenu;
        }

        private void NewGame()
        {
            SceneManager.LoadScene("Game");
            Time.timeScale = 1;
        }

        private void Leave()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void LeaveToMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
