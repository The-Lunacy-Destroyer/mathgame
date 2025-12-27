using System;
using System.Collections;
using System.Collections.Generic;
using InputCodeActions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputCodesController : MonoBehaviour
{
    public enum Actions
    {
        GodMode
    }
    
    [Serializable]
    public struct Code
    {
        public Key[] keySequence;
        public float codeInputDuration;
        public Actions action;
        
        public int NextKeyIndex { get; set; }
        public bool IsFirstKeyPressed { get; set; }
        public IEnumerator ResetNextKey { get; set; }
    }

    public Code[] codes;
    private EnableGodMode _godMode;
    
    private void Start()
    {
        _godMode = GetComponent<EnableGodMode>();
        _godMode.enabled = false;
    }
    
    private void Update()
    {
        for (int i = 0; i < codes.Length; i++)
        {
            if (codes[i].NextKeyIndex >= codes[i].keySequence.Length)
            {
                StopCoroutine(codes[i].ResetNextKey);
                codes[i].IsFirstKeyPressed = false;
                codes[i].NextKeyIndex = 0;
                
                SwitchActionState(codes[i].action);
            }
            else if (Keyboard.current[codes[i].keySequence[codes[i].NextKeyIndex]].wasPressedThisFrame)
            {
                if (!codes[i].IsFirstKeyPressed)
                {
                    codes[i].IsFirstKeyPressed = true;

                    codes[i].ResetNextKey = RemoveCodeFromExpected(i);
                    StartCoroutine(codes[i].ResetNextKey);
                }
                codes[i].NextKeyIndex++;
            }
        }
    }

    IEnumerator RemoveCodeFromExpected(int i)
    {
        yield return new WaitForSeconds(codes[i].codeInputDuration);
        codes[i].IsFirstKeyPressed = false;
        codes[i].NextKeyIndex = 0;
    }

    private void SwitchActionState(Actions action)
    {
        switch (action)
        {
            case Actions.GodMode:
                _godMode.enabled = !_godMode.enabled;
                string state = _godMode.enabled ? "enabled" : "disabled";
                Debug.Log($"God mode {state}");
                break;
            default:
                Debug.Log("No action");
                break;
        }
    }
}