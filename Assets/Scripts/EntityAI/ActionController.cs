using System;
using UnityEngine;

namespace EntityAI
{
    public class ActionController : MonoBehaviour
    {
        public enum AIState
        {
            Default,
            PreSpecial,
            Special,
            PostSpecial
        }

        private AIState _state = AIState.PostSpecial;
        public AIState State
        {
            get => _state;
            set
            {
                if (value > AIState.PostSpecial) _state = AIState.Default;
                else if (value < AIState.Default) _state = AIState.PostSpecial;
                else _state = value;
            }
        }

        private AIState _oldState;

        public int[] aiDurations = { 300, 50, 300, 30 };

        private int _aiTimer;
        public int AITimer
        {
            get => _aiTimer;
            set
            {
                _aiTimer = value;
                if (_aiTimer <= 0)
                {
                    _oldState = State;
                    State++;
                    _aiTimer = aiDurations[(int)State];
                }
            }
        }

        public event Action OnDefault;
        public event Action OnPreSpecial;
        public event Action OnSpecial;
        public event Action OnPostSpecial;
        
        public event Action OnDefaultStart;
        public event Action OnPreSpecialStart;
        public event Action OnSpecialStart;
        public event Action OnPostSpecialStart;
        
        private void Awake()
        {
            _aiTimer = 0;
        }
        
        private void FixedUpdate()
        {
            AITimer--;
            switch (State)
            {
                case AIState.Default:
                    if (_oldState != State)
                    {
                        OnDefaultStart?.Invoke();
                        _oldState = State;
                    }
                    OnDefault?.Invoke();
                    break;
                case AIState.PreSpecial:
                    if (_oldState != State)
                    {
                        OnPreSpecialStart?.Invoke();
                        _oldState = State;
                    }
                    OnPreSpecial?.Invoke();
                    break;                
                case AIState.Special:
                    if (_oldState != State)
                    {
                        OnSpecialStart?.Invoke();
                        _oldState = State;
                    }
                    OnSpecial?.Invoke();
                    break;                
                case AIState.PostSpecial:
                    if (_oldState != State)
                    {
                        OnPostSpecialStart?.Invoke();
                        _oldState = State;
                    }
                    OnPostSpecial?.Invoke();
                    break;
            }
        }
    }
}