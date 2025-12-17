using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class WaveSystem : MonoBehaviour
{
    private AudioSource _gameMusic;
    
    [Serializable]
    public struct WaveInfo
    {
        [Min(1)] public int waveDuration;
        [Min(1)] public int waveBreakDuration;
        public AudioClip waveMusic;
    }

    public WaveInfo[] waves;
    private int _waveCount;
    private int _currentWave = -1;

    private int _waveTimer;
    private int _breakTimer = 5;

    private int WaveTimer
    {
        get => _waveTimer;
        set
        {
            if (value <= 0)
            {
                _breakTimer = waves[_currentWave].waveBreakDuration;
                _timerText.text = $"Break timer: {_breakTimer}";
                _waveTimer = 0;
            }
            else
            {
                _waveTimer = value;
                _timerText.text = $"Wave timer: {_waveTimer}";
            }
        }
    }
    private int BreakTimer
    {
        get => _breakTimer;
        set
        {
            if (value <= 0)
            {
                if (_currentWave < _waveCount - 1)
                {
                    _currentWave++;
                    _waveText.text = $"Wave: {_currentWave + 1}";
                    
                    _waveTimer = waves[_currentWave].waveDuration;
                    _timerText.text = $"Wave timer: {_waveTimer}";
                    SpawnEnemies();

                    if (waves[_currentWave].waveMusic)
                    {
                        _gameMusic.Stop();
                        _gameMusic.clip = waves[_currentWave].waveMusic;
                        _gameMusic.Play();
                    }
                }
                _breakTimer = 0;
            }
            else
            {
                _breakTimer = value;
                _timerText.text = $"Break timer: {_breakTimer}";
            }
        }
    }

    public UIDocument waveUI;
    private Label _waveText;
    private Label _timerText;

    private void Awake()
    {
        _waveCount = Mathf.Min(waves.Length, transform.childCount);
        _gameMusic = Camera.main?.GetComponent<AudioSource>();
    }

    private void Start()
    {
        ControlWaves();
        _waveText = waveUI.rootVisualElement.Q<Label>("WaveLabel");
        _timerText = waveUI.rootVisualElement.Q<Label>("TimerLabel");
        _waveText.text = $"Wave: {_currentWave + 1}";
        _timerText.text = $"Break timer: {_breakTimer}";
    }

    private void ControlWaves()
    {
        if (BreakTimer == 0 && transform.GetChild(_currentWave).childCount == 0)
        {
            WaveTimer = 0;
            ControlWaves();
        }
        else if (BreakTimer > 0) StartCoroutine(nameof(DecreaseBreakTimer));
        else if (WaveTimer > 0) StartCoroutine(nameof(DecreaseWaveTimer));
    }

    IEnumerator DecreaseWaveTimer()
    {
        yield return new WaitForSeconds(1f);
        WaveTimer--;
        ControlWaves();
    }
    IEnumerator DecreaseBreakTimer()
    {
        yield return new WaitForSeconds(1f);
        BreakTimer--;
        ControlWaves();
    }

    private void SpawnEnemies()
    {
        transform.GetChild(_currentWave).gameObject.SetActive(true);
    }
}
