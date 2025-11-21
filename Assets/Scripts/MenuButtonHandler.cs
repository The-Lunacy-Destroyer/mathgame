using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuButtonHandler : MonoBehaviour
{
    private VisualElement _root;
    
    private Button _newGameButton;
    private Button _leaveButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        
        _newGameButton = _root.Q<Button>("NewGame");
        _leaveButton = _root.Q<Button>("Leave");
        
        _newGameButton.clicked += NewGame;
        _leaveButton.clicked += Leave;
    }

    private void OnDisable()
    {
        _newGameButton.clicked -= NewGame;
        _leaveButton.clicked -= Leave;
    }

    private void NewGame()
    {
        SceneManager.LoadScene("Game");
    }

    private void Leave()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
